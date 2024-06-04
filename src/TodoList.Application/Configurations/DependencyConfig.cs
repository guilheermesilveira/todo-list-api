using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;
using TodoList.Application.Contracts.Services;
using TodoList.Application.Notifications;
using TodoList.Application.Services;
using TodoList.Domain.Models;

namespace TodoList.Application.Configurations;

public static class DependencyConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services
            .AddScoped<INotificator, Notificator>()
            .AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();

        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IAssignmentListService, AssignmentListService>()
            .AddScoped<IAssignmentService, AssignmentService>();

        services
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}