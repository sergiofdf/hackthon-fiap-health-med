using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.AuthServices;

public class AuthService(IUserRepository userRepository, IConfiguration config, ILogger<AuthService> logger)
    : IAuthService
{
    public async Task<bool> RegisterUserAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        if (await userRepository.GetByEmailAsync(userDto.Email, cancellationToken) is not null)
        {
            logger.LogError("Failed to add user {user}. Email already registered.", userDto);
               
            Field field = new()
            {
                Name = "email",
                Value = userDto.Email,
                ExMessage = "Email ja existente."
            };
            List<Field> fields = [field];
                
            DataValidationException.Throw("400", "Erro no registro de usuario", "Email duplicado", fields);
        }

        var user = userDto.Profile switch
        {
            EProfile.Doctor => new Doctor()
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Profile = userDto.Profile,
                Crm = userDto.Crm!,
                Specialty = (Specialties)userDto.Specialty!
            },
            EProfile.Patient => new Patient()
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Profile = userDto.Profile,
                Cpf = userDto.Cpf!,
                PhoneNumber = userDto.PhoneNumber!
            },
            _ => new User(userDto.Profile)
            {
                Name = userDto.Name, LastName = userDto.LastName, Email = userDto.Email, Profile = userDto.Profile,
            }
        };

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, userDto.Password);
        
        user.PasswordHash = hashedPassword;
        
        var res = await userRepository.AddAsync(user, cancellationToken);
        
        if (!res)
            ServerException.Throw(ErrorList.Registration.General.Code, ErrorList.Registration.General.ExMessage);
        
        return res; 
    }

    public async Task<string?> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        User? user = null;
        if (IsCrm(loginDto.Login))
        {
            user = await userRepository.GetByCrmAsync(loginDto.Login, cancellationToken);
        } 
        else if (IsEmail(loginDto.Login))
        {
            user = await userRepository.GetByEmailAsync(loginDto.Login, cancellationToken);
        }
        else if (IsCpf(loginDto.Login))
        {
            user = await userRepository.GetByCpfAsync(loginDto.Login, cancellationToken);
        }
        
        if (user is null) return null;
        
        return new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) ==
               PasswordVerificationResult.Failed ? null : GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Profile.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(config.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
    
    private static bool IsEmail(string input)
    {
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(input);
    }

    private static bool IsCpf(string input)
    {
        var regex = new Regex(@"^\d{11}$"); // Apenas 11 números consecutivos
        return regex.IsMatch(input);
    }

    private static bool IsCrm(string input)
    {
        var regex = new Regex(@"^\d{4,6}-[A-Z]{2}$"); // 4 a 6 digitos seguidos por "-UF"
        return regex.IsMatch(input);
    }
}