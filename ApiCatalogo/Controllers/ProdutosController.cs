using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiCatalogo.Models;
using ApiCatalogo.Controllers;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

// .AsNoTracking() - nao faz cache das consultas

namespace ApiCatalogo.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class ProdutosController : ControllerBase
  {
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext contexto)
    {
      _context = contexto;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
      return _context.Produtos.AsNoTracking().ToList();
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
      var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

      if (produto == null)
      {
        return NotFound();
      }
      return produto;
    }

    [HttpPost]
    public ActionResult Post([FromBody]Produto produto)
    {
      //valida se o binding foi feito corretamente
      //agora essa validacao é feita automaticamente pelo [ApiController]
      // if (!ModelState.IsValid)
      // {
      //   return BadRequest(ModelState);
      // }
      _context.Produtos.Add(produto);
      _context.SaveChanges();
      return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Produto produto)
    {
      if (id != produto.ProdutoId)
      {
        return BadRequest();
      }
      _context.Entry(produto).State = EntityState.Modified;

      _context.SaveChanges();
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult<Produto> Delete(int id)
    {
      //var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
      var produto = _context.Produtos.Find(id);

      if (produto == null)
      {
        return NotFound();
      }
      _context.Produtos.Remove(produto);
      _context.SaveChanges();
      return produto;
    }
  }
}