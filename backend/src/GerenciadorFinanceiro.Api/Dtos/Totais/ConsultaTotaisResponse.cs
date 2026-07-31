namespace GerenciadorFinanceiro.Api.Dtos.Totais;

/// <summary>
/// Agrupa os totais individuais e o consolidado geral da aplicação.
/// </summary>
public sealed record ConsultaTotaisResponse(
    IReadOnlyList<TotalPessoaResponse> Pessoas,
    TotalGeralResponse TotalGeral);