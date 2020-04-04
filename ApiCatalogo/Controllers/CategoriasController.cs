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

namespace ApiCatalogo.Controllers
{

  [Route("api/[Controller]")]
  [ApiController]
  public class CategoriasController : ControllerBase
  {
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext contexto)
    {
      _context = contexto;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
      return _context.Categorias.AsNoTracking().ToList();
    }

    [HttpGet("{id}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
      var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);
      if (categoria == null)
      {
        return NotFound();
      }
      return categoria;
    }

    [HttpPost]
    public ActionResult Post([FromBody] Categoria categoria)
    {
      _context.Categorias.Add(categoria);
      _context.SaveChanges();
      return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Categoria categoria)
    {
      if (id != categoria.CategoriaId)
      {
        return BadRequest();
      }
      _context.Entry(categoria).State = EntityState.Modified;
      _context.SaveChanges();
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult<Categoria> Delete(int id)
    {
      var categoria = _context.Categorias.Find(id);
      if (categoria == null)
      {
        return NotFound();
      }
      _context.Categorias.Remove(categoria);
      _context.SaveChanges();
      return categoria;
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
      return _context.Categorias.Include(x => x.Produtos).AsNoTracking().ToList();
    }
  }

}