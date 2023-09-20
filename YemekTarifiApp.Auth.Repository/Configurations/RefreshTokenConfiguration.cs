using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YemekTarifiApp.Auth.Core.Models;

namespace YemekTarifiApp.Auth.Repository.Configurations;

public class RefreshTokenConfiguration:IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.HasKey(t => t.UserId);
        builder.Property(t => t.Token);
        
    }
}