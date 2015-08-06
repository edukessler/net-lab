using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imposto.Core.Domain;
using Imposto.Core.Service;

namespace TestesPrj
{
    [TestClass]
    public class DescontoTeste
    {
        [TestMethod]
        public void TestarDesconto()
        {
            //Realiza o teste com todos os estados.
            //REGRA: Estados do Sudeste devem possuir desconto de 10%
        
            string[] Sudeste = new string[] { "SP", "ES", "MG", "RJ" };
            string[] Estados = new string[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

            foreach (string Uf in Estados)
            {
                double Desconto = new DescontoService().CalcularDesconto(Uf);                
                
                if (Sudeste.Contains(Uf))
                    Assert.AreEqual(0.10, Desconto, String.Format("Erro ao calcular desconto SUDESTE - Uf:{0} Desconto:{1}",Uf,Desconto));
                else
                    Assert.AreEqual(0, Desconto, String.Format("Erro ao calcular desconto OUTRAS REGIÕES - Uf:{0} Desconto:{1}", Uf, Desconto));
            }

        }
    }
}
