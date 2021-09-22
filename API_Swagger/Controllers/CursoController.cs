using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using API_Swagger.Model.Cursos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
namespace API_Swagger.Controllers
{
  [Route("api/v1/curso")]
  [ApiController]
  [Authorize]
  public class CursoController : ControllerBase
  {
    /// <summary>
    /// Este serviço permite cadastrar curso para usuário Autenticado.
    /// </summary>
    /// <param name="cursoViewModelInput"></param>
    /// <returns>Retorna status 201 e dados do curso do usuário</returns>
    [SwaggerResponse(statusCode: 201, description: "Sucesso ao Cadastrar um Curso.", Type = typeof(CursoViewModelOutput))]
    [SwaggerResponse(statusCode: 401, description: "Não Autorizado.")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
    {
      var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
      return Created("", cursoViewModelInput);
    }


    /// <summary>
    /// Este Serviço permite obter todos os cursos ativos do usuário.
    /// </summary>
    /// <returns>Retorna status ok e dados do curso do usuário</returns>
    [SwaggerResponse(statusCode: 200, description: "Sucesso ao Obter os Cursos.", Type = typeof(CursoViewModelOutput))]
    [SwaggerResponse(statusCode: 401, description: "Não Autorizado.")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
      var cursos = new List<CursoViewModelOutput>();

      //var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
      cursos.Add(new CursoViewModelOutput()
      {
        Login = "",
        Descricao = "teste",
        Nome = "teste"
      });

      return Ok(cursos);
    }
  }
}
