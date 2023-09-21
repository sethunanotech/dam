using Microsoft.Extensions.Configuration;
namespace DAM.Domain.MultiTenancy
{
    public static class ConfigurationExtensions
    {
        public static TenantMapping GetTenantMapping(this IConfiguration configuration)
        {
            return (TenantMapping) configuration.GetSection("Tenants").Get(typeof(TenantMapping));
        }
    }
}
