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


//InserirTime("Supernovas Footbol Clube", "Supernovas", new DateOnly(2019, 05, 26));


DateOnly GerarData()
{
    DateOnly data = new DateOnly();
    data = DateOnly.FromDateTime(DateTime.Now);

    int dias = new Random().Next(data.DayNumber - 40, data.DayNumber + 40);

    data = new DateOnly();
    data = data.AddDays(dias);
    return data;
}

void GerarPartidas()
{
    SqlCommand cmd = new SqlCommand();
    cmd.Connection = conexaoSql;
    cmd.CommandText = "SELECT COUNT(*) FROM [Time]";

    int nmrDeTimes;

    conexaoSql.Open();
    using (var retorno = cmd.ExecuteReader())
    {
        retorno.Read();
        nmrDeTimes = (int)retorno[0];
    }
    conexaoSql.Close();

    for (int t1 = 1; t1 <= nmrDeTimes; t1++)
    {
        for (int t2 = 1; t2 <= nmrDeTimes; t2++)
        {
            if (t1 != t2)
            {
                int golT1 = new Random().Next(0, 16);
                int golT2 = new Random().Next(0, 16);
                InserirPartida(t1, t2, GerarData(), golT1, golT2);
            }
        }
    }

}

int InserirPartida(int timeCasa, int timeVisitante, DateOnly data, int golCasa, int golVisitante)
{
    try
    {
        SqlCommand cmd = new("[dbo].Inserir_Partida", conexaoSql);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlParameter tCasa = new SqlParameter("@TimeCasa", SqlDbType.Int);
        SqlParameter tVisitante = new SqlParameter("@TimeVisitante", SqlDbType.Int);
        SqlParameter dataJogo = new SqlParameter("@Data", SqlDbType.Date);
        SqlParameter gCasa = new SqlParameter("@GolCasa", SqlDbType.Int);
        SqlParameter gVisitante = new SqlParameter("@GolVisitante", SqlDbType.Int);

        tCasa.Value = timeCasa;
        tVisitante.Value = timeVisitante;
        dataJogo.Value = data;
        gCasa.Value = golCasa;
        gVisitante.Value = golVisitante;

        cmd.Parameters.Add(tCasa);
        cmd.Parameters.Add(tVisitante);
        cmd.Parameters.Add(dataJogo);
        cmd.Parameters.Add(gCasa);
        cmd.Parameters.Add(gVisitante);

        conexaoSql.Open();
        return cmd.ExecuteNonQuery();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return 0;
    }
    finally
    {
        conexaoSql.Close();
    }
}

void ImprimirPartida(SqlDataReader partida)
{
    Console.WriteLine($"{partida["Casa"].ToString()} {partida["GolCasa"].ToString()} X {partida["GolVisitante"].ToString()} {partida["Visitante"]}");
    Console.WriteLine($"Data: {partida["Data"].ToString()}\n" +
        $"Total de Gols: {partida["Total de gols"].ToString()}");
    Console.WriteLine("~~~~~X~~~X~~~X~~~X~~~~~");
}


GerarPartidas();

Console.ReadLine();

ImprimirVencedor();

Console.ReadLine();
ImprimirTop5();

Console.ReadLine();
ImprimirPartidaComMaisgols();

Console.ReadLine();
ImprimirTimeQueMaisGoleou();

Console.ReadLine();

ImprimirTimeMaisGoleado();
Console.ReadLine();
