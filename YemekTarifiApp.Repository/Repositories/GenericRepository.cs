using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;

namespace YemekTarifiApp.Repository.Repositories;

public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity :Base
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }
    
    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        var entities =  _dbSet.Where(expression);
        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public async Task AddAsync(TEntity? entity)
    {

         await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity? entity)
    {
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = entity.Id;
        _dbSet.Update(entity);
    }

    public Task RemoveAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = entity.Id;
        _dbSet.Update(entity);
        
        return Task.CompletedTask;
    }
}