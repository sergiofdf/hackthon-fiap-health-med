using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class DoctorsConfig : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        builder.HasIndex(d => d.Crm).IsUnique();
        builder.HasIndex(d => d.Email).IsUnique();
        builder.Property(d => d.Specialty)
            .HasConversion<int>();
        builder.HasIndex(d => d.Specialty);
    }
}