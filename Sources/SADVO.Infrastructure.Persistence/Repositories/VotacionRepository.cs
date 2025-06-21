using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class VotacionRepository : IVotacionRepository
	{
		private readonly SADVODbContext _context;

		public VotacionRepository(SADVODbContext context)
		{
			_context = context;
		}

		public async Task<List<AsignacionCandidatos>> GetCandidatosParaVotacionAsync(int eleccionId)
		{
			var eleccion = await _context.Elecciones
				.FirstOrDefaultAsync(e => e.Id == eleccionId && e.Estado == true);

			if (eleccion == null)
				return new List<AsignacionCandidatos>();

			var asignacionesOriginales = await _context.AsignacionCandidatos
				.Include(a => a.Candidato)
				.Include(a => a.puestosElectivos)
				.Include(a => a.PartidosPoliticos)
				.Where(a => a.Estado == true &&
						   a.PartidoRespaldaId == null && 
						   a.Candidato.Estado == true &&
						   a.puestosElectivos.Estado == true &&
						   a.PartidosPoliticos.Estado == true)
				.OrderBy(a => a.puestosElectivos.Nombre)
				.ThenBy(a => a.Candidato.Nombre)
				.ThenBy(a => a.Candidato.Apellido)
				.ToListAsync();

			return asignacionesOriginales;
		}

		public async Task<List<AsignacionCandidatos>> GetRespaldosParaCandidatoAsync(int candidatoId, int puestoElectivoId)
		{
			return await _context.AsignacionCandidatos
				.Include(a => a.PartidoQueRespalda)
				.Where(a => a.CandidatoId == candidatoId &&
						   a.PuestoElectivoId == puestoElectivoId &&
						   a.PartidoRespaldaId != null &&
						   a.Estado == true)
				.ToListAsync();
		}

		public async Task<bool> YaVotoEnEleccionAsync(int ciudadanoId, int eleccionId)
		{
			return await _context.HistorialVotaciones
				.AnyAsync(h => h.CiudadanoId == ciudadanoId &&
							  h.EleccionId == eleccionId &&
							  h.HaVotado == true);
		}

		public async Task<bool> YaVotoEnPuestoAsync(int ciudadanoId, int eleccionId, int puestoElectivoId)
		{
			return await _context.Votos
				.AnyAsync(v => v.CiudadanoId == ciudadanoId &&
							  v.EleccionId == eleccionId &&
							  v.PuestoElectivoId == puestoElectivoId);
		}

		public async Task<bool> RegistrarVotoAsync(Votos voto)
		{
			try
			{
				_context.Votos.Add(voto);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> RegistrarHistorialVotacionAsync(HistorialVotaciones historial)
		{
			try
			{
				var historialExistente = await _context.HistorialVotaciones
					.FirstOrDefaultAsync(h => h.CiudadanoId == historial.CiudadanoId &&
											 h.EleccionId == historial.EleccionId);

				if (historialExistente != null)
				{
					historialExistente.HaVotado = true;
					historialExistente.FechaVotacion = historial.FechaVotacion;
					_context.HistorialVotaciones.Update(historialExistente);
				}
				else
				{
					_context.HistorialVotaciones.Add(historial);
				}

				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public async Task<Elecciones?> GetEleccionActivaAsync()
		{
			return await _context.Elecciones
				.FirstOrDefaultAsync(e => e.Estado == true );
		}
		public async Task<List<int>> GetPuestosVotadosPorCiudadanoAsync(int ciudadanoId, int eleccionId)
		{
			return await _context.Votos
				.Where(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId)
				.Select(v => v.PuestoElectivoId)
				.ToListAsync();
		}

	}
}