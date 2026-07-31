using GerenciadorFinanceiro.Api.Dtos.Transacoes;
using GerenciadorFinanceiro.Api.Services;
using GerenciadorFinanceiro.Api.Services.Resultados;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFinanceiro.Api.Controllers;

/// <summary>
/// Expõe os endpoints HTTP para criação e consulta de transações.
/// </summary>
[ApiController]
[Route("api/transacoes")]
public sealed class TransacoesController(
    TransacaoService transacaoService) : ControllerBase
{
    /// <summary>
    /// Cadastra uma transação para uma pessoa existente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(TransacaoResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TransacaoResponse>> Criar(
        [FromBody] CriarTransacaoRequest request,
        CancellationToken cancellationToken)
    {
        // Encaminha a operação ao serviço, onde estão as regras de negócio.
        var resultado = await transacaoService.CriarAsync(
            request,
            cancellationToken);

        if (resultado.Status == CriarTransacaoStatus.Sucesso &&
            resultado.Transacao is not null)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                resultado.Transacao);
        }

        if (resultado.Status == CriarTransacaoStatus.PessoaNaoEncontrada)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Pessoa não encontrada.",
                Detail =
                    $"Não existe uma pessoa cadastrada com o identificador " +
                    $"'{request.PessoaId}'.",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        // A requisição é válida, mas viola uma regra de negócio.
        if (resultado.Status ==
            CriarTransacaoStatus.ReceitaNaoPermitidaParaMenor)
        {
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Receita não permitida.",
                Detail =
                    "Pessoas menores de 18 anos podem cadastrar apenas despesas.",
                Status = StatusCodes.Status422UnprocessableEntity,
                Instance = HttpContext.Request.Path
            });
        }

        // Resposta defensiva para um status não previsto pelo controller.
        return Problem(
            title: "Não foi possível cadastrar a transação.",
            statusCode: StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Lista todas as transações cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<TransacaoResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TransacaoResponse>>> Listar(
        CancellationToken cancellationToken)
    {
        var transacoes = await transacaoService.ListarAsync(
            cancellationToken);

        return Ok(transacoes);
    }
}