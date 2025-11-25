using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Modelos;

namespace FitLifeAPI.Dados
{
    public class FitLifeContexto : DbContext
    {
        public FitLifeContexto(DbContextOptions<FitLifeContexto> options) : base(options)
        {
        }

        // Tabelas do banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TreinoCardio> TreinosCardio { get; set; }
        public DbSet<TreinoForca> TreinosForca { get; set; }
        public DbSet<Refeicao> Refeicoes { get; set; }
        public DbSet<Habito> Habitos { get; set; }
        public DbSet<Progresso> Progressos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de herança TPH (Table Per Hierarchy)
            modelBuilder.Entity<Treino>()
                .HasDiscriminator<string>("TipoTreino")
                .HasValue<TreinoCardio>("Cardio")
                .HasValue<TreinoForca>("Forca");
        }
    }
}