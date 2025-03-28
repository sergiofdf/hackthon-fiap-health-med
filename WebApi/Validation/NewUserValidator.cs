using Domain.Shared;
using FluentValidation;
using System.Text.RegularExpressions;
using Application.Models;

namespace WebApi.Validation;

public class NewUserValidator : AbstractValidator<UserDto>
{
    public NewUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Campo nome obrigatorio.")
            .Length(2, 50).WithMessage("Deve ter entre 2 e 50 caracteres.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Campo sobrenome obrigatorio.")
            .Length(2, 50).WithMessage("Deve ter entre 2 e 50 caracteres.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Campo email obrigatorio.")
            .EmailAddress().WithMessage("Formato de email inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Campo senha obrigatorio.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
            .Must(ContainDigit).WithMessage("A senha deve conter pelo menos um numero.")
            .Must(ContainSpecialCharacter).WithMessage("A senha deve conter pelo menos um caractere especial (@, #, $, etc).");
    }

    public void IsValid(UserDto user)
    {
        var res = Validate(user);

        if (!res.IsValid)
        {
            var fields = new List<Field>();
            foreach (var error in res.Errors)
            {
                Field field = new()
                {
                    ExMessage = error.ErrorMessage,
                    Name = error.PropertyName,
                    Value = error.AttemptedValue.ToString()
                };
                fields.Add(field);
            }

            DataValidationException.Throw("002", "Dados invalidos", "Dados de contato invalido", fields);
        }
    } 
    
    private bool ContainDigit(string password) =>
        password.Any(char.IsDigit);

    private bool ContainSpecialCharacter(string password) =>
        Regex.IsMatch(password, @"[\W_]"); // Qualquer caractere não alfanumérico
}

