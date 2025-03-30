using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    protected AppDbContext _dbContext;

    public AppointmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<bool> UpdateAppointmentConfirmationAsync(Appointment appointment)
    {
        var appointmentData = await _dbContext.Appointments.FindAsync(appointment.AppointmentId);
        if (appointmentData == null) return false;
        
        appointmentData.Status = appointment.Status;
        return await _dbContext.SaveChangesAsync() > 0;
    }
}