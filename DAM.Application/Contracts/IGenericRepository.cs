using System.Linq.Expressions;

namespace DAM.Application.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        void Remove(T entity);
        Task RemoveByIdAsync(Guid Id);
        void RemoveRange(IEnumerable<T> entities);
    }
}
