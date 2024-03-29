﻿using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Application.Configurations;

public static class CorsConfig
{
    public static void AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}