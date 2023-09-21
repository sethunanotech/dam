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
    public class HostTenantIdentification : ITenantIdentification
    {
        private readonly TenantMapping _tenants;

        public HostTenantIdentification(IConfiguration configuration)
        {
            _tenants = configuration.GetTenantMapping();
        }

        public HostTenantIdentification(TenantMapping tenants)
        {
            _tenants = tenants;
        }

        public IEnumerable<string> GetAllTenants()
        {
            return _tenants.Tenants.Values;
        }

        public string GetCurrentTenant(HttpContext context)
        {
            if (!_tenants.Tenants.TryGetValue(context.Request.Host.Host, out var tenant))
            {
                tenant = _tenants.Default;
            }
            return tenant;
        }
    }
}
