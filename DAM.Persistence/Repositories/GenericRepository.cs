using DAM.Application.Contracts;
using DAM.Domain.Enums;
using DAM.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Hangfire;

namespace DAM.Persistence.Repositories
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        private readonly static CacheFramework _cacheFramework = CacheFramework.InMemory;
        private readonly string _cacheKey = $"{typeof(T)}";

        protected readonly ApplicationDbContext _dbContext;
        private readonly Func<CacheFramework, ICacheService> _cacheService;

        public GenericRepository(ApplicationDbContext dbContext, Func<CacheFramework, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (!_cacheService(_cacheFramework).TryGet(_cacheKey, out IReadOnlyList<T> cachedList)) { 
                cachedList  = await _dbContext.Set<T>()
                                            .AsNoTracking()
                                            .ToListAsync();

                _cacheService(_cacheFramework).Set(_cacheKey, cachedList);
            }
            return cachedList;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() 
                => RefreshCache());
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            var cachedList = await _dbContext.Set<T>()
                                .Where(expression)
                                .AsNoTracking()
                                .ToListAsync();
            return cachedList;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>()
                            .FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
            return entity;
        }

        public async void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }
        public async Task RemoveByIdAsync(Guid Id)
        {
            var _entity = await _dbContext.Set<T>().FindAsync(Id);
            if (_entity != null)
            {
                _dbContext.Set<T>().Remove(_entity);
                await _dbContext.SaveChangesAsync();
                BackgroundJob.Enqueue(() => RefreshCache());
            }
        }
        public async void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task RefreshCache()
        {
            _cacheService(_cacheFramework).Remove(_cacheKey);
            var cachedList = await _dbContext.Set<T>().ToListAsync();
            _cacheService(_cacheFramework).Set(_cacheKey, cachedList);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }
    }
}
