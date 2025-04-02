using Application.Models;
using Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

public class UserRegisterExamples : IMultipleExamplesProvider<UserDto>
{
    public IEnumerable<SwaggerExample<UserDto>> GetExamples()
    {
        var medico = new UserDto("Joao", "Silva", "joao.silva@gmail.com", "Abc102030!", EProfile.Doctor, "654321-SP",
            Specialties.Oftalmologia);
        var paciente = new UserDto("Carlos", "Souza", "carlos.souza@email.com", "Abc102030!", EProfile.Patient, null,
            null, "39858265896", "11998876655");
        
        var admin = new UserDto("Aldo", "Braga", "aldo.braga@email.com", "Abc102030!", EProfile.Admin, null,
            null, null, null);

        
        yield return new SwaggerExample<UserDto>()
        {
            Name = "Médico",
            Description = "Exemplo de requisição para registro de novo médico no sistema.",
            Value = medico
        };

        yield return new SwaggerExample<UserDto>
        {
            Name = "Paciente",
            Description = "Exemplo de requisição para registro de novo paciente no sistema.",
            Value = paciente
        };
        
        yield return new SwaggerExample<UserDto>
        {
            Name = "Admin",
            Description = "Exemplo de requisição para registro de novo admin no sistema.",
            Value = admin
        };
    }
}