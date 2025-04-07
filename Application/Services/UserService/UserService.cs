using Application.Models;
using Domain.Interfaces;

namespace Application.Services.UserService;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllAsync(ct);
        var listUsers = new List<UserResponseDto>();
        users?.ForEach(u =>
        {
            var userResponseDto = new UserResponseDto(u.Id, u.Name, u.LastName, u.Email, u.Profile);
            listUsers.Add(userResponseDto);
        });
        
        return listUsers;
    }
}