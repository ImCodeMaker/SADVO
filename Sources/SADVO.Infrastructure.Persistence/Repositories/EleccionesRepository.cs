using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class EleccionesRepository : GenericRepository<Elecciones>, IEleccionesRepository
	{
		private readonly SADVODbContext _context;

		public EleccionesRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Elecciones?> GetEleccionActivaAsync()
		{
			return await _context.Elecciones
				.FirstOrDefaultAsync(e => e.Estado == true);
		}

		public async Task<List<Elecciones>> GetAllEleccionesOrderedAsync()
		{
			return await _context.Elecciones
				.OrderByDescending(e => e.Estado)
				.ThenByDescending(e => e.Año)
				.ToListAsync();
		}

		public async Task<bool> FinalizarEleccionAsync(int eleccionId)
		{
			var eleccion = await _context.Elecciones.FindAsync(eleccionId);
			if (eleccion == null) return false;

			eleccion.Estado = false;
			_context.Elecciones.Update(eleccion);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<Dictionary<int, int>> GetPartidosCountByEleccionAsync(int eleccionId)
		{
			var partidosCount = await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.GroupBy(v => v.PartidoPoliticoId)
				.Select(g => new { PartidoId = g.Key, Count = g.Count() })
				.ToDictionaryAsync(x => x.PartidoId, x => x.Count);
			return partidosCount;
		}

		public async Task<Dictionary<int, int>> GetPuestosCountByEleccionAsync(int eleccionId)
		{
			var puestosCount = await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.GroupBy(v => v.PuestoElectivoId)
				.Select(g => new { PuestoId = g.Key, Count = g.Count() })
				.ToDictionaryAsync(x => x.PuestoId, x => x.Count);
			return puestosCount;
		}

		public async Task<List<ResultadoEleccion>> GetResultadosEleccionAsync(int eleccionId)
		{
			var resultados = await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.Include(v => v.Candidato)
				.Include(v => v.PuestoElectivo)
				.Include(v => v.PartidoPolitico)
				.GroupBy(v => new { v.PuestoElectivoId, v.CandidatoId })
				.Select(g => new ResultadoEleccion
				{
					PuestoElectivoId = g.Key.PuestoElectivoId,
					PuestoElectivoNombre = g.First().PuestoElectivo.Nombre,
					CandidatoId = g.Key.CandidatoId,
					CandidatoNombre = g.First().Candidato.Nombre,
					PartidoPoliticoNombre = g.First().PartidoPolitico.Nombre,
					CantidadVotos = g.Count()
				})
				.ToListAsync();

			// Calcular porcentajes
			var votosPorPuesto = resultados.GroupBy(r => r.PuestoElectivoId)
				.ToDictionary(g => g.Key, g => g.Sum(r => r.CantidadVotos));

			foreach (var resultado in resultados)
			{
				var totalVotosPuesto = votosPorPuesto[resultado.PuestoElectivoId];
				resultado.Porcentaje = totalVotosPuesto > 0 ? (double)resultado.CantidadVotos / totalVotosPuesto * 100 : 0;
			}

			return resultados.OrderBy(r => r.PuestoElectivoNombre)
						   .ThenByDescending(r => r.CantidadVotos)
						   .ToList();
		}

		public async Task<List<int>> GetAniosDisponiblesAsync()
		{
			return await _context.Elecciones
				.Where(e => e.Estado == false)
				.Select(e => e.Año)
				.Distinct()
				.OrderByDescending(a => a)
				.ToListAsync();
		}

		public async Task<List<Elecciones>> GetEleccionesPorAnioAsync(int anio)
		{
			return await _context.Elecciones
				.Where(e => e.Año == anio && e.Estado == false)
				.OrderByDescending(e => e.Año)
				.ToListAsync();
		}

		public async Task<int> GetCantidadPartidosPorEleccionAsync(int eleccionId)
		{
			return await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.Select(v => v.PartidoPoliticoId)
				.Distinct()
				.CountAsync();
		}

		public async Task<int> GetCantidadCandidatosPorEleccionAsync(int eleccionId)
		{
			return await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.Select(v => v.CandidatoId)
				.Distinct()
				.CountAsync();
		}

		public async Task<int> GetTotalVotosPorEleccionAsync(int eleccionId)
		{
			return await _context.Votos
				.Where(v => v.EleccionId == eleccionId)
				.Select(v => v.CiudadanoId)
				.Distinct()
				.CountAsync();
		}

		public async Task<bool> ExisteEleccionEnAnioAsync(int anio)
		{
			return await _context.Elecciones
				.AnyAsync(e => e.Año == anio);
		}

		public async Task<bool> ExisteEleccionActivaAsync()
		{
			return await _context.Elecciones.AnyAsync(e => e.Estado == true);
		}
	}
}