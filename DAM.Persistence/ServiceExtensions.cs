using AutoMapper;
using DAM.Application.Contracts;
using DAM.Persistence.Data;
using DAM.Persistence.Handlers;
using DAM.Persistence.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAM.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

            //Configure Hangfire (Background worker)
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
            //Register Services Here
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrganization, OrganizationRepository>();

            //Automapper Registration
            var autoMapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperProfile()));
            IMapper mapper = autoMapper.CreateMapper();
            services.AddSingleton(mapper);
            //Service registration ends here
        }

        public static void UsePersistenceCustomConfig(this IApplicationBuilder app)
        {
            //This is optional to monitor the Hangfire Jobs
            app.UseHangfireDashboard("/jobs");
        }
    }
}
