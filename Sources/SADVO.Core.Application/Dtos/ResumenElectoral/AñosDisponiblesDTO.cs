namespace SADVO.Core.Application.Dtos.ResumenElectoral
{
	public class AniosDisponiblesDTO
	{
		public List<int> Años { get; set; } = new List<int>();
		public int AñoMasReciente { get; set; }
	}
}
