using System.Linq.Expressions;

namespace YemekTarifiApp.Auth.Core.Services;

public interface IGenericService<TEntity> where TEntity:class
{


    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> Update(TEntity? entity);

    Task Remove(TEntity entity);
}