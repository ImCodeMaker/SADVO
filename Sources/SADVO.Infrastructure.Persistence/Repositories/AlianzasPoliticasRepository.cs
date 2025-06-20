using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class AlianzasPoliticasRepository : GenericRepository<AlianzasPoliticas>, IAlianzasPoliticasRepository
	{
		private readonly SADVODbContext _context;

		public AlianzasPoliticasRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<AlianzasPoliticas>> GetSolicitudesPendientesAsync(int partidoId)
		{
			return await _context.AlianzasPoliticas
				.Include(a => a.PartidoSolicitante)
				.Where(a => a.PartidoDestinoId == partidoId
						   && a.EstadoAlianza == EstadoAlianza.EnEspera
						   && a.Estado == true)
				.OrderByDescending(a => a.FechaSolicitud)
				.ToListAsync();
		}

		public async Task<List<AlianzasPoliticas>> GetSolicitudesEnviadasAsync(int partidoId)
		{
			return await _context.AlianzasPoliticas
				.Include(a => a.PartidoDestino)
				.Where(a => a.PartidoSolicitanteId == partidoId && a.Estado == true)
				.OrderByDescending(a => a.FechaSolicitud)
				.ToListAsync();
		}

		public async Task<List<AlianzasPoliticas>> GetAlianzasActivasAsync(int partidoId)
		{
			return await _context.AlianzasPoliticas
				.Include(a => a.PartidoSolicitante)
				.Include(a => a.PartidoDestino)
				.Where(a => (a.PartidoSolicitanteId == partidoId || a.PartidoDestinoId == partidoId)
						   && a.EstadoAlianza == EstadoAlianza.Aceptada
						   && a.Estado == true)
				.OrderByDescending(a => a.FechaRespuesta)
				.ToListAsync();
		}

		public async Task<AlianzasPoliticas?> GetByIdAsync(int id)
		{
			return await _context.AlianzasPoliticas
				.Include(a => a.PartidoSolicitante)
				.Include(a => a.PartidoDestino)
				.FirstOrDefaultAsync(a => a.Id == id && a.Estado == true);
		}

		public async Task<bool> ExisteRelacionPendienteAsync(int solicitanteId, int destinoId)
		{
			return await _context.AlianzasPoliticas.AnyAsync(a =>
				(a.PartidoSolicitanteId == solicitanteId && a.PartidoDestinoId == destinoId
				|| a.PartidoSolicitanteId == destinoId && a.PartidoDestinoId == solicitanteId)
				&& a.EstadoAlianza == EstadoAlianza.EnEspera
				&& a.Estado == true);
		}

	}
}