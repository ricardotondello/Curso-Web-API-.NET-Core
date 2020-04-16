using ApiCatalogo.Models;
using GraphQL.Types;

namespace ApiCatalogo.GraphQL
{
  public class CategoriaType : ObjectGraphType<Categoria>
  {
    public CategoriaType()
    {
      Field(x => x.CategoriaId);
      Field(x => x.Nome);
      Field(x => x.ImagemURL);

      Field<ListGraphType<CategoriaType>>("categorias");
    }
  }
}
