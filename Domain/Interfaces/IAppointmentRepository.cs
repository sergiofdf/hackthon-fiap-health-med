using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetAppointmentByIdAsync(string appointmentId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(string doctorId, AppointmentStatus appointmentStatus, CancellationToken cancellationToken = default);
    Task<bool> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<Appointment?> UpdateAppointmentConfirmationAsync(string appointmentId, AppointmentStatus status, string? reason = null, CancellationToken cancellationToken = default);
}