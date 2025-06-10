using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class PuestosElectivosEntityConfiguration : IEntityTypeConfiguration<PuestosElectivos>
	{
		public void Configure(EntityTypeBuilder<PuestosElectivos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(PuestosElectivos));

			modelBuilder.HasKey(p => p.Id);

			#region Propiedades

			modelBuilder.Property(p => p.Nombre)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(p => p.Descripcion)
				.IsRequired()
				.HasMaxLength(500);

			#endregion

			#region Relaciones

			modelBuilder
				.HasMany(p => p.AsignacionesCandidatos)
				.WithOne(a => a.puestosElectivos)   // la propiedad en AsignacionCandidatos es puestosElectivos
				.HasForeignKey(a => a.PuestoElectivoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasMany(p => p.Votos)
				.WithOne(v => v.PuestoElectivo)   // asumo la propiedad en Votos es PuestosElectivos, revisa que así sea
				.HasForeignKey(v => v.PuestoElectivoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion
		}
	}
}
