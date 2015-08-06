using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Service
{
    public class DescontoService
    {
        public double CalcularDesconto(string EstadoDestino)
        {
            //Sudeste possui 10% de desconto            
            if ((new string[] { "SP", "ES", "MG", "RJ" }).Contains(EstadoDestino))
                return 0.10;
            else
                return 0;
        }
    }
}
