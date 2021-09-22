using API_Swagger.Filtros;
using API_Swagger.Model;
using API_Swagger.Model.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Swagger.Controllers
{
  [Route("api/v1/usuario")]
  [ApiController]
  public class UsuarioController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public UsuarioController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    //private object usuarioViewModelOutput;
    /// <summary>
    /// Teste
    /// </summary>
    /// <param name="loginViewModelInput"></param>
    /// <returns>Ok, dados do Usuário e o token em caso de sucesso</returns>
    [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
    [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
    [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
    [HttpPost]
    [Route("logar")]
    [ValidacaoModelStateCustomizado]
    public IActionResult Logar(LoginViewModelInput loginViewModelInput)
    {
      //if (!ModelState.IsValid)
      //{
      //  return BadRequest(new ValidaCampoViewModelOutput(ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage)));
      //}
      var usuarioViewModelOutput = new UsuarioViewModelOutput()
      {
        Codigo = 1,
        Login = "matheusHenrique",
        Email = "matheusHenrique@email.com"
      };

      //var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
      //var symmetricSecurityKey = new SymmetricSecurityKey(secret);
      //var securityTokenDescriptor = new SecurityTokenDescriptor
      //{
      //  Subject = new ClaimsIdentity(new Claim[]
      //  {
      //    new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
      //    new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
      //    new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email.ToString())
      //  }),
      //  Expires = DateTime.UtcNow.AddDays(1),
      //  SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
      //};
      //var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
      //var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
      //var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
            new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
            new Claim(ClaimTypes.Role, usuarioViewModelOutput.Email.ToString())
          }),
        NotBefore = DateTime.UtcNow,
        Expires = DateTime.UtcNow.AddDays(5),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new
      {
        Token = tokenHandler.WriteToken(token),
        Usuario = usuarioViewModelOutput
      });

      //return Ok(loginViewModelInput);
    }

    [HttpPost]
    [Route("registrar")]
    [ValidacaoModelStateCustomizado]
    public IActionResult Registrar(RegistrarViewModelInput registrarViewModelInput)
    {
      return Created("", registrarViewModelInput);
    }
  }
}
