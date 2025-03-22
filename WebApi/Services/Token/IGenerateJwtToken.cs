using Application.ViewModels.Authentication;

namespace WebApi.Services.Token;

public interface IGenerateJwtToken
{
    Task<AuthenticationResponseModel?> GenerateToken(LoginRequestModel user);
}