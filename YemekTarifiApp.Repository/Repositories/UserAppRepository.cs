using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;

namespace YemekTarifiApp.Repository.Repositories;

public class UserAppRepository:GenericRepository<User>,IUserAppRepository
{   
    private readonly DbSet<User> _dbSet;


    public UserAppRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<User>();
    }
}