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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//remove o limitador de caracters retornado do json e adiciona um tratador de excecoes global com filtros
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Catalogo",
        Description = "testando descrição",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }

    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//adciona o tempo de vida do objeto
builder.Services.AddTransient<IMeuService, MeuSevico>();

//desabilita o fromservice
builder.Services.Configure<ApiBehaviorOptions>(options => 
options.DisableImplicitFromServicesParameters = true
);
/*
 * testando o configuration para pegar valores no arquivo JSON
var chave1 = builder.Configuration["chave1"];
var chave2 = builder.Configuration["secao:chave2"];
*/
string? mysqlconectio = builder.Configuration.GetConnectionString("Conexao");

builder.Services.AddDbContext<AppDbContext>(options
    => options.UseMySql(mysqlconectio,
    ServerVersion.AutoDetect(mysqlconectio)));
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig
{
    LogLever = LogLevel.Information
}));
builder.Services.AddScoped<ApiLoggingFilters>();
//aplica o repository
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepositoy<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExeptionHandler();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
