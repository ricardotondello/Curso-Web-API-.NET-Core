using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models
{
  [Table("Categorias")]
  public class Categoria
  {
    public Categoria()
    {
      Produtos = new Collection<Produto>();
    }

    [Key]
    public int CategoriaId { get; set; }

    [Required]
    [MaxLength(80)]
    public string Nome { get; set; }

    [MaxLength(300)]
    public string ImagemURL { get; set; }

    public ICollection<Produto> Produtos { get; set; }
  }
}