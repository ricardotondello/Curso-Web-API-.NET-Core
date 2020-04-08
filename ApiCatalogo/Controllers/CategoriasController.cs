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
using ApiCatalogo.Services;
using Microsoft.Extensions.Configuration;
using ApiCatalogo.Repository;
using AutoMapper;
using ApiCatalogo.DTOs;

namespace ApiCatalogo.Controllers
{

  [Route("api/[Controller]")]
  [ApiController]
  public class CategoriasController : ControllerBase
  {
    private readonly IUnitOfWork _uof;
    private readonly IConfiguration _configuracao;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork contexto, IConfiguration config,
      ILogger<CategoriasController> logger, IMapper mapper)
    {
      _uof = contexto;
      _configuracao = config;
      _logger = logger;
      _mapper = mapper;
    }

    [HttpGet("autor")]
    public string GetAutor()
    {
      return $"Autor: {_configuracao["autor"]}, Conexao: {_configuracao["ConnectionStrings:DefaultConnection"]}";
    }

    [HttpGet("saudacao/{nome}")]
    public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico, string nome)
    {
      return meuservico.Saudacao(nome);
    }


    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
      var categoria = _uof.CategoriaRepository.Get().ToList();
      
      var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

      return categoriaDTO;
    }

    [HttpGet("{id}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
      _logger.LogInformation("## GetPorId");
      var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
      if (categoria == null)
      {
        return NotFound();
      }
      var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
      return categoriaDTO;
    }

    [HttpPost]
    public ActionResult Post([FromBody] CategoriaDTO categoriaDTO)
    {
      var categoria = _mapper.Map<Categoria>(categoriaDTO);
      _uof.CategoriaRepository.Add(categoria);
      _uof.Commit();
      return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDTO)
    {
      var categoria = _mapper.Map<Categoria>(categoriaDTO);

      if (id != categoria.CategoriaId)
      {
        return BadRequest();
      }
      _uof.CategoriaRepository.Update(categoria);
      _uof.Commit();
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
      var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
      if (categoria == null)
      {
        return NotFound();
      }
      _uof.CategoriaRepository.Delete(categoria);
      _uof.Commit();

      var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
      return categoriaDTO;
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
    {
      _logger.LogInformation("## GetCategoriasProdutos");

      var categoria = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
      var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

      return categoriaDTO;
    }
  }

}