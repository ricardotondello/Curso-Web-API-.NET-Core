using System;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCatalogo.Controllers
{
  [Produces("applications/json")]
  [Route("api/[Controller]")]
  [ApiController]
  public class AutorizaController : ControllerBase
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _config;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
    IConfiguration config)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _config = config;
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
      return Ok(GerarToken(model));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
    {
      var result = await _signInManager
      .PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        return Ok(GerarToken(userInfo));
      }
      ModelState.AddModelError(string.Empty, "Login inválido...");
      return BadRequest(ModelState);
    }

    private UsuarioTokenDTO GerarToken(UsuarioDTO userInfo)
    {
      var claims = new[]
      {
          new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
          new Claim("qualquerchave", "qualquervalor"),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expireHours = double.Parse(_config["TokenConfiguration:ExpireHours"]);
      var expiration = DateTime.UtcNow.AddHours(expireHours);

      JwtSecurityToken token = new JwtSecurityToken(
        issuer: _config["TokenConfiguration:Issuer"],
        audience: _config["TokenConfiguration:Audience"],
        claims: claims,
        expires: expiration,
        signingCredentials: credentials
      );

      return new UsuarioTokenDTO()
      {
        Authenticated = true,
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = expiration,
        Message = "Token JWT OK"
      };
    }
  }
}