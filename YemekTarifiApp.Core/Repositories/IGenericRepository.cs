using System.Linq.Expressions;

namespace YemekTarifiApp.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity:class?
{
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression);
    Task AddAsync(TEntity? entity);
    void Update(TEntity? entity);
    Task RemoveAsync(TEntity entity);

}