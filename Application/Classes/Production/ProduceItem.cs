using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Classes;
using Application.Utils.WritersAndReaders;

namespace Application.Classes.Production
{
    public class ProduceItem
    {
        Writer_Reader objeto = new Writer_Reader();

        public List<ProduceItem> ProduceItems = new List<ProduceItem>();

        public string? IdProducao { get; private set; }
        public string? idPrincipio { get; private set; }
        public int QuantidadePrincipio { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "ProduceItem.data";
        string fullPath = Path.Combine(diretorio, file);

        public ProduceItem()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine($"Arquivo {file} e diretório criados com sucesso.");
        }

        public void PopularListaProduceItem()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string id = line.Substring(0, 5).Trim();

            }
        }
    }
}
