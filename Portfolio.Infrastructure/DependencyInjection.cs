using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "DefaultConnection connection string bulunamadı.");
            }

            services.AddDbContext<PortfolioDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
