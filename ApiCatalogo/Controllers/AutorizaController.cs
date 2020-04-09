using System;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class AutorizaController : ControllerBase
  {
    public readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
      return "AutorizaController :: Acessado em: " + DateTime.Now.ToLongDateString();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody]UsuarioDTO model)
    {
      //   if (!ModelState.IsValid)
      //   {
      //     return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
      //   }
      var user = new IdentityUser
      {
        UserName = model.Email,
        Email = model.Email,
        EmailConfirmed = true
      };

      var result = await _userManager.CreateAsync(user, model.Password);
      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      await _signInManager.SignInAsync(user, false);
      return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
    {
      var result = await _signInManager
      .PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        return Ok();
      }
      ModelState.AddModelError(string.Empty, "Login inválido...");
      return BadRequest(ModelState);
    }
  }
}