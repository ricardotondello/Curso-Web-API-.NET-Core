using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
  [ApiVersion("2.0")]
  [Route("api/v{v:apiVersion}/teste")]
  [ApiController]
  public class TesteV2Controller : ControllerBase
  {
    [HttpGet]
    public ActionResult Get()
    {
      return Ok("Versao 2.0");
    }

  }
}