using DAM.Application.Contracts.MultiTenancy;
using DAM.Domain.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Infrastructure.MultiTenancy
{
    public class QueryStringTenantIdentification : ITenantIdentification
    {
        private readonly TenantMapping _tenants;
        public QueryStringTenantIdentification(IConfiguration configuration)
        {
            _tenants = configuration.GetTenantMapping();
        }
        public IEnumerable<string> GetAllTenants()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentTenant(HttpContext context)
        {
            var tenant = context.Request.Query["tenant"].ToString();
            if (string.IsNullOrEmpty(tenant) || !_tenants.Tenants.Values.Contains(tenant, StringComparer.InvariantCultureIgnoreCase))
            {
                return _tenants.Default;
            }

            if (_tenants.Tenants.TryGetValue(tenant, out var tenants))
            {
                return tenants;
            }
            return tenant;
        }
    }
}
