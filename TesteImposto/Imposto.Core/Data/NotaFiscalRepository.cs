using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository
    {
        public void GravarNotaFiscal(NotaFiscal notaFiscal)
        {
            SqlConnection cnn = null;

            try
            {
                cnn = SqlDatabase.Conectar();

                //Inserção do cabeçalho
                SqlCommand cmd = new SqlCommand("P_NOTA_FISCAL", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                //Configurando parâmetro pId - Entrada e saída
                SqlParameter pId = new SqlParameter("@pId", SqlDbType.Int);
                pId.Direction = ParameterDirection.InputOutput;
                pId.Value = notaFiscal.Id;
                cmd.Parameters.Add(pId);

                //Configurando demais parâmetros
                cmd.Parameters.Add("@pNumeroNotaFiscal", SqlDbType.Int).Value = notaFiscal.NumeroNotaFiscal;
                cmd.Parameters.Add("@pSerie", SqlDbType.Int).Value = notaFiscal.Serie;
                cmd.Parameters.Add("@pNomeCliente", SqlDbType.VarChar, 50).Value = notaFiscal.NomeCliente;
                cmd.Parameters.Add("@pEstadoDestino", SqlDbType.VarChar, 50).Value = notaFiscal.EstadoDestino;
                cmd.Parameters.Add("@pEstadoOrigem", SqlDbType.VarChar, 50).Value = notaFiscal.EstadoOrigem;
                cmd.ExecuteNonQuery();
               
                int idNotaFiscal = Convert.ToInt32(pId.Value);

                //Inserção dos itens
                cmd.CommandText = "P_NOTA_FISCAL_ITEM";
                foreach (NotaFiscalItem item in notaFiscal.ItensDaNotaFiscal)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@pId", SqlDbType.Int).Value = item.Id;
                    cmd.Parameters.Add("@pIdNotaFiscal", SqlDbType.Int).Value = idNotaFiscal;
                    cmd.Parameters.Add("@pCfop", SqlDbType.VarChar, 5).Value = item.Cfop;
                    cmd.Parameters.Add("@pTipoIcms", SqlDbType.VarChar, 20).Value = item.TipoIcms;
                    cmd.Parameters.Add("@pBaseIcms", SqlDbType.Decimal).Value = item.BaseIcms;
                    cmd.Parameters.Add("@pAliquotaIcms", SqlDbType.Decimal).Value = item.AliquotaIcms;
                    cmd.Parameters.Add("@pValorIcms", SqlDbType.Decimal).Value = item.ValorIcms;
                    cmd.Parameters.Add("@pBaseIpi", SqlDbType.Decimal).Value = item.BaseIpi;
                    cmd.Parameters.Add("@pAliquotaIpi", SqlDbType.Decimal).Value = item.AliquotaIpi;
                    cmd.Parameters.Add("@pValorIpi", SqlDbType.Decimal).Value = item.ValorIpi;
                    cmd.Parameters.Add("@pDesconto", SqlDbType.Decimal).Value = item.Desconto;
                    cmd.Parameters.Add("@pNomeProduto", SqlDbType.VarChar, 50).Value = item.NomeProduto;
                    cmd.Parameters.Add("@pCodigoProduto", SqlDbType.VarChar, 20).Value = item.CodigoProduto;
                    cmd.ExecuteNonQuery();                 
                }
            }            
            finally
            {
                SqlDatabase.Desconectar(cnn);
            }
        }
    }
}
