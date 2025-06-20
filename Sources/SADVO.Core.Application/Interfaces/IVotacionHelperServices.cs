using SADVO.Core.Application.Dtos.Ciudadanos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Interfaces
{
	public interface IVotacionHelperServices
	{
		Task<CiudadanosDTO?> GetCiudadanosById(int Id);
	}
}
