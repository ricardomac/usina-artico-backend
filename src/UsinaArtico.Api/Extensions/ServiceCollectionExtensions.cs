using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

namespace UsinaArtico.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            o.AddSecurityRequirement(document =>
            {
                var schemeRef = new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme);
                var requirement = new OpenApiSecurityRequirement
                {
                    [schemeRef] = []
                };
                return requirement;
            });
        });

        return services;
    }
}