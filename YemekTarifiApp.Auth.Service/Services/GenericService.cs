using System.Linq.Expressions;
using YemekTarifiApp.Auth.Core.DTOs;
using YemekTarifiApp.Auth.Core.Repositories;
using YemekTarifiApp.Auth.Core.Services;

namespace YemekTarifiApp.Auth.Service.Services;

public class GenericService<T> : IGenericService<T> where T :class
{
    private readonly IGenericRepository<T?> _repository;
    private readonly IUnitOfWork _unitOfWork;



    public GenericService(IGenericRepository<T?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        return entities;
    }
    
    public async Task<bool> Update(T? entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        return true;

    }

    public async Task Remove(T entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
    }

 
}