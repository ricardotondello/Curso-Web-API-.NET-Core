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
using ApiCatalogo.Filters;
using ApiCatalogo.Repository;

// .AsNoTracking() - nao faz cache das consultas

namespace ApiCatalogo.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class ProdutosController : ControllerBase
  {
    private readonly IUnitOfWork _uof;

    public ProdutosController(IUnitOfWork contexto)
    {
      _uof = contexto;
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
    {
      return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Produto>> Get()
    {
      return _uof.ProdutoRepository.Get().ToList();
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
    // {
    //   return await _uof.Produtos.AsNoTracking().ToListAsync();
    // }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id, string param2)
    {
      //throw new Exception("Erro global");

      var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

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
      _uof.ProdutoRepository.Add(produto);
      _uof.Commit();
      return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Produto produto)
    {
      if (id != produto.ProdutoId)
      {
        return BadRequest();
      }
      _uof.ProdutoRepository.Update(produto);

      _uof.Commit();
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult<Produto> Delete(int id)
    {
      //var produto = _uof.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
      var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

      if (produto == null)
      {
        return NotFound();
      }
      _uof.ProdutoRepository.Delete(produto);
      _uof.Commit();
      return produto;
    }
  }
}