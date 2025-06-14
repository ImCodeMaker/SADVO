using AutoMapper;
using SADVO.Core.Application.Dtos.PuestoElectivo;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class PuestosElectivosServices : GenericService<PuestoElectivoDTO,PuestosElectivos>, IPuestosElectivosServices
	{
		private readonly IPuestosElectivosRepository _puestosElectivosRepository;

		public PuestosElectivosServices(IPuestosElectivosRepository puestosElectivosRepository, IMapper mapper)
			: base(puestosElectivosRepository, mapper)
		{
			_puestosElectivosRepository = puestosElectivosRepository;
		}

		public override async Task<bool> AddAsync(PuestoElectivoDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			var puestosElectivos = await _puestosElectivosRepository.GetAllList();

			bool isDuplicated = puestosElectivos.Any(p => p.Nombre == dto.Nombre);
			if (isDuplicated)
			{
				return false;
			}
			return await base.AddAsync(dto);
		}

		public override async Task<bool> UpdateAsync(int id, PuestoElectivoDTO dto)
		{

			var puestosElectivos = await _puestosElectivosRepository.GetAllList();

			bool isDuplicated = puestosElectivos.Any(x => x.Nombre.ToLower() == dto.Nombre.ToLower() && x.Id != id);


			if (isDuplicated)
			{
				throw new InvalidOperationException("Ya existe un puesto electivo con ese nombre.");
			}

			return await base.UpdateAsync(id, dto);
		}

	}
}
