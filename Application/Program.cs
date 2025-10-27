using Application;
using Application.Classes;
using Application.Classes.Medicamento;
using Application.Classes.Production;
using Application.Classes.Sales;
<<<<<<< HEAD
=======
//using Application.Compra;
>>>>>>> 1a4ab92223914065bcebf091876c1b4ea84a6891

internal class Program
{
    private static void Main(string[] args)
    {
        Ingredient ingredient = new();
        Medicine medicine = new();
        Produce produce = new();
        ProduceItem produceItem = new();
        Sale venda = new();
        SaleItens saleItens = new();
        Customer customer = new();
        Supplier supplier = new();
<<<<<<< HEAD
=======
        //Purchase purchase = new();
        //PurchaseItem purchaseItem = new();


>>>>>>> 1a4ab92223914065bcebf091876c1b4ea84a6891

        void Menu()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Opções de Cliente"); //finalizado
                Console.WriteLine("2 - Opções de Princípios ativos: "); //feito
                Console.WriteLine("3 - Opções de medicamento: ");// feito
                Console.WriteLine("4 - Opções de Producao");// feito
                Console.WriteLine("5 - Opções de Item de Producao");
                Console.WriteLine("6 - Opções de Venda");// finalizado
                Console.WriteLine("7 - Opções de Item Venda");// 
                Console.WriteLine("8 - Opções de Fornecedor");// 
                Console.WriteLine("0 - Sair");


                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
<<<<<<< HEAD
=======
                        //purchaseItem.PurchaseItemMenu();
>>>>>>> 1a4ab92223914065bcebf091876c1b4ea84a6891
                        customer.ClientMenu();
                        break;
                    case 2:
                        ingredient.IngredientMenu();
                        break;
                    case 3:
                        medicine.MedicineMenu();
                        break;
                    case 4:
                        produce.ProduceMenu();
                        break;
                    case 5:
                        produceItem.ProduceItemMenu();
                        break;
                    case 6:
                        venda.SaleMenu();
                        break;
                    case 7:
                        saleItens.SaleItensMenu();
                        break;
                    case 8:

                        supplier.MenuPrincipal();
                        break;
<<<<<<< HEAD
                    case 9:
                        break;
                    case 10:
                        break;
=======
                    //case 9:
                    //    purchaseItem.PurchaseItemMenu();
                    //    break;
                    //case 10:
                    //    purchase.PurchaseMenu();
                    //    break;
>>>>>>> 1a4ab92223914065bcebf091876c1b4ea84a6891
                    case 0:
                        Console.WriteLine("Encerrando programa. Obrigado!");
                        break;
                    default:
                        Console.WriteLine("invalido");
                        break;
                }

            } while (opcao != 0);
        }

        Menu();
    }
}
