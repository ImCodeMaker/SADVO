using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SADVO.Core.Domain.Entities;

namespace SADVO.Infrastructure.Persistence.EntityConfigurations
{
	public class UsuariosEntityConfiguration : IEntityTypeConfiguration<Usuarios>
	{
		public void Configure(EntityTypeBuilder<Usuarios> modelBuilder)
		{
			modelBuilder.ToTable(nameof(Usuarios));

			// Key
			modelBuilder.HasKey(u => u.Id);

			// Properties
			modelBuilder.Property(u => u.Nombre)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(u => u.Apellido)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(150);

			modelBuilder.Property(u => u.NombreUsuario)
				.IsRequired()
				.HasMaxLength(100);

			modelBuilder.Property(u => u.Contraseña)
				.IsRequired()
				.HasMaxLength(200);

			modelBuilder.Property(u => u.Rol)
				.IsRequired()
				.HasMaxLength(50);

			// Unique Indexes
			modelBuilder.HasIndex(u => u.Email)
				.IsUnique();

			modelBuilder.HasIndex(u => u.NombreUsuario)
				.IsUnique();

			// Relationships
			modelBuilder
				.HasOne(u => u.AsignacionDirigente)
				.WithOne(ad => ad.Usuario)
				.HasForeignKey<AsignacionDirigentes>(ad => ad.UsuarioId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
