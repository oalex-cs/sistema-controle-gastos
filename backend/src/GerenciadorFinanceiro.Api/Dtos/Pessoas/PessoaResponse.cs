namespace GerenciadorFinanceiro.Api.Dtos.Pessoas;

/// <summary>
/// Representa os dados de uma pessoa retornados pela API.
/// </summary>
public sealed record PessoaResponse(
    Guid Id,
    string Nome,
    int Idade);