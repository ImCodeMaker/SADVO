using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class CiudadanosEntityConfiguration : IEntityTypeConfiguration<Ciudadanos>
	{
		public void Configure(EntityTypeBuilder<Ciudadanos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(Ciudadanos));

			modelBuilder.HasKey(c => c.Id);

			#region Propiedades

			modelBuilder.Property(c => c.FirstName)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(c => c.LastName)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(c => c.Email)
				.IsRequired()
				.HasMaxLength(150);

			modelBuilder.Property(c => c.Documento_Identidad)
				.IsRequired()
				.HasMaxLength(50);

			#endregion

			#region Relaciones

			modelBuilder
				.HasMany(c => c.HistorialVotaciones)
				.WithOne(h => h.Ciudadano)
				.HasForeignKey(h => h.CiudadanoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasMany(c => c.Votos)
				.WithOne(v => v.Ciudadano)
				.HasForeignKey(v => v.CiudadanoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			#region Índices Únicos

			modelBuilder.HasIndex(c => c.Email).IsUnique();
			modelBuilder.HasIndex(c => c.Documento_Identidad).IsUnique();

			#endregion
		}
	}
}
