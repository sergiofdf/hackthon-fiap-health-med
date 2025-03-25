using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class AdminsConfig : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Profile)
            .HasConversion<int>();
    }
    
}