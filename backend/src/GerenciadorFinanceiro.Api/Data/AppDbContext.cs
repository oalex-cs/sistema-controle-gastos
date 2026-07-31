using GerenciadorFinanceiro.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Api.Data;

/// <summary>
/// Centraliza o mapeamento das entidades e as regras de integridade do banco.
/// </summary>
/// <remarks>
/// O contexto define tabelas, chaves, limites, relacionamentos e restrições.
/// Regras que dependem do estado da aplicação permanecem nos serviços.
/// </remarks>
public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();

    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarPessoa(modelBuilder);
        ConfigurarTransacao(modelBuilder);
    }

    private static void ConfigurarPessoa(ModelBuilder modelBuilder)
    {
        var pessoa = modelBuilder.Entity<Pessoa>();

        pessoa.ToTable("Pessoas");
        pessoa.HasKey(p => p.Id);

        pessoa.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        pessoa.Property(p => p.Idade)
            .IsRequired();

        // Mantém o banco alinhado ao intervalo aceito pelo contrato da API.
        pessoa.ToTable(tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Pessoas_Idade_Valida",
                "Idade BETWEEN 0 AND 130");
        });
    }

    private static void ConfigurarTransacao(ModelBuilder modelBuilder)
    {
        var transacao = modelBuilder.Entity<Transacao>();

        transacao.ToTable("Transacoes");
        transacao.HasKey(t => t.Id);

        transacao.Property(t => t.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        transacao.Property(t => t.Valor)
            .IsRequired();

        transacao.Property(t => t.Tipo)
            .IsRequired();

        // Ao excluir uma pessoa, suas transações são removidas pelo banco.
        transacao.HasOne(t => t.Pessoa)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        transacao.HasIndex(t => t.PessoaId);

        transacao.ToTable(tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Transacoes_Valor_Positivo",
                "Valor > 0");

            tableBuilder.HasCheckConstraint(
                "CK_Transacoes_Tipo_Valido",
                "Tipo IN (1, 2)");
        });
    }
}