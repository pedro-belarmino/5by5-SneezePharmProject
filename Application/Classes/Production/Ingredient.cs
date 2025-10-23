using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Classes;
using Application.Utils.WritersAndReaders;

namespace Application.Classes.Production
{
    public class Ingredient
    {
        Writer_Reader objeto = new Writer_Reader();

        public List<Ingredient> Ingredients = new List<Ingredient>();

        public string Id { get; set; }
        public string Nome { get; set; }
        public DateOnly UltimaCompra { get; set; }
        public DateOnly DataCadastro { get; set; }
        public char situacao { get; set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Ingredient.data";
        string fullPath = Path.Combine(diretorio, file);

        public Ingredient()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
        }

        public Ingredient(string id, string nome, DateOnly ultimaCompra, DateOnly dataCadastro, char situacao)
        {
            Id = id;
            Nome = nome;
            UltimaCompra = ultimaCompra;
            DataCadastro = dataCadastro;
            this.situacao = situacao;
        }

        public void CreateIngredient()
        {
            Console.Write("Insira o Id do ingrediente: ");
            string Id = Console.ReadLine()!;

            Console.Write("Insira o nome do ingrediente: ");
            string Nome = Console.ReadLine()!;

            Console.Write("Insira a data da última compra do ingrediente: ");
            DateOnly Data = DateOnly.Parse(Console.ReadLine()!);

            Console.Write("Insira a Data de cadastro do ingrediente: ");
            DateOnly DataCadastro = DateOnly.Parse(Console.ReadLine()!);

            do
            {
                Console.Write("Informe a situação do ingrediente (A - Ativo, I - Inativo): ");
                char situacao = char.Parse(Console.ReadLine()!);

                if ((situacao != 'A') && (situacao != 'I'))
                    Console.WriteLine("Situação inválida, tente novamente.");

            } while ((situacao != 'A') && (situacao != 'I'));

            Ingredient novoIngredient = new(Id, Nome, Data, DataCadastro, situacao);

            Ingredients.Add(novoIngredient);
        }

        public Ingredient? FindIngredient()
        {
            Console.Write("Informe o ID do Ingredient a ser encontrado: ");
            string variavel = Console.ReadLine()!;
            var ingredienteMexido = Ingredients.Find(x => x.Id == variavel);
            return ingredienteMexido;
        }

        public Ingredient UpdateIngredient()
        {
            Ingredient UpdatedIngredient = FindIngredient()!;

            Console.Write("Informe o novo nome do ingrediente: ");
            UpdatedIngredient.Nome = Console.ReadLine()!;

            Console.Write("Informe a nova situação do Ingrediente: ");
            UpdatedIngredient.situacao = char.Parse(Console.ReadLine()!);

            do
            {
                Console.Write("Informe a nova situação do Ingrediente: ");
                UpdatedIngredient.situacao = char.Parse(Console.ReadLine()!);

                if ((UpdatedIngredient.situacao != 'A') && (UpdatedIngredient.situacao != 'I'))
                    Console.WriteLine("Situação inválida, tente novamente.");

            } while ((UpdatedIngredient.situacao != 'A') && (UpdatedIngredient.situacao != 'I'));

            return UpdatedIngredient;
        }

        public void PrintIngredient()
        {
            foreach (var ingredient in Ingredients)
                Console.WriteLine(ingredient);
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);
            foreach (var ingredient in Ingredients)
            {
                writer.WriteLine(ingredient);
            }
            writer.Close();
        }

        public void IngredientMenu()
        {

        }
    }
}
