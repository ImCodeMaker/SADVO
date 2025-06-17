using SADVO.Core.Application.ViewModels.PartidosPoliticos;
using SADVO.Core.Application.ViewModels.Usuarios;
using System.ComponentModel.DataAnnotations;

public class CreateAsignacionDirigentesViewModel
{
	[Required(ErrorMessage = "Este campo es requerido.")]
	public int UsuarioId { get; set; }
	[Required(ErrorMessage = "Este campo es requerido.")]
	public int PartidoPoliticoId { get; set; }
	[Required(ErrorMessage = "Este campo es requerido.")]
	public bool Estado { get; set; } = true;
	public string UsuarioName { get; set; } = "";

	public List<UsuarioViewModel> UsuariosActivos { get; set; } = new();
	public List<PartidosPoliticosViewModel> PartidosActivos { get; set; } = new();
}
