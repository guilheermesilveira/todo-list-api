using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Configurations;

namespace ToDo.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration,
        WebApplicationBuilder builder)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.ResolveDependencies(builder);
        services.AddAuthConfiguration(configuration);
        services.AddCorsConfig();
        services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);
    }
}