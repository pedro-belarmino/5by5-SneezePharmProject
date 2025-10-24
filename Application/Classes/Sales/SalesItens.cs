using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SalesIten
    {
        public int Id { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public double TotalItem { get; private set; }
        public int CDB { get; private set; }

        public SalesIten(int i, int q, decimal v, double t, int cdb)
        {
            this.Id = i;
            this.Quantidade = q;
            this.ValorUnitario = v;
            this.TotalItem = t;
            this.CDB = cdb;
        }
    }
}
