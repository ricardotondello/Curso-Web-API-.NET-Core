using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCatalogo.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CATEGORIAS(NOME, IMAGEMURL) " +
                                 "VALUES ('BEBIDAS', 'QUALQUER CAMINHO')");

            migrationBuilder.Sql("INSERT INTO CATEGORIAS(NOME, IMAGEMURL) " +
                                 "VALUES ('LANCHES', 'QUALQUER CAMINHO')");

            migrationBuilder.Sql("INSERT INTO CATEGORIAS(NOME, IMAGEMURL) " +
                                 "VALUES ('COMIDAS', 'QUALQUER CAMINHO')");

            migrationBuilder.Sql("INSERT INTO PRODUTOS(NOME, DESCRICAO, PRECO, IMAGEMURL, ESTOQUE, DATACADASTRO, CATEGORIAID) " +
                                 "VALUES ('APEROL', 'BEBIDA ALCOLICA', 12.00, 'IMAGEMURL', 100, date('now'), (SELECT CATEGORIAID FROM CATEGORIAS WHERE NOME = 'BEBIDAS'))");

            migrationBuilder.Sql("INSERT INTO PRODUTOS(NOME, DESCRICAO, PRECO, IMAGEMURL, ESTOQUE, DATACADASTRO, CATEGORIAID) " +
                                 "VALUES ('PASTEL', 'PASTEL DE CARNE', 2.5, 'IMAGEMURL', 10, date('now'), (SELECT CATEGORIAID FROM CATEGORIAS WHERE NOME = 'LANCHES'))");

            migrationBuilder.Sql("INSERT INTO PRODUTOS(NOME, DESCRICAO, PRECO, IMAGEMURL, ESTOQUE, DATACADASTRO, CATEGORIAID) " +
                                 "VALUES ('PIZZA', 'PIZZA MARGUERITA', 25.00, 'IMAGEMURL', 1, date('now'), (SELECT CATEGORIAID FROM CATEGORIAS WHERE NOME = 'COMIDAS'))");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM PRODUTOS");
            migrationBuilder.Sql("DELETE FROM CATEGORIAS");
        }
    }
}
