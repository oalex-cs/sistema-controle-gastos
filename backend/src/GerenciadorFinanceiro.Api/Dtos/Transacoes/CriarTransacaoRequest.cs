using System.ComponentModel.DataAnnotations;
using GerenciadorFinanceiro.Api.Models;

namespace GerenciadorFinanceiro.Api.Dtos.Transacoes;

/// <summary>
/// Define os dados aceitos pela API para cadastrar uma transação.
/// </summary>
public sealed class CriarTransacaoRequest : IValidatableObject
{
    /// <summary>
    /// Descrição da transação, limitada ao tamanho aceito pelo banco.
    /// </summary>
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(
        200,
        ErrorMessage = "A descrição deve possuir no máximo 200 caracteres.")]
    [RegularExpression(
        @".*\S.*",
        ErrorMessage = "A descrição deve conter pelo menos um caractere válido.")]
    public string Descricao { get; init; } = string.Empty;

    /// <summary>
    /// Valor positivo da transação. O tipo define se é entrada ou saída.
    /// </summary>
    public decimal Valor { get; init; }

    /// <summary>
    /// Define se a transação é uma receita ou despesa.
    /// </summary>
    public TipoTransacao Tipo { get; init; }

    /// <summary>
    /// Identificador da pessoa associada à transação.
    /// </summary>
    public Guid PessoaId { get; init; }

    /// <summary>
    /// Valida valores padrão ou inválidos não cobertos pelos atributos.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        // O valor deve ser positivo; o tipo representa entrada ou saída.
        if (Valor <= 0)
        {
            yield return new ValidationResult(
                "O valor da transação deve ser maior que zero.",
                new[] { nameof(Valor) });
        }

        // Impede números que não estejam definidos no enum.
        if (!Enum.IsDefined(typeof(TipoTransacao), Tipo))
        {
            yield return new ValidationResult(
                "O tipo da transação é inválido.",
                new[] { nameof(Tipo) });
        }

        // Guid.Empty indica que nenhum identificador válido foi informado.
        if (PessoaId == Guid.Empty)
        {
            yield return new ValidationResult(
                "O identificador da pessoa é obrigatório.",
                new[] { nameof(PessoaId) });
        }
    }
}