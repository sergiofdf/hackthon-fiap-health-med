using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Doctor : User
{
    public string Crm { get; set; }
    public Specialties Specialty { get; set; }
    public decimal HourlyPrice { get; set; }
    
    public List<Agenda> Agendas { get; set; }
    
    public List<Appointment> Appointments { get; set; }

    public Doctor() : base(EProfile.Doctor)
    {
    }
}