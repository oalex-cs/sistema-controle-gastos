using GerenciadorFinanceiro.Api.Dtos.Totais;
using GerenciadorFinanceiro.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFinanceiro.Api.Controllers;

/// <summary>
/// Expõe a consulta consolidada de receitas, despesas e saldos.
/// </summary>
[ApiController]
[Route("api/totais")]
public sealed class TotaisController(TotaisService totaisService)
    : ControllerBase
{
    /// <summary>
    /// Consulta os totais por pessoa e o total geral.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(ConsultaTotaisResponse),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<ConsultaTotaisResponse>> Consultar(
        CancellationToken cancellationToken)
    {
        var totais = await totaisService.ConsultarAsync(cancellationToken);
        return Ok(totais);
    }
}