using Domain.Enums;

namespace Application.Models;

public record UserLoginDto(
    string Login, 
    string Password 
    );
