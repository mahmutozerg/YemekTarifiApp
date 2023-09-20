using System.Linq.Expressions;
using YemekTarifiApp.Core.DTOs;

namespace YemekTarifiApp.Core.Services;

public interface IGenericService<TEntity> where TEntity:class
{

    Task<CustomResponseNoDataDto> Remove(string userId);
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> Update(TEntity? entity);
}