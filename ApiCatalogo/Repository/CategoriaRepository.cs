using System.Collections.Generic;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository
{
  public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
  {
    public CategoriaRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
      return Get().Include(x => x.Produtos);
    }
  }
}