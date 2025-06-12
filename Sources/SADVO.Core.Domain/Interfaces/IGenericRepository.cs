namespace SADVO.Core.Domain.Interfaces
{
	public interface IGenericRepository<Entity>
	  where Entity : class
	{
		Task<Entity?> AddAsync(Entity entity);
		Task DeleteAsync(int Id);
		Task<List<Entity>> GetAllList();
		Task<Entity?> GetById(int Id);
		IQueryable<Entity> GetAllQuery();
		Task<Entity?> UpdateAsync(int Id, Entity entity);
		Task<List<Entity>> GetAllListWithInclude(List<string> properties);
		IQueryable GetAllLQueryWithInclude(List<string> properties);
	}
}
