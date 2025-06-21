using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class CandidatosRepository : GenericRepository<Candidatos>, ICandidatosRepository
	{
		private readonly SADVODbContext _context;

		public CandidatosRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Candidatos?> GetCandidatoConAsignacionesAsync(int candidatoId)
		{
			return await _context.Candidatos
				.Include(c => c.Asignaciones)
				.FirstOrDefaultAsync(c => c.Id == candidatoId);
		}

		public async Task<List<Candidatos>> GetCandidatosActivosByPartidoAsync(int partidoId)
		{
			return await _context.Candidatos
				.Where(c => c.PartidoPoliticoId == partidoId && c.Estado == true)
				.OrderBy(c => c.Nombre)
				.ThenBy(c => c.Apellido)
				.ToListAsync();
		}

		public async Task<List<Candidatos>> GetCandidatosByPartidoAsync(int partidoId)
		{
			return await _context.Candidatos
				.Where(c => c.PartidoPoliticoId == partidoId)
				.OrderBy(c => c.Nombre)
				.ThenBy(c => c.Apellido)
				.ToListAsync();
		}

	}
}
