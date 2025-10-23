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

        public string? Id { get; private set; }
        public string? Nome { get; private set; }
        public DateOnly UltimaCompra { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char situacao { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Ingredient.data";
        string fullPath = Path.Combine(diretorio, file);

        public void Verificador()
        {
            try
            {
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                if (!File.Exists(fullPath))
                {
                    using (StreamWriter wr = new StreamWriter(fullPath)) { }
                    ;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        public void PopularLista()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                var dado = line.Split(',');
                livros.Add(new Livro(dado[0], dado[1], dado[2], dado[3]));
            }
            sr.Close();
        }

        public Ingredient()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
            Verificador();
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
            SaveFile();
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

            SaveFile();
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
            int opcao;
            do
            {
                Console.Write("Escolha uma opção: ");
                Console.Write("1 - Criar novo Ingrediente: ");
                Console.Write("2 - Encontrar algum Ingrediente: ");
                Console.Write("3 - Alterar algum ingrediente existente: ");
                Console.Write("4 - Imprimir todos os ingredientes: ");
                Console.Write("5 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        CreateIngredient();
                        break;
                    case 2:
                        FindIngredient();
                        break;
                    case 3:
                        UpdateIngredient();
                        break;
                    case 4:
                        PrintIngredient();
                        break;
                    case 5:
                        SaveFile();
                        return;
                    default:
                        Console.WriteLine("Informe uma opção válida.");
                        break;
                }
            } while (true);
        }
    }
}
