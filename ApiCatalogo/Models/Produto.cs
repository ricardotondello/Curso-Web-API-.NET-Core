using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiCatalogo.Validation;

namespace ApiCatalogo.Models
{
  [Table("Produtos")]
  public class Produto : IValidatableObject
  {
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatorio")]
    [MaxLength(80)]
    [StringLength(20, ErrorMessage = "O tamanho deve ser entre {2} e {1}", MinimumLength = 5)]
    [PrimeiraLetraMaiuscula]
    public string Nome { get; set; }

    [Required]
    [MaxLength(300)]
    public string Descricao { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage= "O Preco deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }

    [Required]
    [MaxLength(300)]
    public string ImagemURL { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public Categoria Categoria { get; set; }

    public int CategoriaId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (!string.IsNullOrEmpty(this.Nome))
      {
        var primeiraLetra = this.Nome[0].ToString();
        if (primeiraLetra != primeiraLetra.ToUpper())
        {
          yield return new ValidationResult("A Primeira letra do nome deve ser em maiuscula",
          new[] { nameof(this.Nome) });
        }
      }

      if (this.Estoque <= 0)
      {
        yield return new ValidationResult("Estoque deve ser maior que zero",
          new[] { nameof(this.Estoque) });
      }
    }
  }
}