using Application.Compra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Purchases
{
    public class Metodo
    {
        //Lista de itens
        public List<PurchaseItem> purchaseItems { get; private set; }

        public Metodo()
        {
            purchaseItems = new List<PurchaseItem>();
        }

        public void AdicionarItem(PurchaseItem purchaseItems)
        {
            if (purchaseItems.Exists(i => i.id == PurchaseItem.id))
            {
                Console.WriteLine("Item ja cadastrado");

                if (purchaseItems.Count >= 3)       //verificando até 3 itens no máx
                {
                    Console.WriteLine("Limite de 3 itens por compra atingido!");
                }
                purchaseItems.Add(item);
                Console.WriteLine("Item adicionado com sucesso!");
            }
        }

        public PurchaseItem BuscarItem(int id)
        {
            return purchaseItems.Find(i => i.Id == id); //busca o primeiro elemento da lista que satisfaça a condição
        }

        public void ListarItems()
        {
            if (purchaseItems.Count == 0)
            {
                Console.WriteLine("Nenhum item adicionado.");
                return;
            }

            Console.WriteLine("\n Itens: ");
            foreach (var item in purchaseItems)
            {
                Console.WriteLine($"ID: {item.Id} Qtd: {item.Quantidade} Valor Unit: {item.ValorUnitario} Total: {item.TotalItem}");
            }
        }

        public void AlterarItem(int id, int novaQuantidade, decimal novoValor)
        {
            var item = purchaseItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Item não encontrado!");
                return;
            }

            item.Quantidade = novaQuantidade;
            item.ValorUnitario = novoValor;

            Console.WriteLine("Item alterado com sucesso!");
        }

        public void RemoverItem(int id)
        {
            var item = purchaseItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Item não encontrado!");
                return;
            }

            purchaseItems.Remove(item);
            Console.WriteLine($" Item {id} removido!");
        }

        //Calcular total geral dos itens
        public decimal CalcularTotalGeral()
        {
            return purchaseItems.Sum(i => i.TotalItem);
        }
    }

}
