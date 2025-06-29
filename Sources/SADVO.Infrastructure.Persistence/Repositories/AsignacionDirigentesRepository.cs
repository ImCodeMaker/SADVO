﻿using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class AsignacionDirigentesRepository : GenericRepository<AsignacionDirigentes>, IAsignacionDirigentesRepository
	{
		private readonly SADVODbContext _context;

		public AsignacionDirigentesRepository(SADVODbContext context) : base(context) 
		{ 
		_context = context;
		}

		public async Task<AsignacionDirigentes?> GetByUserIdWithPartidoAsync(int userId)
		{
			return await _context.AsignacionDirigentes
				.Include(ad => ad.PartidoPolitico)
				.FirstOrDefaultAsync(ad => ad.UsuarioId == userId);
		}

	}
}
