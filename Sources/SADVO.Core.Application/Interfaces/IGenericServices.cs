using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADVO.Core.Application.Interfaces
{
	public interface IGenericServices<TDto, TEntity>
	   where TEntity : class
	   where TDto : class
	{
		Task<bool> AddAsync(TDto dto);
		Task<bool> UpdateAsync(int id, TDto dto);
		Task UpdateEstadoAsync(int id, bool nuevoEstado);
		Task<List<TDto>> GetAllAsync();
		IQueryable<TDto> GetAllQuery();
		Task<TDto?> GetByIdAsync(int id);
		Task<List<TDto>> GetAllListWithIncludeAsync(List<string> properties);
		IQueryable<TDto> GetAllQueryWithInclude(List<string> properties);
	}
}
