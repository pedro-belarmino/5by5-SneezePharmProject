using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Compra
{
    public class Purchases
    {
        public int Id { get; set; }    // 5 dígitos
        public DateOnly DataCompra { get; set; }
        public string RecebeFornecedor { get; set; }
        public decimal ValorTotal { get; set; }

        // Construtor
        public Purchases(int id, DateOnly dataCompra, string recebeFornecedor, decimal valorTotal)
        {
            // Validação para o Id
            if (id < 10000 || id > 99999)
                Console.WriteLine("Id deve ter 5 dígitos");

            Id = id;
            DataCompra = dataCompra;
            RecebeFornecedor = recebeFornecedor;
            ValorTotal = valorTotal;
        }
    }

}
