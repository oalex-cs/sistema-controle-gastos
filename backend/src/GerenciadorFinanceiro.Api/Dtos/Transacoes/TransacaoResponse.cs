using GerenciadorFinanceiro.Api.Models;

namespace GerenciadorFinanceiro.Api.Dtos.Transacoes;

/// <summary>
/// Representa uma transação retornada pela API com sua pessoa associada.
/// </summary>
public sealed record TransacaoResponse(
    Guid Id,
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    Guid PessoaId,
    string PessoaNome);