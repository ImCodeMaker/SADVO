using AutoMapper;
using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class CandidatosServices : GenericService<CrearCandidatosDTO, UpdateCandidatoDTO, CandidatosDTO, Candidatos>, ICandidatoServices
	{
		private readonly ICandidatosRepository _candidatosRepository;

		public CandidatosServices(ICandidatosRepository candidatosRepository, IMapper mapper) : base(candidatosRepository, mapper)
		{
			_candidatosRepository = candidatosRepository;
		}

		public async Task<bool> AddCandidato(CrearCandidatosDTO dto, IFormFile? logoFile, int PartidoId, string PartidoName)
		{
			var entity = _mapper.Map<Candidatos>(dto);

			entity.Foto = "default.png";
			entity.PartidoPoliticoId = PartidoId;
			entity.PartidoName = PartidoName;
			await _candidatosRepository.AddAsync(entity);

			if (logoFile != null)
			{
				string logoPath = UploadFile.Upload(logoFile, entity.Id, "Candidatos");
				entity.Foto = logoPath;

				await _candidatosRepository.UpdateAsync(entity.Id, entity);
			}

			return true;
		}

		public async Task<bool> UpdateAsync(int id, UpdateCandidatoDTO dto, IFormFile? logoFile = null)
		{
			try
			{
				var existingEntity = await _candidatosRepository.GetById(id);
				if (existingEntity == null)
					return false;

				_mapper.Map(dto, existingEntity);

				if (logoFile != null)
				{
					string logoPath = UploadFile.Upload(logoFile, id, "PartidosPoliticos");
					existingEntity.Foto = logoPath;
				}

				await _candidatosRepository.UpdateAsync(id, existingEntity);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> TienePuestoElectivo(int candidatoId)
		{
			var candidato = await _candidatosRepository.GetCandidatoConAsignacionesAsync(candidatoId);

			if (candidato == null)
				return false;

			return candidato.Asignaciones != null && candidato.Asignaciones.Any();
		}

		public async Task<List<CandidatosDTO>> GetCandidatosByPartidoAsync(int partidoId)
		{
			var entities = await _candidatosRepository.GetCandidatosByPartidoAsync(partidoId);
			return _mapper.Map<List<CandidatosDTO>>(entities);
		}
	}
}
