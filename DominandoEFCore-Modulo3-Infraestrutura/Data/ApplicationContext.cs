using DominandoEFCore_Modulo3_Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore_Modulo3_Infraestrutura.Data
{
    public class ApplicationContext : DbContext
    {
        //static string? caminho = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        static string? caminho = "C:\\Users\\Wagner\\Desktop\\DESENVOLVEDOR.IO\\ENTITY FRAMEWORK CORE\\Dominando o Entity Framework Core\\Projeto\\DominandoEFCore\\DominandoEFCore-Modulo3-Infraestrutura";
        private readonly StreamWriter _writer = new StreamWriter($"{caminho}\\meu_log_do_ef_core.txt", append: true); // append: acrescenta todo vez mais informa�oes no mesmo arquivo
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-02;Integrated Security=true;pooling=true;";

            optionsBuilder
                .UseSqlServer(strConnection, o => o.MaxBatchSize(100) //aumentando o tamanho do lote para gravacao no banco de dados, por padrao o sql server aceita 42 registros
                .CommandTimeout(5) // Configurando tempo do Timeout do comando global
                .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null))  // Ativar nova tentativa em caso de falha - Por padrao ele tenta 6x a durant 30segundos at� falhar
                .LogTo(Console.WriteLine, LogLevel.Information) //inserindo log no console
                .EnableSensitiveDataLogging();  // logs sensives no console -  � uma boa pr�tica no modo debug
                 //.LogTo(_writer.WriteLine, LogLevel.Information); //Gravando logs em um arquivo de texto
                /*.LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted},
                                          LogLevel.Information, DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine) //filtrando eventos do log
                .EnableSensitiveDataLogging();*/
                //.EnableDetailedErrors(); //encontrando erros com mais detalhes - esse m�todo deixa o carregamento mais lento, entao � aconselhavel somenete em modo debug
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}