using AutoMapper;
using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class PartidosPoliticosServices : GenericService<PartidosPoliticosDTO, PartidosPoliticos>, IPartidoPoliticoServices
	{
		private readonly IPartidosPoliticosRepository _partidosPoliticosRepository;

		public PartidosPoliticosServices(IPartidosPoliticosRepository partidosPoliticosRepository, IMapper mapper) : base(partidosPoliticosRepository, mapper)
		{
			_partidosPoliticosRepository = partidosPoliticosRepository;
		}

		public async Task<bool> AddPartido(PartidosPoliticosDTO dto, IFormFile? logoFile)
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
	}
}