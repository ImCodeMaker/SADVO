using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class PartidosPoliticosRepository : GenericRepository<PartidosPoliticos>, IPartidosPoliticosRepository
	{
		private readonly SADVODbContext _context;

		public PartidosPoliticosRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}
	}
}
