using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Compra
{
    public class PurchasesItem
    {
        public int Id { get; set; }
        public string IdIngrediente { get; set; }   //recebe do ingrediente
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal TotalItem { get; set; }



        public PurchasesItem(int id, int idIngrediente, int quantidade, decimal valorUnitario, decimal totalItem)
        {

        }
    }
}
