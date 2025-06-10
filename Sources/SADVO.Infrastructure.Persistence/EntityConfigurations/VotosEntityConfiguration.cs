using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class VotosEntityConfiguration : IEntityTypeConfiguration<Votos>
	{
		public void Configure(EntityTypeBuilder<Votos> modelBuilder)
		{
			modelBuilder.ToTable(nameof(Votos));

			modelBuilder.HasKey(v => v.Id);

			modelBuilder.Property(v => v.FechaVoto)
				.IsRequired();

			modelBuilder
				.HasOne(v => v.Eleccion)
				.WithMany(e => e.Votos)
				.HasForeignKey(v => v.EleccionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(v => v.Ciudadano)
				.WithMany(c => c.Votos)
				.HasForeignKey(v => v.CiudadanoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(v => v.PuestoElectivo)
				.WithMany(p => p.Votos)
				.HasForeignKey(v => v.PuestoElectivoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(v => v.Candidato)
				.WithMany() 
				.HasForeignKey(v => v.CandidatoId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
				.HasOne(v => v.PartidoPolitico)
				.WithMany()  
				.HasForeignKey(v => v.PartidoPoliticoId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
