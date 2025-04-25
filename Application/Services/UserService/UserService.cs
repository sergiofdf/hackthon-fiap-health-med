using Application.Models;
using Domain.Interfaces;

namespace Application.Services.UserService;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<UserResponseDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken ct)
    {
        var users = await userRepository.GetAllAsync(page, pageSize, ct);
        var listUsers = new List<UserResponseDto>();
        users?.ForEach(u =>
        {
            var userResponseDto = new UserResponseDto(u.Id, u.Name, u.LastName, u.Email, u.Profile);
            listUsers.Add(userResponseDto);
        });
        
        return listUsers;
    }
}