using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DominandoEFCore_Modulo6_Functions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore_Modulo6_Functions.Data
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
                .EnableSensitiveDataLogging();
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