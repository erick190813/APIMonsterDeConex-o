using ApiMonsterDeConexao.Interfaces;
using ApiMonsterDeConexao.Middlewares;
using ApiMonsterDeConexao.Services;
using APIMonsterDeConexão.Middlewares;
using APIMonsterDeConexão.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços de controller e serialização JSON estruturada
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração Avançada do Engine do Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ApiMonsterDeConexao - Ingestion & Storage Core",
        Version = "v1",
        Description = "Microsserviço de processamento de volumes de dados globais da GeoData Insight. Captura feeds do App 1, realiza triagem e persiste em nuvem.",
        Contact = new OpenApiContact
        {
            Name = "Erick Silva Fernandes de Araujo",
            Email = "erick.fernandes@geodatainsight.com"
        }
    });

    // Configura o Swagger para ler os metadados gerados pelo arquivo .csproj
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Resolução de Injeção de Dependência (DI) - Escopo Scoped por requisição HTTP
builder.Services.AddScoped<IPokemonService, PokemonService>();

var app = builder.Build();

// Ativação do ambiente Swagger mapeado na rota padrão/raiz
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiMonsterDeConexao v1");
    c.RoutePrefix = string.Empty;
});

// Middleware de resiliência acionado imediatamente antes do roteamento HTTP
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
app.UseAuthorization();
app.MapControllers();

app.Run();