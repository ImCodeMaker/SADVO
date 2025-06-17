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

			modelBuilder
				.HasMany(c => c.asignacionCandidato)
				.WithOne(a => a.Candidato)
				.HasForeignKey(a => a.CandidatoId)
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
