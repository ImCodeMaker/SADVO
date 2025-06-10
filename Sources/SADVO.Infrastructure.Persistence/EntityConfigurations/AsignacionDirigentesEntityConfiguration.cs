using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class AsignacionDirigentesEntityConfiguration : IEntityTypeConfiguration<AsignacionDirigentes>
	{
		public void Configure(EntityTypeBuilder<AsignacionDirigentes> modelBuilder)
		{
			modelBuilder.ToTable(nameof(AsignacionDirigentes));
			modelBuilder.HasKey(ad => ad.Id);

			#region Relaciones

			// Uno a uno: Usuario tiene una única Asignación
			modelBuilder
				.HasOne(ad => ad.Usuario)
				.WithOne(u => u.AsignacionDirigente)
				.HasForeignKey<AsignacionDirigentes>(ad => ad.UsuarioId)
				.OnDelete(DeleteBehavior.Restrict);

			// Muchos a uno: Muchos dirigentes pueden pertenecer al mismo partido
			modelBuilder
				.HasOne(ad => ad.partidosPoliticos)
				.WithMany()
				.HasForeignKey(ad => ad.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			modelBuilder
				.Property(ad => ad.FechaAsignacion)
				.IsRequired();
		}
	}
}
