using Application.Classes.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Compra
{
    public class PurchaseItem
    {
        public int Id { get; set; }


        //deve estar ativo
        public string IdIngrediente { get; set; }   //recebe do ingrediente
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal TotalItem { get; set; }



        public PurchaseItem(int id, string idIngrediente, int quantidade, decimal valorUnitario, decimal totalItem)
        {

            if (quantidade <= 0 || quantidade > 9999)
                throw new ArgumentException("Quantidade deve ser entre 1 e 9999."); //impedem a criação do objeto com dados errados.

            if (valorUnitario <= 0 || valorUnitario > 999.99m) //**
                throw new ArgumentException("Valor unitário inválido.");

            Id = id;
            IdIngrediente = idIngrediente;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            TotalItem = quantidade * valorUnitario;
        }
    }
}
