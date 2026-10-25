using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using apicsharp.Data;
using apicsharp.Models;
using apicsharp.Repositories;
using apicsharp.Services;

// Carrega as variáveis do arquivo .env (se existir)
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Monta a connection string a partir do .env; se não houver, usa o appsettings
var connectionString = Environment.GetEnvironmentVariable("DB_HOST") is { } dbHost
    ? $"Host={dbHost};" +
      $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};" +
      $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
      $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}"
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Endereços do front-end que podem chamar a API (separados por vírgula no .env)
var origensPermitidas = (Environment.GetEnvironmentVariable("CORS_ORIGINS")
        ?? "http://localhost:3000,http://localhost:5173,http://localhost:4200")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(origensPermitidas)
        .AllowAnyHeader()
        .AllowAnyMethod()));


builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();


builder.Services.AddControllers()
    // Enums aparecem como texto no JSON ("Receita"/"Despesa") em vez de 0/1
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Antes do redirecionamento HTTPS, para o navegador não receber redirect no preflight (OPTIONS)
app.UseCors("Frontend");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
