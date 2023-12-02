using Microsoft.Extensions.DependencyInjection;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Infra.Data.Repositories;

namespace ToDo.Infra.Data.Configuration;

public static class DependencyConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAssignmentListRepository, AssignmentListRepository>()
            .AddScoped<IAssignmentRepository, AssignmentRepository>();
    }
}