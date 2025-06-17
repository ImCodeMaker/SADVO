using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.Candidatos;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface ICandidatoServices : IGenericServices<CrearCandidatosDTO,UpdateCandidatoDTO,CandidatosDTO, Candidatos>
	{
		Task<bool> AddCandidato(CrearCandidatosDTO dto, IFormFile? logoFile);
		Task<bool> UpdateAsync(int id, UpdateCandidatoDTO dto, IFormFile? logoFile = null);

	}
}
