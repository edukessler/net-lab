using Imposto.Core.Data;
using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        public void GerarNotaFiscal(Pedido pedido)
        {
            NotaFiscal notaFiscal = EmitirNotaFiscal(pedido);            
            GerarNotaFiscalXML(notaFiscal);
            GravarNotaFiscal(notaFiscal);
        }

        public void GravarNotaFiscal(NotaFiscal notaFiscal)
        {
            new NotaFiscalRepository().GravarNotaFiscal(notaFiscal);
        }

        public void GerarNotaFiscalXML(NotaFiscal notaFiscal)
        {
            string diretorioXML = ConfigurationManager.AppSettings["DiretorioXML"];
            if (!Directory.Exists(diretorioXML))
                throw new Exception(String.Format("Diretório para geração do XML '{0}' não encontrado!", diretorioXML));

            GerarNotaFiscalXML(notaFiscal, diretorioXML);
        }

        public void GerarNotaFiscalXML(NotaFiscal notaFiscal, string caminho)
        {
            FileStream xml = null;
            try
            {
                caminho = string.Format(@"{0}\NotaFiscal_{1}.xml", caminho, notaFiscal.NumeroNotaFiscal);

                xml = new FileStream(caminho, FileMode.Create);
                new XmlSerializer(typeof(NotaFiscal)).Serialize(xml, notaFiscal);
            }
            finally
            {
                xml.Close();
            }
        }

        public NotaFiscal EmitirNotaFiscal(Pedido pedido)
        {
            NotaFiscal notaFiscal = new NotaFiscal();

            notaFiscal.NumeroNotaFiscal = 99999;
            notaFiscal.Serie = new Random().Next(Int32.MaxValue);
            notaFiscal.NomeCliente = pedido.NomeCliente;

            notaFiscal.EstadoDestino = pedido.EstadoDestino;
            notaFiscal.EstadoOrigem = pedido.EstadoOrigem;

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

                notaFiscalItem.Cfop = new CfopService().ObterCfop(notaFiscal.EstadoOrigem, notaFiscal.EstadoDestino);

                //Cálculo do ICMS
                if (notaFiscal.EstadoDestino == notaFiscal.EstadoOrigem || itemPedido.Brinde)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                }
                else
                {
                    notaFiscalItem.TipoIcms = "10";
                    notaFiscalItem.AliquotaIcms = 0.17;
                }

                if (notaFiscalItem.Cfop == "6.009")
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
                else
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;

                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

                //Cálculo do Ipi
                notaFiscalItem.BaseIpi = itemPedido.ValorItemPedido;
                notaFiscalItem.AliquotaIpi = (itemPedido.Brinde ? 0 : 0.1);
                notaFiscalItem.ValorIpi = notaFiscalItem.BaseIpi * notaFiscalItem.AliquotaIpi;

                //Cálculo do desconto
                notaFiscalItem.Desconto = new DescontoService().CalcularDesconto(notaFiscal.EstadoDestino);

                notaFiscal.ItensDaNotaFiscal.Add(notaFiscalItem);
            }

            return notaFiscal;
        }
    }
}
