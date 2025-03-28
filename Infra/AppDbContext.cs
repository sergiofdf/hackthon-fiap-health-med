using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}