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
        public List<PurchaseItem> PurchaseItems { get; private set; }

        public Metodo()
        {
            PurchaseItems = new List<PurchaseItem>();
        }

        public bool AdicionarItem(PurchaseItem item)
        {
            if (PurchaseItems.Count >= 3)       //verificando até 3 itens no máx
            {
                Console.WriteLine("Limite de 3 itens por compra atingido!");
                return false;
            }

            PurchaseItems.Add(item);
            Console.WriteLine("Item adicionado com sucesso!");
            return true;
        }

        public PurchaseItem BuscarItem(int id)
        {
            return PurchaseItems.Find(i => i.Id == id); //busca o primeiro elemento da lista que satisfaça a condição
        }

        public void ListarItens()
        {
            if (PurchaseItems.Count == 0)
            {
                Console.WriteLine("Nenhum item adicionado.");
                return;
            }

            Console.WriteLine("\n Itens: ");
            foreach (var item in PurchaseItems)
            {
                Console.WriteLine($"ID: {item.Id} Qtd: {item.Quantidade} Valor Unit: {item.ValorUnitario} Total: {item.TotalItem}");
            }
        }

        public void AlterarItem(int id, int novaQuantidade, decimal novoValor)
        {
            var item = PurchaseItems.FirstOrDefault(i => i.Id == id);
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
            var item = PurchaseItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Item não encontrado!");
                return;
            }

            PurchaseItems.Remove(item);
            Console.WriteLine($" Item {id} removido!");
        }

        //Calcular total geral dos itens
        public decimal CalcularTotalGeral()
        {
            return PurchaseItems.Sum(i => i.TotalItem);
        }
    }

}
