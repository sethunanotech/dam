using DAM.Application.Contracts;
using DAM.Application.Contracts.MultiTenancy;
using DAM.Domain.Configurations;
using DAM.Domain.Enums;
using DAM.Infrastructure.Cache;
using DAM.Infrastructure.MultiTenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAM.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void ConfigureInfrastructure(this IServiceCollection services,  IConfiguration configuration)
        {
            //InMemory Cache Service Registration
            services.Configure<InMemoryCacheConfiguration>(configuration.GetSection("InMemoryCacheConfiguration"));
            services.AddMemoryCache();
            services.AddTransient<ICacheService, InMemoryCacheService>();
            //InMemory Cache Service Registration Completed

            #region SQL Server Cache Service Registration
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = configuration.GetConnectionString("SQLCacheConnection");
            //    options.TableName = "RootCache";
            //});
            //SQL Server Cache Service Completed
            #endregion

            #region Redis Cache Service Implementation
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            //    {
            //        EndPoints = { "http://localhost:2456" },
            //        Password = "**********",
            //        Ssl = true
            //    };
            //    options.InstanceName = "MyRedisCache"; //Optional instance name if any
            //});
            #endregion

            #region Cache Registration with Dynamic Cache system
            //services.AddTransient<Func<CacheFramework, ICacheService>>(serviceProvider => key =>
            //{
            //    switch (key)
            //    {
            //        case CacheFramework.InMemory:
            //            return serviceProvider.GetService<InMemoryCacheService>();
            //        case CacheFramework.Redis:
            //            return serviceProvider.GetService<RedisCacheService>();
            //        case CacheFramework.SQLServer:
            //            return serviceProvider.GetService<SqlServerCacheService>();
            //        default:
            //            return serviceProvider.GetService<InMemoryCacheService>();
            //    }
            //});
            #endregion

            // Multi Tenant Application
            services.AddSingleton<ITenantIdentification, QueryStringTenantIdentification>();
        }
    }
}
