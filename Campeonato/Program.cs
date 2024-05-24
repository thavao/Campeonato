using Campeonato;
using Microsoft.Data.SqlClient;
Banco conn = new Banco();

SqlConnection conexaoSql = new(conn.getCaminho());

conexaoSql.Open();

SqlCommand cmd = new("[dbo].[Partida_Com_Mais_Gols]", conexaoSql);
cmd.CommandType = System.Data.CommandType.StoredProcedure;

try
{

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

try
{
    cmd = new("[dbo].[Time_Mais_Goleou]", conexaoSql);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    Console.WriteLine("O time que mais fez gol foi: ");
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

try
{
    cmd = new("[dbo].[Time_Mais_Goleado]", conexaoSql);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    Console.WriteLine("O time que mais tomou gol foi: ");
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