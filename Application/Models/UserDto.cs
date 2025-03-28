using Domain.Enums;

namespace Application.Models;

public record UserDto(
    string Name, 
    string LastName,
    string Email, 
    string Password, 
    EProfile Profile, 
    string? Crm = null, 
    string? Specialty = null,
    string? Cpf = null,
    string? PhoneNumber = null
    );
