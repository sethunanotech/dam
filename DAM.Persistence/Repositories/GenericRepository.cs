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
        protected readonly ApplicationDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public GenericRepository(ApplicationDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<T>> GetAllAsync(bool isCacheEnabled = false, string cacheKey = "")
        {
            if (isCacheEnabled)
            {
                if (_cacheService.TryGet(cacheKey, out IReadOnlyList<T> cachedList))
                {
                    return cachedList;
                }
                else
                {
                    var listOfItems = await _dbContext.Set<T>()
                                                    .AsNoTracking()
                                                    .ToListAsync();
                    _cacheService.Set(cacheKey, listOfItems);
                    return listOfItems;
                }
            }
            else
            {
                var listOfItems = await _dbContext.Set<T>()
                                .AsNoTracking()
                                .ToListAsync();
                return listOfItems;
            }
        }

        public async Task AddAsync(T entity, bool isCacheEnabled = false, string cacheKey = "")
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            if (isCacheEnabled)
                BackgroundJob.Enqueue(() 
                    => RefreshCache(cacheKey));
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, bool isCacheEnabled = false, string cacheKey = "")
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            if (isCacheEnabled)
                BackgroundJob.Enqueue(()
                    => RefreshCache(cacheKey));
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>()
                                .Where(expression)
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>()
                            .FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity, bool isCacheEnabled = false, string cacheKey = "")
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            if (isCacheEnabled)
                BackgroundJob.Enqueue(()
                    => RefreshCache(cacheKey));
            return entity;
        }

        public async Task Remove(T entity, bool isCacheEnabled = false, string cacheKey = "")
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            if (isCacheEnabled)
                BackgroundJob.Enqueue(()
                    => RefreshCache(cacheKey));
        }
        public async Task RemoveByIdAsync(Guid Id, bool isCacheEnabled = false, string cacheKey = "")
        {
            var _entity = await _dbContext.Set<T>().FindAsync(Id);
            if (_entity != null)
            {
                _dbContext.Set<T>().Remove(_entity);
                await _dbContext.SaveChangesAsync();
                if (isCacheEnabled)
                    BackgroundJob.Enqueue(()
                        => RefreshCache(cacheKey));
            }
        }
        public async Task RemoveRange(IEnumerable<T> entities, bool isCacheEnabled = false, string cacheKey = "")
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
            if (isCacheEnabled)
                BackgroundJob.Enqueue(()
                    => RefreshCache(cacheKey));
        }

        public async Task RefreshCache(string _cacheKey)
        {
            _cacheService.Remove(_cacheKey);
            var cachedList = await _dbContext.Set<T>().ToListAsync();
            _cacheService.Set(_cacheKey, cachedList);
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
