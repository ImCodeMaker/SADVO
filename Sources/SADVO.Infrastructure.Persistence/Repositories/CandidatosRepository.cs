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
	}
}
