using InnokuMailSender.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnokuMailSender.Core.Data;

public abstract class Repository<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : Entity
{
    protected TDbContext Db;
    protected DbSet<TEntity> DbSet;
    protected IQueryable<TEntity> BaseQuery => DbSet.AsNoTracking();

    protected Repository(TDbContext context)
    {
        Db = context;
        DbSet = Db.Set<TEntity>();
    }

    public virtual async Task InsertAsync(TEntity obj)
    {
        await DbSet.AddAsync(obj);
    }

    public virtual async Task InsertRangeAsync(ICollection<TEntity> obj)
    {
        await DbSet.AddRangeAsync(obj);
    }

    public virtual async Task UpdateAsync(TEntity obj)
    {
        DbSet.Update(obj);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> obj)
    {
        DbSet.UpdateRange(obj);
    }

    public virtual async Task<IEnumerable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includes = null)
    {
        var query = BaseQuery;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includes = null)
    {
        var query = BaseQuery;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await BaseQuery.AnyAsync(predicate);
    }

    public virtual async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await BaseQuery.AllAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await BaseQuery.CountAsync(predicate);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string> includes = null)
    {
        var query = BaseQuery;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync(IEnumerable<string> includes = null)
    {
        var query = BaseQuery;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }
    
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        DbSet.Remove(entity);
    }

    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await BaseQuery.Where(predicate)
            .ToListAsync();
        DbSet.RemoveRange(entities);
    }

    public virtual async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null)
    {
        return await BaseQuery.Where(predicate ?? (entity => true)).SumAsync(sumPredicate);
    }

    public virtual async Task<int> SumAsync(Expression<Func<TEntity, int>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null)
    {
        return await BaseQuery.Where(predicate ?? (entity => true)).SumAsync(sumPredicate);
    }

    public virtual async Task<long> SumAsync(Expression<Func<TEntity, long>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null)
    {
        return await BaseQuery.Where(predicate ?? (entity => true)).SumAsync(sumPredicate);
    }

    public virtual async Task<double> SumAsync(Expression<Func<TEntity, double>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null)
    {
        return await BaseQuery.Where(predicate ?? (entity => true)).SumAsync(sumPredicate);
    }

    public virtual async Task<float> SumAsync(Expression<Func<TEntity, float>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null)
    {
        return await BaseQuery.Where(predicate ?? (entity => true)).SumAsync(sumPredicate);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db.Dispose();
    }
}

public abstract class RepositoryIntId<TDbContext, TEntity> : Repository<TDbContext, TEntity>, IRepositoryIntId<TEntity>
    where TDbContext : DbContext
    where TEntity : EntityIntId
{
    protected RepositoryIntId(TDbContext context)
        : base(context)
    {
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id, IEnumerable<string> includes = null)
    {
        var query = BaseQuery;

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = DbSet.Find(id);
        DbSet.Remove(entity);
    }
}
