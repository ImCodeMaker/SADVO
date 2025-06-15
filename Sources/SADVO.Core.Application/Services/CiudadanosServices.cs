using AutoMapper;
using SADVO.Core.Application.Dtos.Ciudadanos;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class CiudadanosServices : GenericService<CrearCiudadanos, UpdateCiudadanosDTO, CiudadanosDTO, Ciudadanos>,
									 ICiudadanosServices
	{
		private readonly ICiudadanosRepository _repository;

		public CiudadanosServices(ICiudadanosRepository ciudadanosRepository, IMapper mapper)
			: base(ciudadanosRepository, mapper)
		{
			_repository = ciudadanosRepository;
		}

		public override async Task<bool> AddAsync(CrearCiudadanos dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			if (await ExisteCiudadanoConDocumentoOEmail(dto.Documento_Identidad, dto.Email))
				throw new InvalidOperationException("Ya existe un ciudadano con este documento o email.");

			return await base.AddAsync(dto);
		}

		public override async Task<bool> UpdateAsync(int id, UpdateCiudadanosDTO dto)
		{
			if (await ExisteCiudadanoConDocumentoOEmail(dto.Documento_Identidad, dto.Email, id))
				throw new InvalidOperationException("Ya existe un ciudadano con este documento o email.");

			return await base.UpdateAsync(id, dto);
		}

		private async Task<bool> ExisteCiudadanoConDocumentoOEmail(string documento, string email, int? idExcluir = null)
		{
			var ciudadanos = await _repository.GetAllList();
			return ciudadanos.Any(c =>
				(c.Documento_Identidad == documento || c.Email == email) &&
				(idExcluir == null || c.Id != idExcluir.Value));
		}
	}
}