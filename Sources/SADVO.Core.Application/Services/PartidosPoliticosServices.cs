using AutoMapper;
using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class PartidosPoliticosServices : GenericService<CrearPartidosPoliticosDTO,UpdatePartidoPoliticoDTO,PartidosPoliticosDTO, PartidosPoliticos>, IPartidoPoliticoServices
	{
		private readonly IPartidosPoliticosRepository _partidosPoliticosRepository;

		public PartidosPoliticosServices(IPartidosPoliticosRepository partidosPoliticosRepository, IMapper mapper) : base(partidosPoliticosRepository, mapper)
		{
			_partidosPoliticosRepository = partidosPoliticosRepository;
		}

		public async Task<bool> AddPartido(CrearPartidosPoliticosDTO dto, IFormFile? logoFile)
		{
			var entity = _mapper.Map<PartidosPoliticos>(dto);

			entity.Logo = "default.png";
			await _partidosPoliticosRepository.AddAsync(entity);

			if (logoFile != null)
			{
				string logoPath = UploadFile.Upload(logoFile, entity.Id, "PartidosPoliticos");
				entity.Logo = logoPath;


				await _partidosPoliticosRepository.UpdateAsync(entity.Id,entity);
			}

			return true;
		}

		public async Task<bool> UpdateAsync(int id, UpdatePartidoPoliticoDTO dto, IFormFile? logoFile = null)
		{
			try
			{
				var existingEntity = await _partidosPoliticosRepository.GetById(id);
				if (existingEntity == null)
					return false;

				_mapper.Map(dto, existingEntity);

				// Si hay un nuevo archivo de logo, procesarlo
				if (logoFile != null)
				{
					string logoPath = UploadFile.Upload(logoFile, id, "PartidosPoliticos");
					existingEntity.Logo = logoPath;
				}
				// Si no hay nuevo archivo, mantener el logo existente (no cambiar)

				// Actualizar la entidad
				await _partidosPoliticosRepository.UpdateAsync(id, existingEntity);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}