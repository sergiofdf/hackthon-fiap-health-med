using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetAppointmentByIdAsync(string appointmentId);
    Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(string doctorId);
    Task<bool> AddAppointmentAsync(Appointment appointment);
    Task<Appointment?> UpdateAppointmentConfirmationAsync(string appointmentId, AppointmentStatus status);
}