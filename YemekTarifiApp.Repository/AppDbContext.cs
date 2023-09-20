using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Repository;

public class AppDbContext:DbContext
{

    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Favorite> Favorite { get; set; }
    public DbSet<Comment> Comment{ get; set; }



    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {   
        //fill it
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        base.OnModelCreating(modelBuilder);
    }

    
}