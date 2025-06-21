using AutoMapper;
using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class AsignacionDirigentesServices : GenericService<CreateAsignacionDirigentesDTO, NoUpdateDto, AsignacionDirigentesDTO, AsignacionDirigentes>,
									 IAsignacionDirigentesServices
	{
		private new readonly IAsignacionDirigentesRepository _repository;

		public AsignacionDirigentesServices(
			IAsignacionDirigentesRepository asignacionRepository,
			IMapper mapper)
			: base(asignacionRepository, mapper)
		{
			_repository = asignacionRepository;
		}

		public override async Task<bool> AddAsync(CreateAsignacionDirigentesDTO dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			var existingAssignment = await _repository.GetById(dto.UsuarioId);

			if (existingAssignment != null)
			{
				throw new InvalidOperationException("Este usuario ya está asignado como dirigente a un partido político.");
			}

			return await base.AddAsync(dto);
		}

		public async Task<AsignacionDirigentesDTO> GetDirigente(int userId)
		{
			var getAllDirigentes = await _repository.GetAllList();
			var dirigente = getAllDirigentes.FirstOrDefault(u => u.UsuarioId == userId);

			if (dirigente == null || dirigente.Estado == false)
			{
				throw new InvalidOperationException("Este usuario no está relacionado con ningún partido político.");
			}

			// Mapear la entidad al DTO
			return _mapper.Map<AsignacionDirigentesDTO>(dirigente);
		}

		public async Task<int?> GetPartidoIdByUserIdAsync(int userId)
		{
			var asignacion = await _repository.GetByUserIdWithPartidoAsync(userId);
			if (asignacion == null)
				return null;
			return asignacion.PartidoPoliticoId;
		}
	}
}