using Domain.Enums;

namespace Application.Models;

public class UpdateAppointmentDto
{
    public string AppointmentId { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Reason { get; set; }
}