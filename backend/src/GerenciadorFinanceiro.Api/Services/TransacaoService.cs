using GerenciadorFinanceiro.Api.Data;
using GerenciadorFinanceiro.Api.Dtos.Transacoes;
using GerenciadorFinanceiro.Api.Models;
using GerenciadorFinanceiro.Api.Services.Resultados;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Api.Services;

/// <summary>
/// Centraliza as regras de negócio e operações de persistência das transações.
/// </summary>
public sealed class TransacaoService(AppDbContext dbContext)
{
    /// <summary>
    /// Cadastra uma transação após validar a pessoa e as regras aplicáveis.
    /// </summary>
    public async Task<CriarTransacaoResultado> CriarAsync(
        CriarTransacaoRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Projeta apenas os dados necessários para validar e montar a resposta.
        var pessoa = await dbContext.Pessoas
            .AsNoTracking()
            .Where(pessoa => pessoa.Id == request.PessoaId)
            .Select(pessoa => new
            {
                pessoa.Id,
                pessoa.Nome,
                pessoa.Idade
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (pessoa is null)
        {
            return CriarTransacaoResultado.PessoaNaoEncontrada();
        }

        var pessoaEhMenorDeIdade = pessoa.Idade < 18;
        var transacaoEhReceita = request.Tipo == TipoTransacao.Receita;

        // Regra de negócio: menores de idade podem cadastrar apenas despesas.
        if (pessoaEhMenorDeIdade && transacaoEhReceita)
        {
            return CriarTransacaoResultado.ReceitaProibidaParaMenor();
        }

        var transacao = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Tipo = request.Tipo,
            PessoaId = pessoa.Id
        };

        dbContext.Transacoes.Add(transacao);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Retorna um DTO para não expor a entidade de persistência.
        var response = new TransacaoResponse(
            transacao.Id,
            transacao.Descricao,
            transacao.Valor,
            transacao.Tipo,
            pessoa.Id,
            pessoa.Nome);

        return CriarTransacaoResultado.Criada(response);
    }

    /// <summary>
    /// Lista as transações cadastradas com os dados da pessoa associada.
    /// </summary>
    public async Task<IReadOnlyList<TransacaoResponse>> ListarAsync(
        CancellationToken cancellationToken)
    {
        // O join reúne transação e pessoa em uma única consulta de leitura.
        return await (
            from transacao in dbContext.Transacoes.AsNoTracking()
            join pessoa in dbContext.Pessoas.AsNoTracking()
                on transacao.PessoaId equals pessoa.Id
            orderby pessoa.Nome, transacao.Descricao
            select new TransacaoResponse(
                transacao.Id,
                transacao.Descricao,
                transacao.Valor,
                transacao.Tipo,
                pessoa.Id,
                pessoa.Nome)
        ).ToListAsync(cancellationToken);
    }
}