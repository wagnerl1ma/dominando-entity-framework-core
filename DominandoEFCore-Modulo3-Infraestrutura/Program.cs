﻿#nullable enable
using DominandoEFCore_Modulo3_Infraestrutura.Data;
using DominandoEFCore_Modulo3_Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            TempoComandoGeral();
            //HabilitandoBatchSize();
            //DadosSensiveis();
            //ConsultarDepartamentos();
        }

        static void ExecutarEstrategiaResiliencia()
        {
            using var db = new ApplicationContext();

            var strategy = db.Database.CreateExecutionStrategy(); // executando transacao de forma mais segura // essencial quando se usa EnableRetryOnFailure no context, para nao duplicar um dado em caso de falha 
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Departamento { Descricao = "Departamento Transacao" });
                db.SaveChanges();

                transaction.Commit();
            });

        }


        static void TempoComandoGeral()
        {
            using var db = new ApplicationContext();

            db.Database.SetCommandTimeout(10); //Configurando o Timeout do comando para um fluxo especifico ou necessidade

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07';SELECT 1"); // Configurando tempo do Timeout para 7 segundos
        }

        static void HabilitandoBatchSize()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 50; i++)
            {
                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento " + i
                    });
            }

            db.SaveChanges();
        }

        static void DadosSensiveis()
        {
            using var db = new ApplicationContext();

            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(p => p.Descricao == descricao).ToArray();
        }

        static void ConsultarDepartamentos()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToArray();
        }
    }
}
