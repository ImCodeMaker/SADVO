namespace SADVO.Core.Application.ViewModels.AsignacionCandidatos
{
	public class ConfirmarEliminarAsignacionViewModel
	{
		public int Id { get; set; }
		public string NombreCandidato { get; set; } = string.Empty;
		public string ApellidoCandidato { get; set; } = string.Empty;
		public string NombreCompleto => $"{NombreCandidato} {ApellidoCandidato}";
		public string NombrePuestoElectivo { get; set; } = string.Empty;
		public string Mensaje => $"¿Está seguro que desea desvincular a {NombreCompleto} del puesto de {NombrePuestoElectivo}?";
	}
}
