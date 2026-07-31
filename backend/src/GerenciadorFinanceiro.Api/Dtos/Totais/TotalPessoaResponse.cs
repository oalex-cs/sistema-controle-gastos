namespace GerenciadorFinanceiro.Api.Dtos.Totais;

/// <summary>
/// Contém receitas, despesas e saldo consolidados de uma pessoa.
/// </summary>
public sealed record TotalPessoaResponse(
    Guid PessoaId,
    string PessoaNome,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);