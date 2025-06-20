using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Application.Dtos.PartidosPoliticos;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IAsignacionDirigentesServices : IGenericServices<CreateAsignacionDirigentesDTO,NoUpdateDto,AsignacionDirigentesDTO,AsignacionDirigentes>
	{
		Task<AsignacionDirigentesDTO> GetDirigente(int userId);
		Task<int?> GetPartidoIdByUserIdAsync(int userId);
	}
}
