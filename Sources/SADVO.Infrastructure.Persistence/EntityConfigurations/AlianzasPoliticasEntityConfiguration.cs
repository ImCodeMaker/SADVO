using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class AlianzasPoliticasEntityConfiguration : IEntityTypeConfiguration<AlianzasPoliticas>
	{
		public void Configure(EntityTypeBuilder<AlianzasPoliticas> modelBuilder)
		{
			modelBuilder.ToTable(nameof(AlianzasPoliticas));
			modelBuilder.HasKey(a => a.Id);

			#region Relaciones

			modelBuilder
				.HasOne(a => a.PartidoSolicitante)
				.WithMany(p => p.AlianzasSolicitadas)
				.HasForeignKey(a => a.PartidoSolicitanteId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(a => a.PartidoDestino)
				.WithMany(p => p.AlianzasRecibidas)
				.HasForeignKey(a => a.PartidoDestinoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			modelBuilder.Property(a => a.Estado)
				.IsRequired()
				.HasMaxLength(10);

			modelBuilder.Property(a => a.FechaSolicitud)
				.IsRequired();

			modelBuilder.Property(a => a.FechaRespuesta)
				.IsRequired();
		}
	}
}
