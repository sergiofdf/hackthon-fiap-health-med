using Application.Models;
using Application.Services.AuthServices;
using Domain.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validation;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Registra um novo usuário.
    /// </summary>
    /// <remarks>
    /// <p>Somente usuarios com perfil de administrador podem usar este servico.</p>
    /// <p>O email deve ser unico. Se ja estiver em uso, retornar erro 400 com orientacao na mensagem.</p>
    /// <p>A role do usuario é obrigatória</p>
    /// </remarks>
    /// <param name="model">Dados do usuário para cadastro.</param>
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
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserDto userDto)
    {
        NewUserValidator validator = new();
        validator.IsValid(userDto);
        var res = await _authService.RegisterUserAsync(userDto);
        return CreatedAtAction(nameof(Register), new { email = userDto.Email });
    }
    
    /// <summary>
    /// Autentica um usuario e retorna um token JWT.
    /// <p>O campo login pode ser um email  para médico, usuário ou paciente.</p>
    /// <p>O campo login pode ser um CRM  para médico.</p>
    /// <p>O campo login pode ser um CPF para paciente.</p>
    /// </summary>
    /// <param name="model">Dados do usuario para login.</param>
    /// <returns>Retorna um token JWT.</returns>
    /// <response code="200">Autenticado com sucesso.</response>
    /// <response code="400">Credenciais invalidas.</response>
    #region loginConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto model)
    {
        var token = await _authService.LoginAsync(model);
        if (token is null) return Unauthorized(new { message = "Usuário ou senha inválidos" });

        return Ok(new { token });
    }
}