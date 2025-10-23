using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Sales
    {
        public int Id { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public decimal ValorTotal { get; private set; }
        public Cliente cliente = new Cliente ();
        
        public Sales(int i, DateOnly d, decimal v, Cliente c)
        {
            this.Id = i;
            this.DataVenda = d;
            this.ValorTotal = v;
            this.Cliente.Cpf = c;
        }


    }
}
