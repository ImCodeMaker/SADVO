using AutoMapper;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class CiudadanosServices : GenericService<CiudadanosDTO, Ciudadanos>, ICiudadanosServices
	{
		private readonly ICiudadanosRepository _ciudadanosRepository;

		public CiudadanosServices(ICiudadanosRepository ciudadanosRepository, IMapper mapper) : base(ciudadanosRepository,mapper)
		{
			_ciudadanosRepository = ciudadanosRepository;
		}

		public override async Task<bool> AddAsync(CiudadanosDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			var puestosElectivos = await _ciudadanosRepository.GetAllList();

			bool isDuplicated = puestosElectivos.Any(p => p.Documento_Identidad == dto.Documento_Identidad || p.Email == dto.Email);
			if (isDuplicated)
			{
				return false;
			}
			return await base.AddAsync(dto);
		}


	}
}
