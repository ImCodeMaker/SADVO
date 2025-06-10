using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class AsignacionCandidatosEntityConfiguration : IEntityTypeConfiguration<AsignacionCandidatos>
	{
		public void Configure(EntityTypeBuilder<AsignacionCandidatos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(AsignacionCandidatos));
			modelBuilder.HasKey(ac => ac.Id);

			#region Relaciones

			modelBuilder
				.HasOne(ac => ac.Candidato)
				.WithMany()
				.HasForeignKey(ac => ac.CandidatoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(ac => ac.puestosElectivos)
				.WithMany()
				.HasForeignKey(ac => ac.PuestoElectivoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(ac => ac.PartidosPoliticos)
				.WithMany()
				.HasForeignKey(ac => ac.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			modelBuilder
				.Property(ac => ac.FechaAsignacion)
				.IsRequired();
		}
	}
}
