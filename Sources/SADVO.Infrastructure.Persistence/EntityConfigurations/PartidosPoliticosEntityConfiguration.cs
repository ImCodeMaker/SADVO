using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class PartidosPoliticosEntityConfiguration : IEntityTypeConfiguration<PartidosPoliticos>
	{
		public void Configure(EntityTypeBuilder<PartidosPoliticos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(PartidosPoliticos));

			modelBuilder.HasKey(p => p.Id);

			#region Propiedades

			modelBuilder.Property(p => p.Nombre)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(p => p.Descripcion)
				.IsRequired()
				.HasMaxLength(500);

			modelBuilder.Property(p => p.Siglas)
				.IsRequired()
				.HasMaxLength(20);

			modelBuilder.Property(p => p.Logo)
				.IsRequired()
				.HasMaxLength(250);

			#endregion

			#region Relaciones

			// Relaciones con AlianzasPoliticas (como solicitante)
			modelBuilder
				.HasMany(p => p.AlianzasSolicitadas)
				.WithOne(a => a.PartidoSolicitante)
				.HasForeignKey(a => a.PartidoSolicitanteId)
				.OnDelete(DeleteBehavior.Restrict);

			// Relaciones con AlianzasPoliticas (como destino)
			modelBuilder
				.HasMany(p => p.AlianzasRecibidas)
				.WithOne(a => a.PartidoDestino)
				.HasForeignKey(a => a.PartidoDestinoId)
				.OnDelete(DeleteBehavior.Restrict);

			// Relaciones con AsignacionCandidatos
			modelBuilder
				.HasMany(p => p.AsignacionesCandidatos)
				.WithOne(a => a.PartidosPoliticos)
				.HasForeignKey(a => a.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);


			// Relaciones con AsignacionDirigentes
			modelBuilder
				.HasMany(p => p.Dirigentes)
				.WithOne(d => d.PartidoPolitico)
				.HasForeignKey(d => d.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion
		}
	}
}
