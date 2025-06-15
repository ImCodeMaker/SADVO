using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IPartidoPoliticoServices : IGenericServices<CrearPartidosPoliticosDTO,UpdatePartidoPoliticoDTO,PartidosPoliticosDTO, PartidosPoliticos>
	{
		Task<bool> AddPartido(CrearPartidosPoliticosDTO dto, IFormFile? logoFile);
		Task<bool> UpdateAsync(int id, UpdatePartidoPoliticoDTO dto, IFormFile? logoFile = null);

	}
}
