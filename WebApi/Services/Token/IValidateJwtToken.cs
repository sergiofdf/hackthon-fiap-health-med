namespace WebApi.Services.Token;

public interface IValidateJwtToken
{
    bool ValidateToken(HttpRequest request, params string[] validRoles);
}