using Application.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public interface IAppointmentService
{
    Task<List<Appointment>> GetAppointmentsAsync(string doctorId, 
        AppointmentStatus appointmentStatus,
        CancellationToken cancellationToken = default);
    Task<Appointment> AddAppointmentAsync(AppointmentDto appointmentDto, CancellationToken cancellationToken = default);
    Task<AppointmentResponseDto> UpdateAppointmentConfirmationAsync(UpdateAppointmentDto appointmentDto, CancellationToken cancellationToken = default);
}