using Domain.Enums;

namespace Domain.Entities;

public class Authentication: EntityBase
{
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Crm { get; set; }
    public string Password { get; set; }
    public bool EmailValidated { get; set; }
    public bool ChangePassword { get; set; }
    
    public EProfile Profile { get; set; }
}