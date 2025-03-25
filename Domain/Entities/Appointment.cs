namespace Domain.Entities;

public class Appointment : EntityBase
{
    public string AppointmentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Confirmed { get; set; }

    // Relação com Médico
    public string Crm { get; set; }
    public Doctor Doctor { get; set; }

    // Relação com Paciente
    public string Cpf { get; set; }
    public Patient Patient { get; set; }
}