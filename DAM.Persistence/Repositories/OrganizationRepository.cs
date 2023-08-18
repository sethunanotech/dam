using DAM.Application.Contracts;
using DAM.Domain.Entities;
using DAM.Domain.Enums;
using DAM.Persistence.Data;

namespace DAM.Persistence.Repositories
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganization
    {
        public OrganizationRepository(ApplicationDbContext dbContext, Func<CacheFramework, ICacheService> cacheService) 
            : base(dbContext, cacheService)
        {
        }
    }
}
