using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Appointment : EntityBase
{
    [Key]
    public string AppointmentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? CancellingJustification { get; set; } = null;

    // Relação com Médico
    public string DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    // Relação com Paciente
    public string PatientId { get; set; }
    public Patient Patient { get; set; }

    public Appointment()
    {
        AppointmentId = Guid.NewGuid().ToString();
    }
}