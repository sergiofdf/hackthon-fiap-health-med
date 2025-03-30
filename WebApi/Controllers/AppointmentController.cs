using Application.Models;
using Application.Services.AppointmentService;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController: ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Cadastra uma consulta.
    /// </summary>
    /// <returns>true</returns>
    /// <response code="201">Consulta criada com sucesso</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region postConsultaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpPost("consulta")]
    public async Task<ActionResult> PostAppointment( [FromBody] AppointmentDto request,
        CancellationToken cancellationToken)
    {
        var res = await _appointmentService.AddAppointmentAsync(request);
        return Ok(res);
    }
    
    /// <summary>
    /// Lista consultas pendentes de aprovação de um médico.
    /// </summary>
    /// <returns>true</returns>
    /// <response code="200">Consultas para aprovação.</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region getConsultaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpGet("consultas-pendentes/{doctorId}")]
    public async Task<ActionResult> GetPendingAppointment( [FromRoute] string doctorId,
        CancellationToken cancellationToken)
    {
        var res = await _appointmentService.GetPendingConfirmationAppointsAsync(doctorId);
        return Ok(res);
    }
    
}