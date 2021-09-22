using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using API_Swagger.Model;
using System.Linq;

namespace API_Swagger.Filtros
{
  public class ValidacaoModelStateCustomizado : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        var validaCampoViewModel = new ValidaCampoViewModelOutput(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage));
        context.Result = new BadRequestObjectResult(validaCampoViewModel);
      }

    
    }
  }
}
