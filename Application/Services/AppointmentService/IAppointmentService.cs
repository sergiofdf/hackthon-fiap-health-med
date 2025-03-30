using Application.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
    Task<List<Appointment>> GetPendingConfirmationAppointsAsync(string doctorId);
    Task<bool> AddAppointmentAsync(AppointmentDto appointmentDto);
    Task<bool> UpdateAppointmentConfirmationAsync(string appointmentId, bool confirmed, string applicantId);
}