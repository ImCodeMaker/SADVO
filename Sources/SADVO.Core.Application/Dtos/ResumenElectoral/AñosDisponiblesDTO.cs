using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Dtos.ResumenElectoral
{
	public class AniosDisponiblesDTO
	{
		public List<int> Años { get; set; } = new List<int>();
		public int AñoMasReciente { get; set; }
	}
}
