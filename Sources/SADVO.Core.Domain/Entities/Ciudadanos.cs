
using SADVO.Core.Domain.Common;

namespace SADVO.Core.Domain.Entities
{
	public class Ciudadanos : SharedEntity<int>
	{
		public required string NombreCompleto { get; set; }
		public required string Email { get; set; }
		public required string Documento_Identidad { get; set; }

		#region Relacion uno a muchos - Un Ciudadano pudo haber votado varios veces
		public ICollection<HistorialVotaciones> HistorialVotaciones { get; set; } = new List<HistorialVotaciones>();
		public ICollection<Votos> Votos { get; set; } = new List<Votos>();
		#endregion

	}
}
