using Application.Classes.Medicamento;
using Application.Classes.Production;

internal class Program
{
    private static void Main(string[] args)
    {
        Ingredient ingredient = new();
        Medicine medicine = new();

        void Menu()
        {
            int opcao;

            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Opções de medicamento: ");
                Console.WriteLine("2 - Opções de Princípios ativos: ");
                Console.WriteLine("3 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        medicine.MedicineMenu();
                        break;
                    case 2:
                        ingredient.IngredientMenu();
                        break;
                    case 3:
                        Console.WriteLine("Encerrando programa. Obrigado!");
                        return;
                    default:
                        Console.WriteLine("Informe uma opção válida.");
                        break;
                }

            } while (opcao != 5);
        }

        Menu();
    }
}
