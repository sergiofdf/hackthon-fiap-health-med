using Application.Models;
using Domain.Entities;

namespace Application.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(UserDto userDto);
    Task<string?> LoginAsync(UserLoginDto loginDto);
}