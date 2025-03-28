using Domain.Enums;

namespace Application.Models;

public record UserLoginDto(
    string Password, 
    string? Crm = null, 
    string? Email = null, 
    string? Cpf = null
    );
