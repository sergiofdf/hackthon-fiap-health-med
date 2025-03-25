using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Admin : User
{
    [Key]
    public string Id { get; set; }
    
    public Admin()
    {
        Profile = EProfile.Admin;
        Id = Guid.NewGuid().ToString();
    }
}