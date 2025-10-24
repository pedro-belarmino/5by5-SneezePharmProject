using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SaleItens
    {
        public int Id { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario  { get; private set; }
        public int TotalItem { get; private set; }
        //public Medicine medicamento = new Medicine();

        public SaleItens(int i, int q, decimal v
            //, Medicine m
            )
        {
            this.Id = i;
            this.Quantidade = q;
            this.ValorUnitario = v; 
            //this.TotalItem = m;
            //this.Medicine.CDB;
        }
    }
}
