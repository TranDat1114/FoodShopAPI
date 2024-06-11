using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace FoodShopAPI;
public class Repository<TEntity>(ApplicationDbContext _context) : IRepository<TEntity> where TEntity : class
{
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public async Task DeleteAsync(string guid)
    {
        var entity = await GetByGuidAsync(guid);
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity> GetByGuidAsync(string guid)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Set<TEntity>().FindAsync(guid);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToArrayAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).ToArrayAsync();
    }

    // public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
    // {
    //     return await _context.Set<TEntity>().Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
    // }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        int page,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy
    )
    {
        return await _context.Set<TEntity>().Where(predicate).OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
    }

    public IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    public async Task<IEnumerable<TEntity>> PaginateAsync(int page, int pageSize, Expression<Func<TEntity, object>> orderBy)
    {
        return await _context.Set<TEntity>().OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

}
