using Campeonato;
using Microsoft.Data.SqlClient;
using System.Data;
Banco conn = new Banco();

SqlConnection conexaoSql = new(conn.getCaminho());

int opcao;

do
{
    Console.Clear();
    Console.WriteLine("SELECIONE UMA OPÇÃO");
    Console.WriteLine("Ver");
    Console.WriteLine("1 - Campeão do campeonato");
    Console.WriteLine("2 - Classificação dos times");
    Console.WriteLine("3 - Time que fez o maior numero de gols no campeonato");
    Console.WriteLine("4 - Time que sofreu o maior numero de gols no campeonato");
    Console.WriteLine("5 - A partida com mais gols no campeonato");
    Console.WriteLine("6 - Maior numero de gols que cada time fez em um jogo");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");
    opcao = int.Parse(Console.ReadLine());

    switch (opcao)
    {
        case 0:
            Console.WriteLine("Saindo...");
            break;
        case 1:
            ImprimirVencedor();
            break;
        case 2:
            ImprimirTop5();
            break;
        case 3:
            ImprimirTimeQueMaisGoleou();
            break;
        case 4:
            ImprimirTimeMaisGoleado();
            break;
        case 5:
            ImprimirPartidaComMaisgols();
            break;
        case 6:
            ImprimirMaiorQtdGolPartida();
            break;
        default:
            Console.WriteLine("Opção inválida");
            break;
    }
    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
} while (opcao != 0);

void ImprimirMaiorQtdGolPartida()
{
    try
    {
        SqlCommand cmd = new("SELECT Nome, MaxGolPartida FROM [Time]", conexaoSql);
        conexaoSql.Open();

        using (SqlDataReader reader = cmd.ExecuteReader())
        {

            Console.WriteLine("Maior numero de gols que cada time fez em um jogo");
            while (reader.Read())
            {
                Console.WriteLine($"Time: {reader["Nome"]} | Gols: {reader["MaxGolPartida"]}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}
void ImprimirPartidaComMaisgols()
{
    try
    {

        conexaoSql.Open();

        SqlCommand cmd = new("[dbo].[Partida_Com_Mais_Gols]", conexaoSql);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        using (SqlDataReader valorRetornado = cmd.ExecuteReader())
        {

            if (valorRetornado.HasRows)
            {

                Console.WriteLine("Partida com mais gols");
                while (valorRetornado.Read())
                {
                    ImprimirPartida(valorRetornado);
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}
void ImprimirTimeQueMaisGoleou()
{
    try
    {

        SqlCommand cmd = new("[dbo].[Time_Mais_Goleou]", conexaoSql);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        Console.WriteLine("O time que mais fez gol foi: ");
        conexaoSql.Open();
        using (SqlDataReader ValorRetornado = cmd.ExecuteReader())
        {
            while (ValorRetornado.Read())
                Console.WriteLine($"O time {ValorRetornado["Nome"].ToString()} fez {ValorRetornado["TotalGol"]} gols no campeonato");
        }



    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}

void ImprimirTimeMaisGoleado()
{
    try
    {
        SqlCommand cmd = new("[dbo].[Time_Mais_Goleado]", conexaoSql);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        Console.WriteLine("O time que mais tomou gol foi: ");
        conexaoSql.Open();
        using (SqlDataReader ValorRetornado = cmd.ExecuteReader())
        {
            while (ValorRetornado.Read())
                Console.WriteLine($"O time {ValorRetornado["Nome"].ToString()} tomou {ValorRetornado["GolRecebido"]} gols no campeonato");
        }



    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}

void ImprimirVencedor()
{
    try
    {
        SqlCommand cmd = new();
        cmd.Connection = conexaoSql;
        cmd.CommandText = "SELECT TOP 1 Nome, Apelido, Pontuacao, MaxGolPartida, TotalGol, GolRecebido" +
            " FROM Time" +
            " ORDER BY Pontuacao DESC, TotalGol DESC;";
        conexaoSql.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            reader.Read();
            Console.WriteLine("O time vencedor foi " + reader["Apelido"].ToString() + "!");
            ImprimirTime(reader);

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}

void ImprimirTop5()
{
    try
    {
        SqlCommand cmd = new();
        cmd.Connection = conexaoSql;
        cmd.CommandText = "SELECT TOP 5 Nome, Apelido, Pontuacao, MaxGolPartida, TotalGol, GolRecebido" +
            " FROM Time" +
            " ORDER BY Pontuacao DESC, TotalGol DESC;";
        conexaoSql.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            int i = 1;
            while (reader.Read())
            {
                Console.WriteLine("X~~~~X~~~~X~~~~X~~~~X~~~~X~~~~X~~~~X~~~~X");
                Console.WriteLine($"{i}º Lugar: " + reader["Apelido"].ToString());
                ImprimirTime(reader);
                Console.WriteLine();
                i++;
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        conexaoSql.Close();
    }
}

void ImprimirTime(SqlDataReader time)
{
    Console.WriteLine($"Nome: {time["Nome"].ToString()} | Apelido {time["Apelido"].ToString()}");
    Console.WriteLine($"Pontuacao: {time["Pontuacao"].ToString()} | Maior numero de gols em uma partida: {time["MaxGolPartida"].ToString()}");
    Console.WriteLine($"Gols feitos: {time["TotalGol"].ToString()} | Gols tomados: {time["GolRecebido"].ToString()}");
}

void ImprimirPartida(SqlDataReader partida)
{
    Console.WriteLine($"{partida["Casa"].ToString()} {partida["GolCasa"].ToString()} X {partida["GolVisitante"].ToString()} {partida["Visitante"]}");
    Console.WriteLine($"Data: {partida["Data"].ToString()}\n" +
        $"Total de Gols: {partida["Total de gols"].ToString()}");
    Console.WriteLine("~~~~~X~~~X~~~X~~~X~~~~~");
}
