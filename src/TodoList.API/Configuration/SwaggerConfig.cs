using Microsoft.OpenApi.Models;

namespace TodoList.API.Configuration;

public static class SwaggerConfig
{
    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        var contact = new OpenApiContact
        {
            Name = "Guilherme Silveira",
            Email = "guilhermesilveirasousa@gmail.com",
            Url = new Uri("https://github.com/guilheermesilveira")
        };

        var license = new OpenApiLicense
        {
            Name = "Free License",
            Url = new Uri("https://github.com/guilheermesilveira")
        };

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "To-do List API",
                    Contact = contact,
                    License = license
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JSON Web Token based security",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
    }
}