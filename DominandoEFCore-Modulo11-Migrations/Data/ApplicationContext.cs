using System;
using DominandoEFCore_Modulo11_Migrations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore_Modulo11_Migrations.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=DevIO-03;Integrated Security=true;";

            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf =>
            {
                conf.HasKey(p => p.Id);
                conf.Property(p => p.Nome).HasMaxLength(60).IsUnicode(false);
            });
        }
    }
}