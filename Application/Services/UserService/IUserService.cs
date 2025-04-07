using Application.Models;

namespace Application.Services.UserService;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken ct);
}