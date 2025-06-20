using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class CandidatosEntityConfiguration : IEntityTypeConfiguration<Candidatos>
	{
		public void Configure(EntityTypeBuilder<Candidatos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(Candidatos));
			modelBuilder.HasKey(c => c.Id);

			#region Relaciones

			// Un candidato tiene muchas asignaciones
			modelBuilder
				.HasMany(c => c.Asignaciones)
				.WithOne(a => a.Candidato)
				.HasForeignKey(a => a.CandidatoId)
				.OnDelete(DeleteBehavior.Restrict);

			// Un candidato pertenece a un partido político
			modelBuilder
				.HasOne(c => c.PartidoPolitico)
				.WithMany()
				.HasForeignKey(c => c.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			#region Reglas de propiedades

			modelBuilder.Property(c => c.Nombre)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(c => c.Apellido)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(c => c.Foto)
				.IsRequired();

			modelBuilder.Property(c => c.Estado)
				.IsRequired();

			modelBuilder.Property(c => c.FechaCreacion)
				.IsRequired();

			#endregion
		}
	}
}
