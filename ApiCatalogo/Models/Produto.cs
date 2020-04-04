using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models
{
  [Table("Produtos")]
  public class Produto
  {
    [Key]
    public int ProdutoId { get; set; }
  
    [Required(ErrorMessage = "O nome � obrigatorio")]
    [MaxLength(80)]
    [StringLength(20, ErrorMessage = "O tamanho deve ser entre {2} e {1}", MinimumLength=5)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(300)]
    public string Descricao { get; set; }

    [Required]
    public decimal Preco { get; set; }

    [Required]
    [MaxLength(300)]
    public string ImagemURL { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public Categoria Categoria { get; set; }

    public int CategoriaId { get; set; }
  }
}