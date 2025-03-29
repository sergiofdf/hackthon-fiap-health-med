using Application.Services.DoctorServices;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController: ControllerBase
{
    private readonly IAgendaService _agendaService;

    public DoctorController(IAgendaService agendaService)
    {
        _agendaService = agendaService;
    }
    
    /// <summary>
    /// Consulta agenda dísponível de um médico.
    /// </summary>
    /// <returns>Retorna agendas.</returns>
    /// <response code="200">Agendas disponíveis</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region getAgendaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpGet("{doctorId}/agenda")]
    public async Task<ActionResult> GetAgenda([FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime,
        [FromRoute] string doctorId,
        CancellationToken cancellationToken)
    {
        startDateTime = DateTime.SpecifyKind(startDateTime, DateTimeKind.Utc);
        endDateTime = DateTime.SpecifyKind(endDateTime, DateTimeKind.Utc);
        var res = await _agendaService.GetDoctorAvailableAgendaByTime(doctorId, startDateTime, endDateTime);
        return Ok(res);
    }
    
    /// <summary>
    /// Cadastra agenda para um médico.
    /// </summary>
    /// <returns>true</returns>
    /// <response code="201">Agenda criada com sucesso</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region getAgendaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpPost("{doctorId}/agenda")]
    public async Task<ActionResult> PostAgenda([FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime,
        [FromRoute] string doctorId,
        CancellationToken cancellationToken)
    {
        startDateTime = DateTime.SpecifyKind(startDateTime, DateTimeKind.Utc);
        endDateTime = DateTime.SpecifyKind(endDateTime, DateTimeKind.Utc);
        var res = await _agendaService.AddNewAvailableAgenda(doctorId, startDateTime, endDateTime);
        return Ok(res);
    }
}