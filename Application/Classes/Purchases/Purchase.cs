using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Compra
{
    public class Purchase
    {
        public int Id { get; set; }    // 5 dígitos
        public DateOnly DataCompra { get; set; }       //ira atualizar ultima compra do fornecedor

        //deve existir/ nao pode estar bloqueado
        public string CnpjFornecedor { get; set; } //recebe do fornecedor
        public decimal ValorTotal { get; set; }



        // Construtor
        public Purchase(int id, DateOnly dataCompra, string recebeFornecedor, decimal valorTotal)
        {
            // Validação para o Id
            if (id < 10000 || id > 99999)
                throw new ArgumentException("Id deve ter 5 dígitos");

            Id = id;
            DataCompra = dataCompra;
            CnpjFornecedor = recebeFornecedor;
           // ValorTotal = PurshaseItem.TotalItem;
        }
    }

}
