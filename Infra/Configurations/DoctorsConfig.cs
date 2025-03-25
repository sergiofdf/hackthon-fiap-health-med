using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class DoctorsConfig : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        builder.HasKey(d => d.Crm);
        builder.Property(d => d.Profile)
            .HasConversion<int>();
        builder.HasOne(d => d.Agenda)
            .WithOne(a => a.Doctor)
            .HasForeignKey<Agenda>(a => a.Crm);
    }
    
}