using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class AsignacionCandidatosRepository : GenericRepository<AsignacionCandidatos>, IAsignacionCandidatosRepository
	{
		private readonly SADVODbContext _context;

		public AsignacionCandidatosRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

		// Método privado para liberar asignaciones de puestos inactivos
		private async Task LiberarAsignacionesDePuestosInactivosAsync()
		{
			var asignacionesAfectadas = await _context.AsignacionCandidatos
				.Where(a => a.Estado == true && a.puestosElectivos.Estado == false)
				.ToListAsync();

			if (asignacionesAfectadas.Any())
			{
				foreach (var asignacion in asignacionesAfectadas)
				{
					asignacion.Estado = false;
				}
				await _context.SaveChangesAsync();
			}
		}

		public async Task<List<AsignacionCandidatos>> GetAsignacionesByPartidoAsync(int partidoId)
		{
			// Primero liberar asignaciones de puestos inactivos
			await LiberarAsignacionesDePuestosInactivosAsync();

			return await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.Where(a => a.PartidoPoliticoId == partidoId && a.Estado == true)
				.OrderBy(a => a.Candidato.Nombre)
				.ThenBy(a => a.Candidato.Apellido)
				.ToListAsync();
		}

		public async Task<List<AsignacionCandidatos>> GetAsignacionesActivasAsync(int partidoId)
		{
			// Primero liberar asignaciones de puestos inactivos
			await LiberarAsignacionesDePuestosInactivosAsync();

			return await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.Where(a => (a.PartidoPoliticoId == partidoId || a.PartidoRespaldaId == partidoId) &&
						   a.Estado == true &&
						   a.Candidato.Estado == true &&
						   a.puestosElectivos.Estado == true)
				.OrderBy(a => a.Candidato.Nombre)
				.ThenBy(a => a.Candidato.Apellido)
				.ToListAsync();
		}

		public async Task<bool> ExisteCandidatoAsignadoEnPartidoAsync(int candidatoId, int partidoId)
		{
			return await _context.AsignacionCandidatos
				.AnyAsync(a => a.CandidatoId == candidatoId &&
							  a.PartidoPoliticoId == partidoId &&
							  a.Estado == true &&
							  a.PartidoRespaldaId == null); // Solo asignaciones originales
		}

		public async Task<bool> ExistePuestoAsignadoEnPartidoAsync(int puestoElectivoId, int partidoId)
		{
			return await _context.AsignacionCandidatos
				.AnyAsync(a => a.PuestoElectivoId == puestoElectivoId &&
							  a.PartidoPoliticoId == partidoId &&
							  a.Estado == true &&
							  a.PartidoRespaldaId == null); // Solo asignaciones originales
		}

		public async Task<AsignacionCandidatos?> GetAsignacionCandidatoEnOtroPartidoAsync(int candidatoId, int partidoIdActual)
		{
			return await _context.AsignacionCandidatos
				.Include(a => a.puestosElectivos)
				.FirstOrDefaultAsync(a => a.CandidatoId == candidatoId &&
										 a.PartidoPoliticoId != partidoIdActual &&
										 a.Estado == true &&
										 a.PartidoRespaldaId == null); // Solo asignaciones originales
		}

		public async Task<AsignacionCandidatos?> GetByIdWithIncludesAsync(int id)
		{
			return await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.FirstOrDefaultAsync(a => a.Id == id && a.Estado == true);
		}

		public async Task<List<AsignacionCandidatos>> GetCandidatosAsignadosDePartidoAsync(int partidoId)
		{
			// Primero liberar asignaciones de puestos inactivos
			await LiberarAsignacionesDePuestosInactivosAsync();

			return await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Where(a => a.PartidoPoliticoId == partidoId &&
						   a.Estado == true &&
						   a.PartidoRespaldaId == null)
				.ToListAsync();
		}

		public async Task<AsignacionCandidatos?> GetAsignacionOriginalAsync(int candidatoId, int puestoId)
		{
			return await _context.AsignacionCandidatos
				.Where(x => x.CandidatoId == candidatoId &&
							x.PuestoElectivoId == puestoId &&
							x.Estado)
				.OrderBy(x => x.PartidoRespaldaId != null)
				.FirstOrDefaultAsync();
		}

		public async Task<bool> ExisteRespaldoAsync(int candidatoId, int puestoElectivoId, int partidoRespaldaId)
		{
			return await _context.AsignacionCandidatos
				.AnyAsync(a => a.CandidatoId == candidatoId &&
							  a.PuestoElectivoId == puestoElectivoId &&
							  a.PartidoRespaldaId == partidoRespaldaId &&
							  a.Estado == true);
		}

		public async Task<bool> EliminarRespaldosAsync(int candidatoId, int puestoElectivoId)
		{
			var respaldos = await _context.AsignacionCandidatos
				.Where(a => a.CandidatoId == candidatoId &&
						   a.PuestoElectivoId == puestoElectivoId &&
						   a.PartidoRespaldaId != null &&
						   a.Estado == true)
				.ToListAsync();

			foreach (var respaldo in respaldos)
			{
				respaldo.Estado = false;
			}

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<List<AsignacionCandidatos>> GetRespaldosAsync(int candidatoId, int puestoElectivoId)
		{
			return await _context.AsignacionCandidatos
				.Include(a => a.PartidosPoliticos)
				.Where(a => a.CandidatoId == candidatoId &&
						   a.PuestoElectivoId == puestoElectivoId &&
						   a.PartidoRespaldaId != null &&
						   a.Estado == true)
				.ToListAsync();
		}

		public async Task<List<int>> GetCandidatosNoDisponiblesParaPartidoAsync(int partidoId)
		{
			await LiberarAsignacionesDePuestosInactivosAsync();

			var candidatosAsignados = await _context.AsignacionCandidatos
				.Where(a => a.PartidoPoliticoId == partidoId &&
						   a.Estado == true &&
						   a.PartidoRespaldaId == null)
				.Select(a => a.CandidatoId)
				.ToListAsync();

			return candidatosAsignados;
		}

		public async Task<List<int>> GetPuestosNoDisponiblesParaPartidoAsync(int partidoId)
		{
			await LiberarAsignacionesDePuestosInactivosAsync();

			var puestosOcupados = await _context.AsignacionCandidatos
				.Where(a => a.PartidoPoliticoId == partidoId &&
						   a.Estado == true &&
						   a.PartidoRespaldaId == null)
				.Select(a => a.PuestoElectivoId)
				.ToListAsync();

			return puestosOcupados;
		}

		public async Task<AsignacionCandidatos?> GetAsignacionDelCandidatoAsync(int candidatoId)
		{
			return await _context.AsignacionCandidatos
				.FirstOrDefaultAsync(x => x.CandidatoId == candidatoId && x.PartidoRespaldaId == null && x.Estado);
		}

		public async Task<List<AsignacionCandidatos>> GetAsignacionesConAliadosAsync(int partidoId)
		{
			await LiberarAsignacionesDePuestosInactivosAsync();

			var partidosAliadosIds = await _context.AlianzasPoliticas
				.Where(a => (a.PartidoSolicitanteId == partidoId || a.PartidoDestinoId == partidoId) && a.Estado)
				.Select(a => a.PartidoSolicitanteId == partidoId ? a.PartidoDestinoId : a.PartidoSolicitanteId)
				.ToListAsync();

			partidosAliadosIds.Add(partidoId);

			var asignaciones = await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.Include(a => a.PartidoQueRespalda)
				.Where(a => partidosAliadosIds.Contains(a.PartidoPoliticoId) && a.Estado)
				.OrderBy(a => a.Candidato.Nombre)
				.ThenBy(a => a.Candidato.Apellido)
				.ToListAsync();

			return asignaciones;
		}

		public async Task<List<AsignacionCandidatos>> GetRespaldosEntrePartidosAsync(int partidoId1, int partidoId2)
		{
			return await _context.AsignacionCandidatos
				.Where(a =>
					a.Estado == true &&
					a.PartidoRespaldaId != null &&
					(
						(a.PartidoPoliticoId == partidoId1 && a.PartidoRespaldaId == partidoId2) ||
						(a.PartidoPoliticoId == partidoId2 && a.PartidoRespaldaId == partidoId1)
					))
				.ToListAsync();
		}
		public async Task<List<AsignacionCandidatos>> GetAsignacionesConPuestosInactivosAsync()
		{
			return await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.Where(a => a.Estado == true && a.puestosElectivos.Estado == false)
				.ToListAsync();
		}

		public async Task<bool> DesactivarAsignacionesPorPuestoInactivoAsync(int puestoElectivoId)
		{
			var asignaciones = await _context.AsignacionCandidatos
				.Where(a => a.PuestoElectivoId == puestoElectivoId && a.Estado == true)
				.ToListAsync();

			foreach (var asignacion in asignaciones)
			{
				asignacion.Estado = false;
			}

			await _context.SaveChangesAsync();
			return asignaciones.Count > 0;
		}

		public async Task<bool> LiberarAsignacionesDePuestosInactivosManualAsync()
		{
			await LiberarAsignacionesDePuestosInactivosAsync();
			return true;
		}


	}
}