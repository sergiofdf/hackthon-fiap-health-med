using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configurations;

public class AgendasConfig : IEntityTypeConfiguration<Agenda>
{
    public void Configure(EntityTypeBuilder<Agenda> builder)
    {
        builder.ToTable("Agendas");

        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Available)
                .HasDefaultValue(true); // Por padrão, um horário é livre

        // Relacionamento: Um médico tem muitas agendas, uma agenda pertence a um médico
        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Agendas)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade); // Se o médico for deletado, suas agendas também são
    }
}