using Application.Classes.Medicamento;
using Application.Classes.Production;

internal class Program
{
    private static void Main(string[] args)
    {
        Ingredient ingredient = new();
        Medicine medicine = new();
        Produce produce = new();
        ProduceItem produceItem = new();

        void Menu()
        {
            int opcao;

            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Opções de Cliente");
                Console.WriteLine("2 - Opções de Princípios ativos: ");
                Console.WriteLine("3 - Opções de medicamento: ");
                Console.WriteLine("4 - Opções de Produce");
                Console.WriteLine("5 - Opções de produce Item");
                Console.WriteLine("6 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        // lógica
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
                        Console.WriteLine("Encerrando programa. Obrigado!");
                        break;
                    default:
                        Console.WriteLine("Informe uma opção válida.");
                        break;
                }

            } while (opcao != 6);
        }

        Menu();
    }
}
