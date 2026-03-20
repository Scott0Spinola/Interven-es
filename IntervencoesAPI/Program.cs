

using IntervencoesAPI.Data;
using IntervencoesAPI.Services;
using IntervencoesAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Serilog;
using System.Reflection;
using Microsoft.OpenApi;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt",
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddDbContext<IntervencoesAPIContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<EntidadeService>();
builder.Services.AddScoped<ProcessoProjectoService>();
builder.Services.AddScoped<IntervencaoService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.SchemeName,
        _ => { });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(ApiKeyAuthenticationHandler.SchemeName)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.AddSecurityDefinition(ApiKeyAuthenticationHandler.SchemeName, new OpenApiSecurityScheme
    {
        Description = $"API Key via header {ApiKeyAuthenticationHandler.HeaderName}",
        Type = SecuritySchemeType.ApiKey,
        Name = ApiKeyAuthenticationHandler.HeaderName,
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(hostDocument => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(
                ApiKeyAuthenticationHandler.SchemeName,
                hostDocument,
                externalResource: null),
            new List<string>()
        }
    });
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.EnablePersistAuthorization());
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();


app.Run();



