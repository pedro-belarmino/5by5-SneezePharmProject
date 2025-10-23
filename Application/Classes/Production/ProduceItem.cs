using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utils.WritersAndReaders;

namespace Application.Prod
{
    public class ProduceItem
    {
        public List<string> lista = new List<string>();

        Writer_Reader objeto = new Writer_Reader();

        public int IdProducao { get; set; }
        public string? Principio { get; set; }
        public int QuantidadePrincipio { get; set; }

        string file = "ProduceItem.data";

        public ProduceItem() 
        {
            string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
            string fullPath = Path.Combine(diretorio, file);

            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
        }

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
