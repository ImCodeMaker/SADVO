using Microsoft.EntityFrameworkCore;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;

namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class UserRepository : GenericRepository<Usuarios>, IUserRepository
	{
		private readonly SADVODbContext _context;

		public UserRepository(SADVODbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Usuarios?> LoginAsync(string username, string password)
		{
			string encriptedPassword = PasswordEncryption.ComputeSha256Hash(password);

			Usuarios? usuario = await _context.Set<Usuarios>().FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Contraseña == encriptedPassword);

			return usuario;
		}

	}

}