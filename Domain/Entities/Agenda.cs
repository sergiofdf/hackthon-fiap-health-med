using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Domain.Entities;

public class Agenda : EntityBase
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Available { get; set; }
    public decimal HourlyPrice { get; set; }
    
    public string DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public Agenda()
    {
        Id = Guid.NewGuid().ToString();
    }
}