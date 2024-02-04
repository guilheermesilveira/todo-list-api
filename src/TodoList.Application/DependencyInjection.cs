using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Configurations;

namespace TodoList.Application;

public static class DependencyInjection
{
    public static void AddApplicationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddAuthConfig(configuration);

        services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);

        services.AddCorsConfig();

        services.ResolveDependencies();
    }
}