using Application.ViewModels.Authentication;
using Common.Config;

namespace WebApi.Services.Token;

public class GenerateJwtToken: IGenerateJwtToken
{
    private readonly IHealthMedConfiguration _configuration;
    // private readonly AppDbContext _context;
    // private readonly IValidateUser _validateUser;
    
    public Task<AuthenticationResponseModel?> GenerateToken(LoginRequestModel user)
    {
        throw new NotImplementedException();
    }
}