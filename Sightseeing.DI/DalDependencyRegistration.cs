using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.DAL.DbContext;
using SightSeeing.DAL.UnitOfWork;

namespace Sightseeing.DI
{
    public static class DalDependencyRegistration
    {
        public static IServiceCollection AddSightSeeingDal(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SightSeeingDbContext>(options =>
                options.UseSqlite(connectionString)
                    .UseLazyLoadingProxies());
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}