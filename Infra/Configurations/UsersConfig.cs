using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.UseTptMappingStrategy();
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Profile)
            .HasConversion<int>();
    }
    
}