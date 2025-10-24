//using Application.Prod;
using Application.Classes.Production;
using Application.Utils.WritersAndReaders;

internal class Program
{
    private static void Main(string[] args)
    {
        Ingredient ingredient = new Ingredient();

        ingredient.IngredientMenu();
    }
}
