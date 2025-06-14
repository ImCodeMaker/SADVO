using Microsoft.AspNetCore.Http;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IPartidoPoliticoServices : IGenericServices<PartidosPoliticosDTO, PartidosPoliticos>
	{
		Task<bool> AddPartido(PartidosPoliticosDTO dto, IFormFile? logoFile);
	}
}
