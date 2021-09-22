using System.Collections.Generic;

namespace API_Swagger.Model
{
  public class ValidaCampoViewModelOutput
  {
    public IEnumerable<string> Erros { get; private set; }

    public ValidaCampoViewModelOutput(IEnumerable<string> erros)
    {
      Erros = erros;
    }

  }
}
