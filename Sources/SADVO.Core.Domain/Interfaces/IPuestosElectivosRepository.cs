﻿using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Domain.Interfaces
{
	public interface IPuestosElectivosRepository : IGenericRepository<PuestosElectivos>
	{
		Task<List<PuestosElectivos>> GetPuestosElectivosActivesAsync();
	}
}
