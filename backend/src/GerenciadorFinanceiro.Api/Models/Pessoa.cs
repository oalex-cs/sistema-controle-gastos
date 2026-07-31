namespace GerenciadorFinanceiro.Api.Models;

/// <summary>
/// Representa uma pessoa responsável por transações financeiras.
/// </summary>
public sealed class Pessoa
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int Idade { get; set; }

    // Uma pessoa pode possuir várias transações.
    public ICollection<Transacao> Transacoes { get; } = [];
}