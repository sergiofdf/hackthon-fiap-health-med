using Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Controllers;

[ApiController]
[Route("api/healthcheck")]
public class HealthCheckController(HealthCheckService healthCheckService) : ControllerBase
{
    /// <summary>
    /// Verifica status do sistema.
    /// </summary>
    /// <remarks>
    /// <p>Valida o status de requisitos do sistema.</p>
    /// </remarks>
    /// <returns>Retorna ok caso sistemas estejam respondendo.</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="500">Internal Server Error</response>
    #region getHealthCheckConfig
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    #endregion
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    public async Task<IActionResult> GetHealthCheck()
    {
       
        var report = await healthCheckService.CheckHealthAsync();
        
        return Ok(report);
    }
}