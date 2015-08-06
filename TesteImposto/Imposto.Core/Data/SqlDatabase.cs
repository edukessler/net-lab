using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public static class SqlDatabase
    {
        public static SqlConnection Conectar()
        {
            try
            {
                string cnnStr = ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

                SqlConnection cnn = new SqlConnection(cnnStr);
                cnn.Open();

                return cnn;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro de conexão no banco de dados: " + ex.Message);
            }
        }

        public static void Desconectar(SqlConnection cnn)
        {
            try
            {
                cnn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desconectar do banco de dados: " + ex.Message);
            }
        }
    }
}
