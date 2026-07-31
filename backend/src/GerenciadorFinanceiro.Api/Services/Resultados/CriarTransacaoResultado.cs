using GerenciadorFinanceiro.Api.Dtos.Transacoes;

namespace GerenciadorFinanceiro.Api.Services.Resultados;

/// <summary>
/// Representa os resultados possíveis da criação de uma transação.
/// Evita textos ou números isolados para identificar o resultado.
/// </summary>
public enum CriarTransacaoStatus
{
    Sucesso = 1,
    PessoaNaoEncontrada = 2,
    ReceitaNaoPermitidaParaMenor = 3
}

/// <summary>
/// Retorna o status da operação e, em caso de sucesso, a transação criada.
/// </summary>
public sealed record CriarTransacaoResultado(
    CriarTransacaoStatus Status,
    TransacaoResponse? Transacao)
{
    /// <summary>
    /// Cria um resultado de sucesso contendo a transação cadastrada.
    /// </summary>
    public static CriarTransacaoResultado Criada(
        TransacaoResponse transacao)
    {
        return new CriarTransacaoResultado(
            CriarTransacaoStatus.Sucesso,
            transacao);
    }

    /// <summary>
    /// Cria um resultado indicando que a pessoa informada não existe.
    /// </summary>
    public static CriarTransacaoResultado PessoaNaoEncontrada()
    {
        return new CriarTransacaoResultado(
            CriarTransacaoStatus.PessoaNaoEncontrada,
            null);
    }

    /// <summary>
    /// Cria um resultado indicando que menores não podem cadastrar receitas.
    /// </summary>
    public static CriarTransacaoResultado ReceitaProibidaParaMenor()
    {
        return new CriarTransacaoResultado(
            CriarTransacaoStatus.ReceitaNaoPermitidaParaMenor,
            null);
    }
}