using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;

namespace YemekTarifiApp.Repository.Repositories;

public class CommentRepository:GenericRepository<Comment>,ICommentRepository
{
    private readonly DbSet<Comment> _dbSet;

    public CommentRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Comment>();

    }
}