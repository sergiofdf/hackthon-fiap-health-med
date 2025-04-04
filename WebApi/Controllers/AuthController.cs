using Application.Models;
using Application.Services.AuthServices;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Validation;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registra um novo usuário.
    /// </summary>
    /// <remarks>
    /// <p>Somente usuarios com perfil de administrador podem usar este servico.</p>
    /// <p>O email deve ser unico. Se ja estiver em uso, retornar erro 400 com orientacao na mensagem.</p>
    /// <p>A role do usuario é obrigatória</p>
    /// </remarks>
    /// <param name="userDto"></param>
    /// <param name="ct"></param>
    /// <returns>Retorna 201 se criado com sucesso.</returns>
    /// <response code="201">Usuário registrado com sucesso.</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>

    #region registerconfig
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [SwaggerRequestExample(typeof(UserDto), typeof(UserRegisterExamples))]
    #endregion
    [Authorize(Roles = "Admin")]
    [HttpPost("registro")]
    public async Task<ActionResult> Register([FromBody] UserDto userDto, CancellationToken ct = default)
    {
        NewUserValidator validator = new();
        validator.IsValid(userDto);
        await authService.RegisterUserAsync(userDto, ct);
        return CreatedAtAction(nameof(Register), new { email = userDto.Email });
    }

    /// <summary>
    /// Autentica um usuario e retorna um token JWT.
    /// </summary>
    /// /// <remarks>
    /// <p>O campo login pode ser um email  para médico, usuário ou paciente.</p>
    /// <p>O campo login pode ser um CRM  para médico.</p>
    /// <p>O campo login pode ser um CPF para paciente.</p>
    /// </remarks>
    /// <param name="model">Dados do usuario para login.</param>
    /// <param name="ct"></param>
    /// <returns>Retorna um token JWT.</returns>
    /// <response code="200">Autenticado com sucesso.</response>
    /// <response code="400">Credenciais invalidas.</response>

    #region loginConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [SwaggerRequestExample(typeof(UserLoginDto), typeof(UserLoginExample))]
    #endregion
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] UserLoginDto model, CancellationToken ct = default)
    {
        var token = await authService.LoginAsync(model, ct);
        if (token is null) return Unauthorized(new { message = "Usuário ou senha inválidos" });

        return Ok(new TokenDto(token));
    }
}