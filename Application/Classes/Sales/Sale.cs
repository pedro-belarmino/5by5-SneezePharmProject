using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Classes;

namespace Application
{
    public class Sale
    {
        public int Id { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public decimal ValorTotal { get; private set; }
        public Customer cliente = new Customer();
        
        public Sale(int i, DateOnly d, decimal v, Customer c)
        {
            this.Id = i;
            this.DataVenda = d;
            this.ValorTotal = v;
            this.Customer.Cpf = c;
        }
    }
}
