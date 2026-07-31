namespace GerenciadorFinanceiro.Api.Models;

/// <summary>
/// Representa uma receita ou despesa associada a uma pessoa.
/// </summary>
public sealed class Transacao
{
    public Guid Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public TipoTransacao Tipo { get; set; }

    public Guid PessoaId { get; set; }

    // Propriedade de navegação utilizada pelo Entity Framework Core.
    public Pessoa? Pessoa { get; set; }
}