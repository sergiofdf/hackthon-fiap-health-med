using Application.Services.DoctorServices;
using Domain.Dto;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController: ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IAgendaService _agendaService;

    public DoctorController(IAgendaService agendaService, IDoctorService doctorService)
    {
        _agendaService = agendaService;
        _doctorService = doctorService;
    }
    
    /// <summary>
    /// Consulta médicos.
    /// </summary>
    /// <remarks>
    /// <p>A consulta permite um filtro por especilidade via queryParam.</p>
    /// </remarks>
    /// <returns>Retorna medicos.</returns>
    /// <response code="200">Médicos disponíveis</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region getMedicosConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    // [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetDoctors([FromQuery] Specialties? especilidade, 
        CancellationToken cancellationToken)
    {
        List<DoctorDto> res;
        if (especilidade != null)
        {
            if (!Enum.TryParse<Specialties>(especilidade.ToString(), true, out var specialtyEnum))
            {
                return BadRequest("Especialidade inválida.");
            }
            res = await _doctorService.GetBySpecialtyAsync((Specialties)specialtyEnum);
        }
        else
        {
            res = await _doctorService.GetAllAsync(); 
        }
        
        return Ok(res);
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