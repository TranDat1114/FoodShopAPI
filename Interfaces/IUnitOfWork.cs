namespace FoodShopAPI;

public interface IUnitOfWork
{
    void SaveChanges();
    void SaveChangesAsync();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}

