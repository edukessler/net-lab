using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Service
{
    public class CfopService
    {
        /// <summary>
        /// Retorna CFOP de acordo com EstadoOrigem e Estado Destino
        /// </summary>
        /// <param name="EstadoOrigem"></param>
        /// <param name="EstadoDestino"></param>
        /// <returns></returns>
        public string ObterCfop(string EstadoOrigem, string EstadoDestino)
        {
            switch (EstadoOrigem)
            {
                case "SP":
                    return ObterCfopOrigemSP(EstadoDestino);
                case "MG":
                    return ObterCfopOrigemMG(EstadoDestino);
                default:
                    return "";
            }         
        }

        private string ObterCfopOrigemSP(string EstadoDestino)
        {
            //Origem SP
            switch (EstadoDestino)
            {
                case "RJ":
                    return "6.000";
                case "PE":
                    return "6.001";
                case "MG":
                    return "6.002";
                case "PB":
                    return "6.003";
                case "PR":
                    return "6.004";
                case "PI":
                    return "6.005";
                case "RO":
                    return "6.006";
                case "TO":
                    return "6.008";
                case "SE":
                    return "6.009";
                case "PA":
                    return "6.010";
                default:
                    return "";
            }
        }

        private string ObterCfopOrigemMG(string EstadoDestino)
        {
            //Origem MG
            switch (EstadoDestino)
            {
                case "RJ":
                    return "6.000";
                case "PE":
                    return "6.001";
                case "MG":
                    return "6.002";
                case "PB":
                    return "6.003";
                case "PR":
                    return "6.004";
                case "PI":
                    return "6.005";
                case "RO":
                    return "6.006";
                case "TO":
                    return "6.008";
                case "SE":
                    return "6.009";
                case "PA":
                    return "6.010";
                default:
                    return "";
            }
        }
    }
}
