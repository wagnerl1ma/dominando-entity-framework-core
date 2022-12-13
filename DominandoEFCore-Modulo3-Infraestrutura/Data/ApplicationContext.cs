using DominandoEFCore_Modulo3_Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore_Modulo3_Infraestrutura.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-02;Integrated Security=true;pooling=true;";

            optionsBuilder
                .UseSqlServer(strConnection, o => o.MaxBatchSize(100).CommandTimeout(5).EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null))
                .LogTo(Console.WriteLine, LogLevel.Information) //inserindo log no console
                .EnableSensitiveDataLogging();
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}