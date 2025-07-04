﻿using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface ICiudadanosRepository : IGenericRepository<Ciudadanos>
	{
		Task<Ciudadanos?> getCiudadanoByCedula(string cedula);
	}
}
