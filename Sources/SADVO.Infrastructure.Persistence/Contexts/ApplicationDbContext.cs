using Microsoft.EntityFrameworkCore;
using SADVO.Core.Domain.Entities;
using SADVO.Infrastructure.Persistence.EntityConfigurations;

namespace SADVO.Infrastructure.Persistence.Contexts
{
	public class SADVODbContext : DbContext
	{
		public SADVODbContext(DbContextOptions<SADVODbContext> options) : base(options)
		{
		}

		public DbSet<AlianzasPoliticas> AlianzasPoliticas { get; set; }
		public DbSet<AsignacionCandidatos> AsignacionCandidatos { get; set; }
		public DbSet<AsignacionDirigentes> AsignacionDirigentes { get; set; }
		public DbSet<Candidatos> Candidatos { get; set; }
		public DbSet<Ciudadanos> Ciudadanos { get;set; }
		public DbSet<Elecciones> Elecciones { get; set; }
		public DbSet<HistorialVotaciones> HistorialVotaciones { get; set; }
		public DbSet<PartidosPoliticos>	PartidosPoliticos { get; set; }
		public DbSet<PuestosElectivos> PuestosElectivos { get; set; }
		public DbSet<Usuarios> Usuarios { get; set; }
		public DbSet<Votos> Votos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new AlianzasPoliticasEntityConfiguration());
			modelBuilder.ApplyConfiguration(new AsignacionCandidatosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new AsignacionDirigentesEntityConfiguration());
			modelBuilder.ApplyConfiguration(new CandidatosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new CiudadanosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new EleccionesEntityConfiguration());
			modelBuilder.ApplyConfiguration(new HistorialVotacionesEntityConfiguration());
			modelBuilder.ApplyConfiguration(new PartidosPoliticosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new PuestosElectivosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new UsuariosEntityConfiguration());
			modelBuilder.ApplyConfiguration(new VotosEntityConfiguration());
		}

	}
}
