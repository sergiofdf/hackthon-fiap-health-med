using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class AppointmentRepository(AppDbContext dbContext) : IAppointmentRepository
{
    public async Task<Appointment?> GetAppointmentByIdAsync(string appointmentId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Appointments.FindAsync([appointmentId, cancellationToken], cancellationToken: cancellationToken);
    }

    public async Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(string doctorId, AppointmentStatus appointmentStatus, CancellationToken cancellationToken = default)
    {
        return await dbContext.Appointments.Where(a => a.DoctorId == doctorId && a.Status == appointmentStatus).ToListAsync(cancellationToken);
    }

    public async Task<bool> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await dbContext.Appointments.AddAsync(appointment, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Appointment?> UpdateAppointmentConfirmationAsync(string appointmentId, AppointmentStatus status, string? reason = null, CancellationToken cancellationToken = default)
    {
        var appointmentData = await dbContext.Appointments.FindAsync([appointmentId, cancellationToken], cancellationToken: cancellationToken);
        if (appointmentData == null)
        {
            NotFoundException.Throw("404", "Consulta não encontrada.");
        }

        appointmentData!.Status = status;
        if(reason != null) appointmentData!.CancellingJustification = reason;
        return await dbContext.SaveChangesAsync(cancellationToken) > 0 ? appointmentData : null;
    }
}