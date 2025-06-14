using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class CiudadanosRepository : GenericRepository<Ciudadanos>, ICiudadanosRepository
	{
		private readonly SADVODbContext _context;

		public CiudadanosRepository(SADVODbContext context) : base(context) 
		{ 
		_context = context;
		}
	}
}
