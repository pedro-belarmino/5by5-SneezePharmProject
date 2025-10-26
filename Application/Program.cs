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
                Console.WriteLine("Escolha uma op��o: ");
                Console.WriteLine("1 - Op��es de medicamento: ");
                Console.WriteLine("2 - Op��es de Princ�pios ativos: ");
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
                        Console.WriteLine("Informe uma op��o v�lida.");
                        break;
                }

            } while (opcao != 5);
        }

        Menu();
    }
}
