using Campeonato;
using Microsoft.Data.SqlClient;
Banco conn = new Banco();

SqlConnection conexaoSql = new(conn.getCaminho());


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
                    Console.WriteLine($"{valorRetornado["Casa"].ToString()} {valorRetornado["GolCasa"].ToString()} X {valorRetornado["GolVisitante"].ToString()} {valorRetornado["Visitante"]}");
                    Console.WriteLine($"Data: {valorRetornado["Data"].ToString()}\n" +
                        $"Total de Gols: {valorRetornado["Total de gols"].ToString()}");
                    Console.WriteLine("~~~~~X~~~X~~~X~~~X~~~~~");
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


ImprimirVencedor();
ImprimirTop5();
ImprimirPartidaComMaisgols();
ImprimirTimeQueMaisGoleou();
ImprimirTimeMaisGoleado();