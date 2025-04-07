using Domain.Enums;

namespace Application.Models;

public record UserDto(
    string Name, 
    string LastName,
    string Email, 
    string Password, 
    EProfile Profile, 
    string? Crm = null, 
    Specialties? Specialty = null,
    string? Cpf = null,
    string? PhoneNumber = null
    );


public record UserResponseDto(
    string Id,
    string Name, 
    string LastName,
    string Email, 
    EProfile Profile
);
