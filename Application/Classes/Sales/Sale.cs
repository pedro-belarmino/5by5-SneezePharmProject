using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Application.Utils;


namespace Application.Classes.Sales
{
    public class Sale
    {
        public int Id { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public decimal ValorTotal { get; private set; }

        public string CpfCliente { get; private set; }

        public Customer cliente;

        public List<string> saleList { get; private set; }

        CreateId id = new();

        CreateId generateId = new();
        public Sale(int i, DateOnly d, decimal v, Customer c)
        {

            string newId = generateId.Create(saleList!);

            Id = int.Parse(newId);
            DataVenda = d;
            ValorTotal = v;
            CpfCliente = cliente?.Cpf!;
        }
        public Sale()
        {
        }

        public void CreateSale()
        {
            Sale s = new Sale();
            var newId = id.Create;

            string ID = generateId.Create(saleList!);
            s.Id = int.Parse(ID);

            System.Console.WriteLine("valor total");
            s.ValorTotal = int.Parse(Console.ReadLine()!);

            System.Console.WriteLine("cliente cpf");
            s.CpfCliente = Console.ReadLine()!;
        }
        public void ReadSale()
        {
            foreach (var s in saleList)
            {
                Console.WriteLine(saleList.ToString());
            }
        }


        public List<string> DeleteSale()
        {
            System.Console.WriteLine("id");
        }
    }
}
