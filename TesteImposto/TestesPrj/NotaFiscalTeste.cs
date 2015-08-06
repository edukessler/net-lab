using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imposto.Core.Domain;
using Imposto.Core.Service;

namespace TestesPrj
{
    [TestClass]
    public class NotaFiscalTeste
    {
        private string[] Estados = new string[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

        [TestMethod]
        public void TestarAliquotaIcms()
        {
            //Realiza o teste entre todas as possibilidades de estados Origem e Destino (com item brinde e não)
            //Regra: Estado origem = destino ou item é brinde -> Aliq. ICMS 0.18 caso contrário Aliq. ICMS 0.17
            Pedido pedido = new Pedido();
            pedido.ItensDoPedido.Add(new PedidoItem());

            for (int i = 0; i < 2; i++)
            {                
                pedido.ItensDoPedido[0].Brinde = (i == 0);
                
                foreach (var ufOrigem in Estados)
                {
                    pedido.EstadoOrigem = ufOrigem;

                    foreach (var UfDestino in Estados)
                    {
                        pedido.EstadoDestino = UfDestino;

                        NotaFiscal notaFiscal = new NotaFiscalService().EmitirNotaFiscal(pedido);

                        if (notaFiscal.EstadoDestino == notaFiscal.EstadoOrigem || pedido.ItensDoPedido[0].Brinde)
                            Assert.AreEqual(0.18, notaFiscal.ItensDaNotaFiscal[0].AliquotaIcms, "Alíquota de ICMS inválida!");
                        else
                            Assert.AreEqual(0.17, notaFiscal.ItensDaNotaFiscal[0].AliquotaIcms, "Alíquota de ICMS inválida!");
                    }
                }
                
            }
        }

        [TestMethod]
        public void TestarValorIpi()
        {
            //Realiza testes no valor do ipi
            //Regra: Valor do ipi = base ipi * aliquota ipi [Aliquota do ipi brinde = 0 caso contrário 10%]

            for (int i = 0; i < 2; i++)
            {
                Pedido pedido = new Pedido();
                pedido.ItensDoPedido.Add(new PedidoItem() { ValorItemPedido = 19.75, Brinde = (i == 0) });
                NotaFiscal notaFiscal = new NotaFiscalService().EmitirNotaFiscal(pedido);

                if (pedido.ItensDoPedido[0].Brinde)
                    Assert.AreEqual(0, notaFiscal.ItensDaNotaFiscal[0].ValorIpi, "Valor do IPI divergente para Brindes!");
                else
                {
                    double valor = pedido.ItensDoPedido[0].ValorItemPedido * 0.1;
                    Assert.AreEqual(valor, notaFiscal.ItensDaNotaFiscal[0].ValorIpi, "Valor do IPI divergente!");                
                }
            }
        }
    }
}
