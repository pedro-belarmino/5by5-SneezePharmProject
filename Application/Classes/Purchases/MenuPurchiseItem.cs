using Application.Compra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Application.Classes.Purchases
{
    public class MenuPurchaseItem
    {
        Metodo metodo = new Metodo();
        //private const string ItensPath = "Items.data";
        int opcao;

        // Método público para iniciar o loop do menu (substitui o uso de Main)
        public void Run()

        {
            while (true)
            {
                Console.WriteLine("\n MENU DE OPERAÇÕES ");
                Console.WriteLine("1 - Adicionar novo item");
                Console.WriteLine("2 - Exibir todos os itens");
                Console.WriteLine("3 - Alterar item ");
                Console.WriteLine("4 - Remover item");
                Console.WriteLine("5 - Buscar item ");
                Console.WriteLine("6 - Registrar compra (verifica fornecedor)");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");
                int opcao = Console.ReadLine();

                switch (opcao)
                {
                    case 1:
                        Console.Write("Digite o nome: ");
                        string nome = Console.ReadLine();
                        PurchaseItem purchaseItems = new PurchaseItem(nome);
                        metodo.AdicionarItem(purchaseItems);
                        break;

                    case 2:
                        metodo.ListarItems();
                        break;
                    case 3:
                        AlterarItem();
                        break;
                    case 4:
                        Console.Write("Digite o nome a ser removido: ");
                        string nome = Console.ReadLine() ?? "";
                        metodo.RemoverItem(purchaseItems);
                        break;
                    case 5:
                        BuscarItem();
                        break;
                    case 6:
                        RegistrarCompra();
                        break;
                    case 0:
                        SalvarItens();
                        Console.WriteLine("Dados salvos. Saindo");
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;

                }
            }
        }

        public void Adicionar()
        {
            Console.Write("Nome do item: ");
            int nome = int.Parse(Console.ReadLine());



        }
    }
}



