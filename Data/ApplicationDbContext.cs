using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodShopAPI;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : IdentityDbContext<User>(options)
{
    protected readonly IConfiguration _configuration = configuration;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Coupon> Coupons { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
       {
           entity.Property(e => e.Avatar).HasDefaultValue("https://fastly.picsum.photos/id/228/600/600.jpg?hmac=TDkN4LVBjPRvjQqMs-TT63NvrvlB-FhcHIilfj8U4xg");
       });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ImageUrl).HasDefaultValue("https://fastly.picsum.photos/id/228/600/600.jpg?hmac=TDkN4LVBjPRvjQqMs-TT63NvrvlB-FhcHIilfj8U4xg");
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Order>(entity => { entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)"); });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MinimumAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethodInfo = typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));
        // 
        var Now = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {

                    if (string.IsNullOrEmpty(entity.Guid))
                    {
                        entity.Guid = Guid.NewGuid().ToString();
                    }
                    entity.CreatedAt = Now;
                    entity.CreatedBy = currentUser;
                    entity.IsDeleted = false;
                }

                entity.UpdatedAt = Now;
                entity.UpdatedBy = currentUser;

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                }
            }
        }
    }
}
