using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class HistorialVotacionesEntityConfiguration : IEntityTypeConfiguration<HistorialVotaciones>
	{
		public void Configure(EntityTypeBuilder<HistorialVotaciones> modelBuilder)
		{
			modelBuilder.ToTable(nameof(HistorialVotaciones));

			modelBuilder.HasKey(h => h.Id);

			#region Propiedades

			modelBuilder.Property(h => h.EleccionId)
				.IsRequired();

			modelBuilder.Property(h => h.CiudadanoId)
				.IsRequired();

			modelBuilder.Property(h => h.HaVotado)
				.IsRequired();

			modelBuilder.Property(h => h.FechaVotacion)
				.IsRequired();

			modelBuilder.Property(h => h.EmailEnviado)
				.IsRequired();

			#endregion

			#region Relaciones

			modelBuilder
				.HasOne(h => h.Eleccion)
				.WithMany(e => e.HistorialVotaciones)
				.HasForeignKey(h => h.EleccionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(h => h.Ciudadano)
				.WithMany(c => c.HistorialVotaciones)
				.HasForeignKey(h => h.CiudadanoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion
		}
	}
}
