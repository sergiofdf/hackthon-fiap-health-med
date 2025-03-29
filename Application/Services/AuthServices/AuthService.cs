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

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration config, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _config = config;
        _logger = logger;
    }
    
    public async Task<bool> RegisterUserAsync(UserDto userDto)
    {
        if (await _userRepository.GetByEmailAsync(userDto.Email!) is not null)
        {
            _logger.LogError("Failed to add user {user}. Email already registered.", userDto);
               
            Field field = new()
            {
                Name = "email",
                Value = userDto.Email,
                ExMessage = "Email ja existente."
            };
            List<Field> fields = new() { field };
                
            DataValidationException.Throw("400", "Erro no registro de usuario", "Email duplicado", fields);
        }

        User user = userDto.Profile switch
        {
            EProfile.Doctor => new Doctor()
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Profile = userDto.Profile,
                Crm = userDto.Crm!,
                Specialty = userDto.Specialty!
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
        
        var res = await _userRepository.AddAsync(user);
        
        if (!res)
            ServerException.Throw(ErrorList.Registration.General.Code, ErrorList.Registration.General.ExMessage);
        
        return res; 
    }

    public async Task<string?> LoginAsync(UserLoginDto loginDto)
    {
        User? user = null;
        if (IsCrm(loginDto.Login))
        {
            user = await _userRepository.GetByCrmAsync(loginDto.Login);
        } 
        else if (IsEmail(loginDto.Login))
        {
            user = await _userRepository.GetByEmailAsync(loginDto.Login);
        }
        else if (IsCpf(loginDto.Login))
        {
            user = await _userRepository.GetByCpfAsync(loginDto.Login);
        }
        
        if (user is null) return null;
        
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) ==
            PasswordVerificationResult.Failed)
        {
            return null;    
        }
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Profile.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
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
        var regex = new Regex(@"^\d{4,6}-[A-Z]{2}$"); // 4 a 6 dígitos seguidos por "-UF"
        return regex.IsMatch(input);
    }
}