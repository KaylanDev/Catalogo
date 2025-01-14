using Catalogo.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using Catalogo.Services;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options 
    => options.JsonSerializerOptions.ReferenceHandler
    = ReferenceHandler.IgnoreCycles);
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


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
