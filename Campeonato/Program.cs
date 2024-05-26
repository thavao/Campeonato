using Campeonato;
using Microsoft.Data.SqlClient;
using System.Data;
Banco conn = new Banco();

SqlConnection conexaoSql = new(conn.getCaminho());


void InserirTime(string nome, string apelido, DateOnly dataCriacao)
{
    try
    {
        conexaoSql.Open();

        SqlCommand cmd = new("[dbo].[Inserir_Time]", conexaoSql);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlParameter n = new SqlParameter("@Nome", SqlDbType.VarChar);
        SqlParameter a = new SqlParameter("@Apelido", SqlDbType.VarChar);
        SqlParameter d = new SqlParameter("@DataCriacao", SqlDbType.Date);

        n.Value = nome;
        a.Value = apelido;
        d.Value = dataCriacao;

        cmd.Parameters.Add(n);
        cmd.Parameters.Add(a);
        cmd.Parameters.Add(d);


        if (cmd.ExecuteNonQuery() == 1)
            Console.WriteLine("Time inserido com sucesso");

        else
            Console.WriteLine("Não foi possível inserir o time...");

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


//InserirTime("Supernovas Footbol Clube", "Supernovas", new DateOnly(2019, 05, 26));
ImprimirVencedor();
ImprimirTop5();
ImprimirPartidaComMaisgols();
ImprimirTimeQueMaisGoleou();
ImprimirTimeMaisGoleado();