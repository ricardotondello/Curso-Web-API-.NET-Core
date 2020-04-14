using ApiCatalogo.Context;
using ApiCatalogo.Models;

namespace ApiCatalogoxUnitTest
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {
        }

        public void Seed(AppDbContext context)
        {
            Categoria c1 = new Categoria {CategoriaId = 1, Nome = "Categoria 1", ImagemURL = "kkk"};
            Categoria c2 = new Categoria {CategoriaId = 2, Nome = "Categoria 2", ImagemURL = "kkk"};
            Categoria c3 = new Categoria {CategoriaId = 3, Nome = "Categoria 3", ImagemURL = "kkk"};
            Categoria c4 = new Categoria {CategoriaId = 4, Nome = "Categoria 4", ImagemURL = "kkk"};
            Categoria c5 = new Categoria {CategoriaId = 5, Nome = "Categoria 5", ImagemURL = "kkk"};
            Categoria c6 = new Categoria {CategoriaId = 6, Nome = "Categoria 6", ImagemURL = "kkk"};
            Categoria c7 = new Categoria {CategoriaId = 7, Nome = "Categoria 7", ImagemURL = "kkk"};
            Categoria c8 = new Categoria {CategoriaId = 8, Nome = "Categoria 8", ImagemURL = "kkk"};


            context.Categorias.Remove(c1);
            context.Categorias.Remove(c2);
            context.Categorias.Remove(c3);
            context.Categorias.Remove(c4);
            context.Categorias.Remove(c5);
            context.Categorias.Remove(c6);
            context.Categorias.Remove(c7);
            context.Categorias.Remove(c8);

            context.SaveChanges();

            context.Categorias.Add(c1);
            context.Categorias.Add(c2);
            context.Categorias.Add(c3);
            context.Categorias.Add(c4);
            context.Categorias.Add(c5);
            context.Categorias.Add(c6);
            context.Categorias.Add(c7);
            context.Categorias.Add(c8);

            context.SaveChanges();
        }

    }
}