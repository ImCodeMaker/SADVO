using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Interfaces;
using SADVO.Infrastructure.Persistence.Contexts;


namespace SADVO.Infrastructure.Persistence.Repositories
{
	public class GenericRepository<Entity> : IGenericRepository<Entity>
		where Entity : class 
	{
		private readonly SADVODbContext _context;

		//Create

		public GenericRepository(SADVODbContext context)
		{
			_context = context;
		}
		public async Task<Entity?> AddAsync(Entity entity)
		{
			await _context.Set<Entity>().AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		//Get by id

		public async Task DeleteAsync(int Id)
		{
			var entity = await _context.Set<Entity>().FindAsync(Id);
			if (entity != null)
			{
				_context.Set<Entity>().Remove(entity);
				await _context.SaveChangesAsync();
			}
		}

		//Get all elements
		public async Task<List<Entity>> GetAllList()
		{
			try
			{
				return await _context.Set<Entity>().ToListAsync();
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine($"Database error: {ex.Message}");
				throw;
			}
		}

		//Get By Id
		public async Task<Entity?> GetById(int Id)
		{
			return await _context.Set<Entity>().FindAsync(Id);
		}
		// Get as Query
		public IQueryable<Entity> GetAllQuery()
		{
			return _context.Set<Entity>().AsQueryable();
		}
		// Update
		public async Task<Entity?> UpdateAsync(int Id, Entity entity)
		{
			var entry = await _context.Set<Entity>().FindAsync(Id);

			if (entry != null)
			{
				_context.Entry(entry).CurrentValues.SetValues(entity);
				await _context.SaveChangesAsync();
				return entry;
			}

			return null;
		}

		public virtual async Task<List<Entity>> GetAllListWithInclude(List<string> properties)
		{
			var query = _context.Set<Entity>().AsQueryable();

			foreach (var property in properties)
			{
				query = query.Include(property);
			}
			return await query.ToListAsync();
		}

		public virtual IQueryable GetAllLQueryWithInclude(List<string> properties)
		{
			var query = _context.Set<Entity>().AsQueryable();

			foreach (var property in properties)
			{
				query = query.Include(property);
			}
			return query;
		}
	}
}
