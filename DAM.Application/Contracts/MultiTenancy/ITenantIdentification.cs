using Microsoft.AspNetCore.Http;

namespace DAM.Application.Contracts.MultiTenancy
{
    public interface ITenantIdentification
    {
        IEnumerable<String> GetAllTenants();
        string GetCurrentTenant(HttpContext context);
    }
}
