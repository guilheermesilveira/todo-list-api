using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;
using ToDo.Application.Contracts;
using ToDo.Application.Notifications;
using ToDo.Application.Services;
using ToDo.Domain.Models;

namespace ToDo.Application.Configurations;

public static class DependencyConfig
{
    public static void ResolveDependencies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services
            .AddSingleton(_ => builder.Configuration)
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services
            .AddScoped<INotificator, Notificator>()
            .AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();

        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAssignmentListService, AssignmentListService>()
            .AddScoped<IAssignmentService, AssignmentService>();
    }
}