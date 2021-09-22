using System.ComponentModel.DataAnnotations;

namespace API_Swagger.Model.Usuarios
{
  public class RegistrarViewModelInput
  {
    [Required(ErrorMessage = "O Login é Obrigatório!")]
    public string Login { get; set; }

    [Required(ErrorMessage = "A Senha é Obrigatória!")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O Email é Obrigatório!")]
    public string Email { get; set; }
  }
}
