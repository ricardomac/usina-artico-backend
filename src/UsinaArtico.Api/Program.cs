using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using UsinaArtico.Api;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Domain.Users;
using UsinaArtico.Application;
using UsinaArtico.Infrastructure;
using UsinaArtico.Infrastructure.Authentication;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.UseCors(x => x
    .WithOrigins("http://localhost:4200", "https://sistema.usinaartico.com.br/") 
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()); 

app.MapEndpoints();

    app.UseSwaggerWithUi();


if (app.Environment.IsDevelopment())
{

    app.ApplyMigrations();

    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
        await seeder.SeedAsync();
    }
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

// REMARK: If you want to use Controllers, you'll need this.
// app.MapControllers();




await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace UsinaArtico.Api
{
    public partial class Program;
}