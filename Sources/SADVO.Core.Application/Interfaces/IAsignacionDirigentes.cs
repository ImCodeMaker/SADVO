using SADVO.Core.Application.Dtos.AsignacionDirigentes;
using SADVO.Core.Domain.Entities;

namespace SADVO.Core.Application.Interfaces
{
	public interface IAsignacionDirigentesServices : IGenericServices<CreateAsignacionDirigentesDTO,NoUpdateDto,AsignacionDirigentesDTO,AsignacionDirigentes>
	{
	}
}
