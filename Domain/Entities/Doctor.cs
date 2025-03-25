using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Doctor : User
{
    [Key]
    public string Crm { get; set; }
    public string Specialty { get; set; }
    
    public Agenda Agenda { get; set; }
    
    public List<Appointment> Appointments { get; set; }

    public Doctor()
    {
        Profile = EProfile.Doctor;
    }
}