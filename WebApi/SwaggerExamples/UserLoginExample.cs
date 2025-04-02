using Application.Models;
using Swashbuckle.AspNetCore.Filters;

public class UserLoginExample : IExamplesProvider<UserLoginDto>
{
    public UserLoginDto GetExamples()
    {
        return new UserLoginDto("sergio.dias@email.com", "Abc102030!");
    }
}
