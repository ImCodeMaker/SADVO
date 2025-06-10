namespace SADVO.Core.Domain.Common
{
	public class SharedEntity<T>
	{
		public required T Id { get; set; }
		public DateTime FechaCreacion { get; set; } = DateTime.Now;
		public DateTime FechaModificacion { get; set; }
		public required bool Estado { get; set; } = true;
	}
}
