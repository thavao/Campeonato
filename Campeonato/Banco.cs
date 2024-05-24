using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato
{
    internal class Banco
    {
        readonly string strConexao = "";

        public Banco()
        {
            strConexao += "Data Source=127.0.0.1;";
            strConexao += "Initial Catalog=Campeonato;";
            strConexao += "User Id=sa; Password=SqlServer2019!;";
            strConexao += "TrustServerCertificate=Yes;";
        }
        public string getCaminho()
        {
            return strConexao;
        }
    }
}
