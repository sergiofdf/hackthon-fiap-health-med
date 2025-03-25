using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Domain.Entities;

public class Agenda : EntityBase
{
    [Key]
    public string Crm { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Available { get; set; }
    
    public Doctor Doctor { get; set; }
}