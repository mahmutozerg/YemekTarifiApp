using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;

namespace YemekTarifiApp.Service.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity :Base
{
    private readonly IGenericRepository<TEntity?> _repository;
    private readonly IUnitOfWork _unitOfWork;


    public GenericService(IGenericRepository<TEntity?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CustomResponseNoDataDto> Remove(string id)
    {
        var entity = await _repository.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        if (entity is null )
        {
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.UserNotFound);
        }
        entity.UpdatedAt =DateTime.Now;
        entity.UpdatedBy = entity.Id;
        await _repository.RemoveAsync(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        return entities;
    }
    
    public async Task<bool> Update(TEntity? entity)
    {
        entity.UpdatedBy = entity.Id;
        entity.UpdatedAt = DateTime.Now;
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        return true;

    }
    
}