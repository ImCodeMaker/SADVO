using AutoMapper;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class GenericService<TCreateDto, TUpdateDto, TDto, TEntity> : IGenericServices<TCreateDto, TUpdateDto, TDto, TEntity>
		where TEntity : class
		where TCreateDto : class
		where TUpdateDto : class
		where TDto : class
	{
		protected readonly IGenericRepository<TEntity> _repository;
		protected readonly IMapper _mapper;

		public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public virtual async Task<bool> AddAsync(TCreateDto createDto)
		{
			if (createDto == null) throw new ArgumentNullException(nameof(createDto));

			var entity = _mapper.Map<TEntity>(createDto);
			await _repository.AddAsync(entity);

			return true;
		}

		public virtual async Task<bool> UpdateAsync(int id, TUpdateDto updateDto)
		{
			if (updateDto == null) throw new ArgumentNullException(nameof(updateDto));

			var existingEntity = await _repository.GetById(id);
			if (existingEntity == null) return false;

			_mapper.Map(updateDto, existingEntity);
			await _repository.UpdateAsync(id, existingEntity);

			return true;
		}

		public virtual async Task UpdateEstadoAsync(int id, bool nuevoEstado)
		{
			await _repository.UpdateEstadoAsync(id, nuevoEstado);
		}

		public virtual async Task<List<TDto>> GetAllAsync()
		{
			var entities = await _repository.GetAllList();
			return _mapper.Map<List<TDto>>(entities);
		}

		public virtual IQueryable<TDto> GetAllQuery()
		{
			return _repository.GetAllQuery().Select(e => _mapper.Map<TDto>(e));
		}

		public virtual async Task<TDto?> GetByIdAsync(int id)
		{
			var entity = await _repository.GetById(id);
			return entity == null ? null : _mapper.Map<TDto>(entity);
		}

		public virtual async Task<List<TDto>> GetAllListWithIncludeAsync(List<string> properties)
		{
			var entities = await _repository.GetAllListWithInclude(properties);
			return _mapper.Map<List<TDto>>(entities);
		}

		public virtual IQueryable<TDto> GetAllQueryWithInclude(List<string> properties)
		{
			var query = _repository.GetAllLQueryWithInclude(properties);
			return query.Cast<TEntity>().Select(e => _mapper.Map<TDto>(e));
		}

	}
}