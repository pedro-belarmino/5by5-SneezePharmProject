using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utils.WritersAndReaders;

namespace Application
{
    public class SaleItens
    {
        Writer_Reader objeto = new();

        public List<SaleItens> SaleItensList = new();
        public string Id { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int TotalItem { get; private set; }
        public string CDBMedicamento { get; private set; }
        private int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "SaleItens.data";

        string fullPath = Path.Combine(diretorio, file);

        public SaleItens()
        {
            objeto.Verificador(diretorio, fullPath);

        }

        public SaleItens(string i, int q, decimal v, string m, int t)
        {
            this.Id = i;
            this.Quantidade = q;
            this.ValorUnitario = v;
            this.TotalItem = t;
            this.CDBMedicamento = m;
        }





    }
}
