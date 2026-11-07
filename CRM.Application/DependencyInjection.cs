using CRM.Application.Interfaces;
using CRM.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(DependencyInjection).Assembly); });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}