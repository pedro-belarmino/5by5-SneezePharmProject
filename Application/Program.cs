using Application;
using Application.Classes.Medicamento;
using Application.Classes.Production;
using Application.Classes.Sales;

internal class Program
{
    private static void Main(string[] args)
    {
        Ingredient ingredient = new();
        Medicine medicine = new();
        Produce produce = new();
        ProduceItem produceItem = new();
        Sale venda = new();
        Customer customer = new();

        void Menu()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Opções de Cliente");
                Console.WriteLine("2 - Opções de Princípios ativos: "); //feito
                Console.WriteLine("3 - Opções de medicamento: ");// feito
                Console.WriteLine("4 - Opções de Producao");// feito
                Console.WriteLine("5 - Opções de Item de Producao");
                Console.WriteLine("6 - Opções de Venda");// feito
                Console.WriteLine("7 - Opções de Item Venda");// 

                Console.WriteLine("9 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
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
                    case 9:
                        Console.WriteLine("Encerrando programa. Obrigado!");
                        break;
                    default:
                        Console.WriteLine("invalido");
                        break;
                }

            } while (opcao != 7);
        }

        Menu();
    }
}
