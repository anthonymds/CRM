using CRM.Application.Interfaces;
using CRM.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace CRM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<
            ICustomerRepository,
            CustomerRepository>();

        return services;
    }
}
