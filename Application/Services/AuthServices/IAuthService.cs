using Application.Models;
using Domain.Entities;

namespace Application.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(UserDto userDto, CancellationToken cancellationToken = default);
    Task<string?> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
}