using Domain.Enums;

namespace Application.Models;

public class AppointmentDto
{
    public DateTime StartTime { get; set; } 
    public DateTime EndTime { get; set; } 
    public string DoctorId { get; set; } 
    public string PatientId { get; set; }
}

public class AppointmentResponseDto : AppointmentDto
{
    public AppointmentStatus SaStatus { get; set; } 
}