using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
  [ApiVersion("1.0", Deprecated = true)]
  [Route("api/v{v:apiVersion}/teste")]
  [ApiController]
  public class TesteV1Controller : ControllerBase
  {
    [HttpGet]
    public ActionResult Get()
    {
      return Ok("Versao 1.0");
    }
  }
}