using System.Linq.Expressions;

namespace FoodShopAPI;

public interface IRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    void Update(TEntity entity);

    Task<TEntity> GetByGuidAsync(string guid);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        int page,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy
    );
    IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> PaginateAsync(int page, int pageSize, Expression<Func<TEntity, object>> orderBy);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(string guid);

    Task<int> CountAsync();
}
