using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public virtual DbSet<Conta> Contas { get; set; }
        public virtual DbSet<Transferencia> Transferencias { get; set; }
        public virtual DbSet<Transacao> Transacoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Meta;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Conta>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
        }
    }
}
