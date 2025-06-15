namespace SADVO.Core.Application.Interfaces
{
	public interface IGenericServices<TCreateDto, TUpdateDto, TDto, TEntity>
	where TEntity : class
	where TCreateDto : class
	where TUpdateDto : class
	where TDto : class
	{
		Task<bool> AddAsync(TCreateDto createDto);
		Task<bool> UpdateAsync(int id, TUpdateDto updateDto);
		Task UpdateEstadoAsync(int id, bool nuevoEstado);
		Task<List<TDto>> GetAllAsync();
		IQueryable<TDto> GetAllQuery();
		Task<TDto?> GetByIdAsync(int id);
		Task<List<TDto>> GetAllListWithIncludeAsync(List<string> properties);
		IQueryable<TDto> GetAllQueryWithInclude(List<string> properties);
	}
}
