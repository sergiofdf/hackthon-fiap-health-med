using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Patient : User
{
    [Key]
    public string Cpf { get; set; }
    public string PhoneNumber { get; set; }
    
    public List<Appointment> Appointments { get; set; }

    public Patient()
    {
        Profile = EProfile.Patient;
    }
}