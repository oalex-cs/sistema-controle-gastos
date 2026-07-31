namespace GerenciadorFinanceiro.Api.Dtos.Totais;

/// <summary>
/// Contém os valores consolidados de todas as pessoas.
/// </summary>
public sealed record TotalGeralResponse(
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);