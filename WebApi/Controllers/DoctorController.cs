using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Models;
using Application.Services.DoctorServices;
using Domain.Dto;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebApi.Controllers;

[ApiController]
[Route("api/medicos")]
public class DoctorController(IAgendaService agendaService, IDoctorService doctorService, IMemoryCache memoryCache) : ControllerBase
{
    /// <summary>
    /// Consulta de médicos com filtro por especialidade e paginacao.
    /// </summary>
    /// <remarks>
    /// <p>A consulta permite um filtro por especilidade via queryParam.</p>
    /// <p>Este endpoint possui paginação. Se não forem informados os parametros de página e tamanho da página, será considerado por defualt 1 e 10 respectivamente.</p>
    /// </remarks>
    /// <returns>Retorna medicos.</returns>
    /// <response code="200">Médicos disponíveis</response>
    /// <response code="401">Sem autorizacao</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    #region getMedicosConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    #endregion
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors([FromQuery] Specialties? especilidade,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue($"doctors-{especilidade}-{page}-{pageSize}", out List<DoctorDto> res)) return Ok(res);
        if (especilidade != null)
        {
            if (!Enum.TryParse<Specialties>(especilidade.ToString(), true, out var specialtyEnum))
            {
                return BadRequest("Especialidade inválida.");
            }
            res = await doctorService.GetBySpecialtyAsync(specialtyEnum, cancellationToken);
        }
        else
        {
            res = await doctorService.GetAllAsync(page, pageSize, cancellationToken); 
        }
        
        memoryCache.Set($"doctors-{especilidade}-{page}-{pageSize}", res, DateTimeOffset.Now.AddMinutes(5));

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
    [Authorize]
    [HttpGet("{doctorId}/agenda")]
    public async Task<ActionResult<IEnumerable<DoctorAgendaDto>>> GetAgenda([FromQuery] DateTime startDateTime, 
        [FromQuery] DateTime endDateTime,
        [FromRoute] string doctorId,
        CancellationToken cancellationToken = default)
    {
        startDateTime = DateTime.SpecifyKind(startDateTime, DateTimeKind.Utc);
        endDateTime = DateTime.SpecifyKind(endDateTime, DateTimeKind.Utc);
        var res = await agendaService.GetDoctorAvailableAgendaByTime(doctorId, startDateTime, endDateTime, cancellationToken);
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
    #region postAgendaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    [Authorize(Roles = "Doctor, Admin")]
    [HttpPost("{doctorId}/agenda")]
    public async Task<ActionResult> PostAgenda([FromBody] AgendaDto agendaDto,
        [FromRoute] string doctorId,
        CancellationToken cancellationToken = default)
    {
        await agendaService.AddNewAvailableAgenda(doctorId, agendaDto, cancellationToken);
        return Created();
    }
    
    /// <summary>
    /// Atualiza agenda de um médico.
    /// </summary>
    /// <returns>true</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="400">Erro na requisição.</response>
    /// <response code="401">Sem autorizacao.</response>
    /// <response code="403">Forbidden</response>
    #region putAgendaConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    #endregion
    [Authorize(Roles = "Doctor,Admin")]
    [HttpPut("agenda/{agendaId}")]
    public async Task<ActionResult> PutAgenda(
        [FromRoute] string agendaId,
        [FromBody] AgendaDto requestDto, 
        CancellationToken cancellationToken = default)
    {

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var res = await agendaService.UpdateAgenda(agendaId, requestDto, userRole, userId, cancellationToken);
        return Ok(res);
    }
}