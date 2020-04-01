using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Curso_Web_API_.NET_Core.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TesteController : ControllerBase
  {
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      return new string[] { "teste", "teste 1" };
    }

    [HttpGet("pessoas")]
    public ActionResult<IEnumerable<Pessoa>> GetPessoas()
    {
      return new[] {
        new Pessoa { Nome = "Nome 1"},
        new Pessoa { Nome = "Nome 2"},
        new Pessoa { Nome = "Nome 3"}
      };
    }
  }

  public class Pessoa
  {
    public string Nome { get; set; }
  }

}