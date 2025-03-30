using Domain.Entities;

namespace Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(string doctorId);
    Task<bool> AddAppointmentAsync(Appointment appointment);
    Task<bool> UpdateAppointmentConfirmationAsync(Appointment appointment);
}