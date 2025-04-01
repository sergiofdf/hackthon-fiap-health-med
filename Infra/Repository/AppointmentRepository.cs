using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    protected AppDbContext _dbContext;

    public AppointmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(string appointmentId)
    {
        return await _dbContext.Appointments.FindAsync(appointmentId);
    }

    public async Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(string doctorId)
    {
        return await _dbContext.Appointments.Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Pending).ToListAsync();
    }

    public async Task<bool> AddAppointmentAsync(Appointment appointment)
    {
        await _dbContext.Appointments.AddAsync(appointment);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Appointment?> UpdateAppointmentConfirmationAsync(string appointmentId, AppointmentStatus status)
    {
        var appointmentData = await _dbContext.Appointments.FindAsync(appointmentId);
        if (appointmentData == null)
        {
            NotFoundException.Throw("404", "Consulta não encontrada.");
        }

        appointmentData!.Status = status;
        return await _dbContext.SaveChangesAsync() > 0 ? appointmentData : null;
    }
}