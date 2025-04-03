using Application.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
    Task<List<Appointment>> GetPendingConfirmationAppointsAsync(string doctorId);
    Task<Appointment> AddAppointmentAsync(AppointmentDto appointmentDto);
    Task<AppointmentResponseDto> UpdateAppointmentConfirmationAsync(string appointmentId, AppointmentStatus status);
}