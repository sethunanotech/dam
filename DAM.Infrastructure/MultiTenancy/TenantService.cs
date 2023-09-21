using DAM.Application.Contracts.MultiTenancy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Infrastructure.MultiTenancy
{
    public class TenantService : ITenantService
    {
        private readonly HttpContext _httpContext;
        private readonly ITenantIdentification _tenantIdentificationService;

        public TenantService(IHttpContextAccessor accessor, ITenantIdentification tenantIdentificationService)
        {
            _httpContext = accessor.HttpContext;
            _tenantIdentificationService = tenantIdentificationService;
        }
        public string GetCurrentTenant()
        {
            return _tenantIdentificationService.GetCurrentTenant(_httpContext);
        }
    }
}
