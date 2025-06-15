using AutoMapper;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class PuestosElectivosServices : GenericService<CrearPuestoElectivoDTO, UpdatePuestoElectivoDTO, PuestoElectivoDTO, PuestosElectivos>,
										  IPuestosElectivosServices
	{
		private readonly IPuestosElectivosRepository _repository;

		public PuestosElectivosServices(IPuestosElectivosRepository repository, IMapper mapper)
			: base(repository, mapper)
		{
			_repository = repository;
		}

		public override async Task<bool> AddAsync(CrearPuestoElectivoDTO dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			if (await ExistePuestoConNombre(dto.Nombre))
				throw new InvalidOperationException("Ya existe un puesto electivo con ese nombre.");

			return await base.AddAsync(dto);
		}

		public override async Task<bool> UpdateAsync(int id, UpdatePuestoElectivoDTO dto)
		{
			if (await ExistePuestoConNombre(dto.Nombre, id))
				throw new InvalidOperationException("Ya existe un puesto electivo con ese nombre.");

			return await base.UpdateAsync(id, dto);
		}

		private async Task<bool> ExistePuestoConNombre(string nombre, int? idExcluir = null)
		{
			var puestos = await _repository.GetAllList();
			return puestos.Any(p =>
				p.Nombre.Equals(nombre, StringComparison.CurrentCultureIgnoreCase) &&
				(idExcluir == null || p.Id != idExcluir.Value));
		}
	}
}