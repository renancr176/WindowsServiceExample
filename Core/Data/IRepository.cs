﻿using InnokuMailSender.Core.DomainObjects;
using System.Linq.Expressions;

namespace InnokuMailSender.Core.Data;

public interface IRepository<TEntity> : IDisposable
    where TEntity : Entity
{
    Task InsertAsync(TEntity obj);
    Task InsertRangeAsync(ICollection<TEntity> obj);
    Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string> includes = null);
    Task<IEnumerable<TEntity>?> GetAllAsync(IEnumerable<string> includes = null);
    Task UpdateAsync(TEntity obj);
    Task UpdateRangeAsync(IEnumerable<TEntity> obj);
    Task DeleteAsync(Guid id);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includes = null);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includes = null);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> sumPredicate,
        Expression<Func<TEntity, bool>> predicate = null);
    Task<int> SumAsync(Expression<Func<TEntity, int>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null);
    Task<long> SumAsync(Expression<Func<TEntity, long>> sumPredicate, Expression<Func<TEntity, bool>> predicate = null);
    Task<double> SumAsync(Expression<Func<TEntity, double>> sumPredicate,
        Expression<Func<TEntity, bool>> predicate = null);
    Task<float> SumAsync(Expression<Func<TEntity, float>> sumPredicate,
        Expression<Func<TEntity, bool>> predicate = null);
    Task<int> SaveChangesAsync();
}

public interface IRepositoryIntId<TEntity> : IRepository<TEntity>
    where TEntity : EntityIntId
{
    Task<TEntity?> GetByIdAsync(int id, IEnumerable<string> includes = null);
    Task DeleteAsync(int id);
}
