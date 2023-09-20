using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Auth.Core.Models;

namespace YemekTarifiApp.Auth.Repository;

public class AppDbContext:IdentityDbContext<User,Roles,string>
{
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}