using Application.Models;

namespace Application.Services.UserService;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken ct);
}