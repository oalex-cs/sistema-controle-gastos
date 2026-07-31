using GerenciadorFinanceiro.Api.Data;
using GerenciadorFinanceiro.Api.Dtos.Totais;
using GerenciadorFinanceiro.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Api.Services;

/// <summary>
/// Consolida receitas, despesas e saldos por pessoa e no total geral.
/// </summary>
/// <remarks>
/// A consulta parte das pessoas cadastradas, soma separadamente suas
/// receitas e despesas e calcula o saldo pela diferença entre os valores.
/// Pessoas sem transações permanecem no resultado com totais iguais a zero.
/// </remarks>
public sealed class TotaisService(AppDbContext dbContext)
{
    /// <summary>
    /// Calcula os totais financeiros individuais e gerais.
    /// </summary>
    public async Task<ConsultaTotaisResponse> ConsultarAsync(
        CancellationToken cancellationToken)
    {
        // Calcula no banco os totais necessários de cada pessoa.
        var dadosPorPessoa = await dbContext.Pessoas
            .AsNoTracking()
            .OrderBy(pessoa => pessoa.Nome)
            .Select(pessoa => new
            {
                PessoaId = pessoa.Id,
                PessoaNome = pessoa.Nome,

                // O valor nulo ocorre quando não existem receitas.
                TotalReceitas = pessoa.Transacoes
                    .Where(transacao =>
                        transacao.Tipo == TipoTransacao.Receita)
                    .Sum(transacao => (decimal?)transacao.Valor)
                    ?? 0m,

                // O valor nulo ocorre quando não existem despesas.
                TotalDespesas = pessoa.Transacoes
                    .Where(transacao =>
                        transacao.Tipo == TipoTransacao.Despesa)
                    .Sum(transacao => (decimal?)transacao.Valor)
                    ?? 0m
            })
            .ToListAsync(cancellationToken);

        // Calcula o saldo individual após obter os totais do banco.
        var totaisPorPessoa = dadosPorPessoa
            .Select(dados => new TotalPessoaResponse(
                dados.PessoaId,
                dados.PessoaNome,
                dados.TotalReceitas,
                dados.TotalDespesas,
                dados.TotalReceitas - dados.TotalDespesas))
            .ToList();

        // Soma os resultados individuais para gerar o total geral.
        var totalReceitas = totaisPorPessoa.Sum(
            pessoa => pessoa.TotalReceitas);

        var totalDespesas = totaisPorPessoa.Sum(
            pessoa => pessoa.TotalDespesas);

        var totalGeral = new TotalGeralResponse(
            totalReceitas,
            totalDespesas,
            totalReceitas - totalDespesas);

        return new ConsultaTotaisResponse(
            totaisPorPessoa,
            totalGeral);
    }
}