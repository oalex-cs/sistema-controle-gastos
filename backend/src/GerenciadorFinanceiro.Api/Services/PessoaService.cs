using GerenciadorFinanceiro.Api.Data;
using GerenciadorFinanceiro.Api.Dtos.Pessoas;
using GerenciadorFinanceiro.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Api.Services;

/// <summary>
/// Centraliza as operações de cadastro, consulta e exclusão de pessoas.
/// </summary>
public sealed class PessoaService(AppDbContext dbContext)
{
    /// <summary>
    /// Cadastra uma pessoa após a validação estrutural do request.
    /// </summary>
    public async Task<PessoaResponse> CriarAsync(
        CriarPessoaRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // O controller com ApiController impede requests sem idade.
        var idade = request.Idade ?? throw new ArgumentException(
            "A idade deve ser informada.",
            nameof(request));

        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome.Trim(),
            Idade = idade
        };

        dbContext.Pessoas.Add(pessoa);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PessoaResponse(
            pessoa.Id,
            pessoa.Nome,
            pessoa.Idade);
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas em ordem alfabética.
    /// </summary>
    public async Task<IReadOnlyList<PessoaResponse>> ListarAsync(
        CancellationToken cancellationToken)
    {
        return await dbContext.Pessoas
            .AsNoTracking()
            .OrderBy(pessoa => pessoa.Nome)
            .Select(pessoa => new PessoaResponse(
                pessoa.Id,
                pessoa.Nome,
                pessoa.Idade))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Exclui uma pessoa e suas transações pelo comportamento em cascata.
    /// </summary>
    public async Task<bool> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var pessoa = await dbContext.Pessoas
            .SingleOrDefaultAsync(
                pessoa => pessoa.Id == id,
                cancellationToken);

        if (pessoa is null)
        {
            return false;
        }

        dbContext.Pessoas.Remove(pessoa);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}