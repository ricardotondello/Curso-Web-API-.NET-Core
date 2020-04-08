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
using AutoMapper;
using ApiCatalogo.DTOs;

// .AsNoTracking() - nao faz cache das consultas

namespace ApiCatalogo.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class ProdutosController : ControllerBase
  {
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork contexto, IMapper mapper)
    {
      _uof = contexto;
      _mapper = mapper;
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
    {
      var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
      var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
      return produtosDTO;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
      var produtos = _uof.ProdutoRepository.Get().ToList();
      var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
      return produtosDTO;
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
    // {
    //   return await _uof.Produtos.AsNoTracking().ToListAsync();
    // }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id, string param2)
    {
      //throw new Exception("Erro global");

      var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

      if (produto == null)
      {
        return NotFound();
      }

      var produtosDTO = _mapper.Map<ProdutoDTO>(produto);
      return produtosDTO;
    }

    [HttpPost]
    public ActionResult Post([FromBody]ProdutoDTO produtoDTO)
    {
      //valida se o binding foi feito corretamente
      //agora essa validacao é feita automaticamente pelo [ApiController]
      // if (!ModelState.IsValid)
      // {
      //   return BadRequest(ModelState);
      // }
      var produto = _mapper.Map<Produto>(produtoDTO);
      _uof.ProdutoRepository.Add(produto);
      _uof.Commit();

      var retProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

      return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, retProdutoDTO);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
    {
      if (id != produtoDTO.ProdutoId)
      {
        return BadRequest();
      }

      var produto = _mapper.Map<Produto>(produtoDTO);

      _uof.ProdutoRepository.Update(produto);

      _uof.Commit();
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
      //var produto = _uof.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
      var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

      if (produto == null)
      {
        return NotFound();
      }
      _uof.ProdutoRepository.Delete(produto);
      _uof.Commit();

      var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
      return produtoDTO;
    }
  }
}