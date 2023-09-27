using System.Linq.Expressions;

namespace DAM.Application.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(bool isCacheEnabled = false, string cacheKey = "");
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity, bool isCacheEnabled = false, string cacheKey = "");
        Task AddRangeAsync(IEnumerable<T> entities, bool isCacheEnabled = false, string cacheKey = "");
        Task<T> UpdateAsync(T entity, bool isCacheEnabled = false, string cacheKey = "");
        Task Remove(T entity, bool isCacheEnabled = false, string cacheKey = "");
        Task RemoveByIdAsync(Guid Id, bool isCacheEnabled = false, string cacheKey = "");
        Task RemoveRange(IEnumerable<T> entities, bool isCacheEnabled = false, string cacheKey = "");
    }
}
