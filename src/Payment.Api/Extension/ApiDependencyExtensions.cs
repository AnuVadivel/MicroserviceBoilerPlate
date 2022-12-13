using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Repository;
using Payment.Persistence.Provider;
using Payment.Persistence.Provider.Contract;
using Payment.Persistence.Repository;


namespace Payment.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ApiDependencyExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddApiDependency(configuration)
                .AddDomainDependency(configuration)
                .AddPersistenceDependency(configuration);
        }

        private static IServiceCollection AddApiDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMediatR(typeof(Startup))
                .AddHttpContextAccessor();
        }

        private static IServiceCollection AddDomainDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        private static IServiceCollection AddPersistenceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IBankRepository, BankRepository>()
                .AddScoped<IBankProvider, BankProvider>();
        }
    }
}