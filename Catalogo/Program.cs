using Catalogo.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using Catalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Catalogo.Models.Extensions;
using Catalogo.Logging;
using Catalogo.Filters;
using Catalogo.Repositories;
using Catalogo.DTOs.Mappins;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Catalogo.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.AllowAnyOrigin().WithMethods("GET");

                      });
});




// Add services to the container.
//remove o limitador de caracters retornado do json e adiciona um tratador de excecoes global com filtros
builder.Services.AddControllers(options =>
{
    //options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson(); // Adiciona suporte ao JsonPatchDocument<T>

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalogo API",
        Version = "v1",
        Description = "API para gerenciamento de catálogo de produtos"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//adciona o tempo de vida do objeto
builder.Services.AddTransient<IMeuService, MeuSevico>();

//desabilita o fromservice
builder.Services.Configure<ApiBehaviorOptions>(options =>
options.DisableImplicitFromServicesParameters = true
);

builder.Services.AddIdentity<AplicationUsers, IdentityRole>().
    AddEntityFrameworkStores<AppDbContext>().
    AddDefaultTokenProviders();

//Autentificação bearer jwt
var SecretKey = builder.Configuration["JWT:Secretkey"] ?? throw new ArgumentNullException("secret key is invalid!");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireRole("id", "Kaylan"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("ExclusivePolicyOnly", policy =>
    policy.RequireAssertion(context => context.User.HasClaim(claim =>
    claim.Type == "id" && claim.Value == "Kaylan"
    || context.User.IsInRole("SuperAdmin")))
    );
});

//                                                                                      rate limiter global


var myRateLimit = new MyRateLimitOptions();

builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myRateLimit);
builder.Services.AddMemoryCache();
builder.Services.AddRateLimiter(option =>
{
    option.AddFixedWindowLimiter(policyName: "FixedLimit", context =>
    {
        context.PermitLimit = myRateLimit.PermitLimit;
        context.Window = TimeSpan.FromSeconds(myRateLimit.Window);
        context.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        context.QueueLimit = myRateLimit.QueueLimit;
    });
    option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
}
);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>

        RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpcontext.User.Identity?.Name ??
                                                                 httpcontext.Response.Headers.Host.ToString(),
          factory: partition => new FixedWindowRateLimiterOptions
          {
              AutoReplenishment = myRateLimit.AutoReplenishment,
              PermitLimit = myRateLimit.PermitLimit,
              QueueLimit = myRateLimit.QueueLimit,
              Window = TimeSpan.FromSeconds(myRateLimit.Window)
          }));




});


string? mysqlconectio = builder.Configuration.GetConnectionString("Conexao");
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseMySql(mysqlconectio,
    ServerVersion.AutoDetect(mysqlconectio)));

/*builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig
{
    LogLever = LogLevel.Information
}));
*/
builder.Services.AddScoped<ApiLoggingFilters>();
builder.Services.AddAutoMapper(typeof(ProdutosDTOMappingProfile));

//aplica o DI
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepositoy<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Logging.AddConsole();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExeptionHandler();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();




app.MapControllers();

app.Run();
