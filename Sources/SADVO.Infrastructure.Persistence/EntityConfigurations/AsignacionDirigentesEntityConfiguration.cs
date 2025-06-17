using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class AsignacionDirigentesEntityConfiguration : IEntityTypeConfiguration<AsignacionDirigentes>
	{
		public void Configure(EntityTypeBuilder<AsignacionDirigentes> modelBuilder)
		{
			modelBuilder.ToTable("AsignacionDirigentes");
			modelBuilder.HasKey(ad => ad.Id);

			modelBuilder
				.HasOne(ad => ad.Usuario)
				.WithOne(u => u.AsignacionDirigente)
				.HasForeignKey<AsignacionDirigentes>(ad => ad.UsuarioId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(ad => ad.PartidoPolitico)
				.WithMany(pp => pp.Dirigentes)
				.HasForeignKey(ad => ad.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);

			// Configuración adicional
			modelBuilder.Property(ad => ad.FechaAsignacion)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");
		}
	}
}