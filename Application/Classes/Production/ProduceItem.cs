using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Prod
{
    public class ProduceItem
    {
        public int IdProducao { get; set; }
        public string Principio { get; set; }
        public int QuantidadePrincipio { get; set; }

        public ProduceItem(int idProducao, string principio, int quantidadePrincipio)
        {
            IdProducao = idProducao;
            Principio = principio;
            if (QuantidadePrincipio < 10000) 
                QuantidadePrincipio = quantidadePrincipio;
            else
                Console.WriteLine("O limite de princípios armazenados é 10000");
        }
    }
}
