namespace FoodShopAPI;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly Dictionary<Type, object> _repositories = [];

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async void SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return _repositories[typeof(TEntity)] as IRepository<TEntity> ?? throw new Exception("Repository not found");
        }

        var repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

}
