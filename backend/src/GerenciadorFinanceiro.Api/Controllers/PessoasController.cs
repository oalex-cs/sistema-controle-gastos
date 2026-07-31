using GerenciadorFinanceiro.Api.Dtos.Pessoas;
using GerenciadorFinanceiro.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFinanceiro.Api.Controllers;

/// <summary>
/// Expõe os endpoints HTTP para cadastro, consulta e exclusão de pessoas.
/// </summary>
[ApiController]
[Route("api/pessoas")]
public sealed class PessoasController(PessoaService pessoaService)
    : ControllerBase
{
    /// <summary>
    /// Cadastra uma pessoa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(PessoaResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PessoaResponse>> Criar(
        [FromBody] CriarPessoaRequest request,
        CancellationToken cancellationToken)
    {
        // O serviço concentra a criação e a persistência da pessoa.
        var pessoa = await pessoaService.CriarAsync(
            request,
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, pessoa);
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<PessoaResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PessoaResponse>>> Listar(
        CancellationToken cancellationToken)
    {
        var pessoas = await pessoaService.ListarAsync(cancellationToken);
        return Ok(pessoas);
    }

    /// <summary>
    /// Exclui uma pessoa e todas as suas transações.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(
        Guid id,
        CancellationToken cancellationToken)
    {
        var excluida = await pessoaService.ExcluirAsync(
            id,
            cancellationToken);

        if (excluida)
        {
            return NoContent();
        }

        return NotFound(new ProblemDetails
        {
            Title = "Pessoa não encontrada.",
            Detail = $"Não existe uma pessoa cadastrada com o identificador '{id}'.",
            Status = StatusCodes.Status404NotFound,
            Instance = HttpContext.Request.Path
        });
    }
}