using DominandoEFCore_Modulo7_Interceptacao.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore_Modulo7_Interceptacao.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Funcao> Funcoes {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-02;Integrated Security=true;pooling=true;";

            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .AddInterceptors(new Interceptadores.InterceptadorDeComandos())
                .AddInterceptors(new Interceptadores.InterceptadorDeConexao())
                .AddInterceptors(new Interceptadores.InterceptadorPersistencia());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Funcao>(conf=>
                {
                    conf.Property<string>("PropriedadeSombra")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValueSql("'Teste'");
                });
        }
    }
}