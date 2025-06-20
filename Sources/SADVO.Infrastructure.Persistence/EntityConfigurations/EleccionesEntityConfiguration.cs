using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class EleccionesEntityConfiguration : IEntityTypeConfiguration<Elecciones>
	{
		public void Configure(EntityTypeBuilder<Elecciones> modelBuilder)
		{
			modelBuilder.ToTable(nameof(Elecciones));

			modelBuilder.HasKey(e => e.Id);

			#region Propiedades

			modelBuilder.Property(e => e.Nombre)
				.IsRequired()
				.HasMaxLength(150);

			modelBuilder.Property(e => e.FechaRealizacion)
				.IsRequired();

			modelBuilder.Property(e => e.Estado)
				.IsRequired()
				.HasMaxLength(50);

			#endregion

			#region Relaciones

			modelBuilder
				.HasMany(e => e.HistorialVotaciones)
				.WithOne(h => h.Eleccion)
				.HasForeignKey(h => h.EleccionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasMany(e => e.Votos)
				.WithOne(v => v.Eleccion)
				.HasForeignKey(v => v.EleccionId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion
		}
	}
}
