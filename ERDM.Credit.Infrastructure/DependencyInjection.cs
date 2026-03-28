using ERDM.Credit.Domain.Interfaces;
using ERDM.Credit.Infrastructure.Repositories;
using ERDMCore.Infrastructure.MongoDB.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERDM.Credit.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add ERDMCore MongoDB infrastructure
            services.AddMongoDB(configuration);

            // Register repositories
            services.AddScoped<ICreditApplicationRepository, CreditApplicationRepository>();

            return services;
        }
    }
}
