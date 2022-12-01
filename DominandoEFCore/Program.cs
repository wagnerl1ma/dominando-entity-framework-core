﻿// See https://aka.ms/new-console-template for more information

using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

#region Main


Console.WriteLine("Hello, World!");


//SqlInjection();

////warmup
//new ApplicationContext().Departamentos.AsNoTracking().Any();
//GerenciarEstadoDaConexao(false);
//GerenciarEstadoDaConexao(true);

//HealthCheckBancoDeDado();
//EnsureCreatedAndDeleted();
//GapDoEnsureCreated();

#endregion


static void SqlInjection()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted(); //deletando banco
    db.Database.EnsureCreated(); //criando banco

    //inserir departamento
    db.Departamentos.AddRange(
        new Departamento
        {
            Descricao = "Departamento 01"
        },
        new Departamento
        {
            Descricao = "Departamento 02"
        });
    db.SaveChanges();

    var descricao = "Teste ' or 1='1"; //ataque que altera todos os dados.
    //nunca aceitar parametros de fora para executar comandos SQL, pode ter um ataque.
    db.Database.ExecuteSqlRaw("update departamentos set descricao='AtaqueSqlInjection' where descricao={0}", descricao);
    db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");

    foreach (var departamento in db.Departamentos.AsNoTracking())
    {
        Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
    }
}

static void ExecuteSQL() // Opcoes de executar comandos SQL
{
    using var db = new ApplicationContext();

    // Primeira Opcao
    using (var cmd = db.Database.GetDbConnection().CreateCommand())
    {
        cmd.CommandText = "SELECT 1";
        cmd.ExecuteNonQuery();
    }

    // Segunda Opcao
    var descricao = "TESTE";
    db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id=1", descricao);

    //Terceira Opcao
    db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id=1");
}

static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
{
    int _count = 0;

    using var db = new ApplicationContext();
    var time = System.Diagnostics.Stopwatch.StartNew();

    var conexao = db.Database.GetDbConnection();

    conexao.StateChange += (_, __) => ++_count;

    if (gerenciarEstadoConexao)
    {
        conexao.Open();
    }

    for (var i = 0; i < 200; i++)
    {
        db.Departamentos.AsNoTracking().Any();
    }

    time.Stop();
    var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";

    Console.WriteLine(mensagem);
}

static void HealthCheckBancoDeDado()
{
    using var db = new ApplicationContext();

    // opcao 3
    // novo método com o entity framework core, mais viável
    // validando se posso me conectar ao banco
    var canConect = db.Database.CanConnect();

    if (canConect)
        Console.WriteLine("Posso me conectar");

    else
        Console.WriteLine($"Não oosso me conectar");

    // modo antigo
    //try
    //{
    //    // opcao 1
    //    var connection = db.Database.GetDbConnection();
    //    connection.Open();

    //    // opcao 2
    //    db.Departamentos.Any();

    //    

    //    Console.WriteLine("Posso me conectar");
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine($"Não oosso me conectar {ex.Message}");
    //}
}

static void EnsureCreatedAndDeleted()
{
    using var db = new ApplicationContext();
    //db.Database.EnsureCreated(); // cria o banco de dados com os dbsets e string de conexao configurados caso nao exitir
    db.Database.EnsureDeleted(); // exclui o banco de dados com os dbsets e string de conexao configurados casos existir
}

static void GapDoEnsureCreated()
{
    //método para excecutar a criacao de bancos de dados em contextos diferentes
    using var db1 = new ApplicationContext();
    using var db2 = new ApplicationContextCidade();

    db1.Database.EnsureCreated();
    db2.Database.EnsureCreated();

    var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
    databaseCreator.CreateTables();
}