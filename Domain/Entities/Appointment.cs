using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Appointment : EntityBase
{
    [Key]
    public string AppointmentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Confirmed { get; set; }

    // Relação com Médico
    public string DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    // Relação com Paciente
    public string PatientId { get; set; }
    public Patient Patient { get; set; }
}