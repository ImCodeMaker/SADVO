using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class PuestosElectivosRepository : GenericRepository<PuestosElectivos>, IPuestosElectivosRepository
	{
		private readonly SADVODbContext _context;

		public PuestosElectivosRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

	}

}