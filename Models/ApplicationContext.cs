using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Threading;
using System.Threading.Tasks;

namespace Clientes.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=Clientes;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(p=>
            {
                p.ToTable("Clientes");
                p.HasKey(p => p.Id);
                p.Property(p => p.Nome).HasColumnType("VARCHAR(100)").IsRequired();
                p.Property(p => p.TipoCliente).HasConversion<int>();
                p.Property(p => p.Documento).HasColumnType("VARCHAR(20)").IsRequired();
                p.Property(p => p.Cadastro).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                p.Property(p => p.Telefone).HasColumnType("VARCHAR(11)").IsRequired();
                p.Property(p => p.IsDeleted).HasColumnType("BIT").GetType();

                p.HasIndex(i => i.Nome).HasDatabaseName("idx_cliente_nome");
                p.HasIndex(i => i.Documento).HasDatabaseName("idx_cliente_documento");

            });
            modelBuilder.Entity<Cliente>().HasQueryFilter(p => !p.IsDeleted);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is Cliente)
                {
                    entry.State = EntityState.Modified;
                    ((Cliente)entry.Entity).IsDeleted = true;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is Cliente)
                {
                    entry.State = EntityState.Modified;
                    ((Cliente)entry.Entity).IsDeleted = true;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }

}
