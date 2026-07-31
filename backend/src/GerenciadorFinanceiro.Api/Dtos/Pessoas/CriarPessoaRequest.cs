using System.ComponentModel.DataAnnotations;

namespace GerenciadorFinanceiro.Api.Dtos.Pessoas;

/// <summary>
/// Define os dados aceitos pela API para cadastrar uma pessoa.
/// </summary>
/// <remarks>
/// As validações são estruturais. Regras que exigem consultas ou estado
/// da aplicação devem permanecer na camada de serviço.
/// </remarks>
public sealed class CriarPessoaRequest
{
    /// <summary>
    /// Nome da pessoa, limitado ao tamanho aceito pelo banco.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(
        100,
        ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    /// <summary>
    /// Idade obrigatória entre 0 e 130 anos.
    /// </summary>
    /// <remarks>
    /// O tipo anulável diferencia idade zero de um campo não enviado.
    /// </remarks>
    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(
        0,
        130,
        ErrorMessage = "A idade deve estar entre 0 e 130 anos.")]
    public int? Idade { get; init; }
}