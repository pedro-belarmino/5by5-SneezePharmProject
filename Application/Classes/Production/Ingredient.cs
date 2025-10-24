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
            PopularLista();
        }

        public void PopularLista()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string id = line.Substring(0, 5).Trim();
                string nome = line.Substring(5, 20).Trim();
                DateOnly ultimaCompra = DateOnly.ParseExact(line.Substring(25, 8), "ddMMyyyy");
                DateOnly dataCadastro = DateOnly.ParseExact(line.Substring(33, 8), "ddMMyyyy");
                char situacao = line[41];

                Ingredient ing = new Ingredient(id, nome, ultimaCompra, dataCadastro, situacao);

                Ingredients.Add(ing);
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
            Console.WriteLine();

            Console.Write("Insira o nome do ingrediente: ");
            string Nome = Console.ReadLine()!;
            Console.WriteLine();

            Console.Write("Insira a data da última compra do ingrediente DD/MM/AAAA: ");
            DateOnly Data = DateOnly.Parse(Console.ReadLine()!);
            Console.WriteLine();

            Console.Write("Insira a Data de cadastro do ingrediente DD/MM/AAAA: ");
            DateOnly DataCadastro = DateOnly.Parse(Console.ReadLine()!);
            Console.WriteLine();

            char situacao;

            do
            {
                Console.Write("Informe a situação do ingrediente (A - Ativo, I - Inativo): ");
                situacao = char.Parse(Console.ReadLine()!);

                if (situacao != 'A' && situacao != 'I')
                    Console.WriteLine("Situação inválida, tente novamente.");

            } while (situacao != 'A' && situacao != 'I');

            Ingredient novoIngredient = new(Id, Nome, Data, DataCadastro, situacao);

            Ingredients.Add(novoIngredient);

            SaveFile();
        }

        public Ingredient? FindIngredient()
        {
            Console.Write("Informe o ID do Ingredient a ser encontrado: ");
            string variavel = Console.ReadLine()!;
            var ingredienteMexido = Ingredients.Find(x => x.Id == variavel);
            Console.WriteLine(ingredienteMexido);
            return ingredienteMexido;
        }

        public Ingredient UpdateIngredient()
        {
            Ingredient UpdatedIngredient = FindIngredient()!;

            Console.Write("Informe o novo nome do ingrediente: ");
            UpdatedIngredient.Nome = Console.ReadLine()!;

            do
            {
                Console.Write("Informe a nova situação do Ingrediente (A - Ativo, I - Inativo): ");
                UpdatedIngredient.situacao = char.Parse(Console.ReadLine()!);

                if (UpdatedIngredient.situacao != 'A' && UpdatedIngredient.situacao != 'I')
                    Console.WriteLine("Situação inválida, tente novamente.");

            } while (UpdatedIngredient.situacao != 'A' && UpdatedIngredient.situacao != 'I');

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
                string idFormatado = ingredient.Id!.PadRight(5);
                string nomeFormatado = ingredient.Nome!.PadRight(20);
                string UltimaCompraFormatado = ingredient.UltimaCompra.ToString("ddMMyyyy");
                string DataCadastroFormatado = ingredient.DataCadastro.ToString("ddMMyyyy");

                string dadoFinal = idFormatado + nomeFormatado + UltimaCompraFormatado + DataCadastroFormatado + ingredient.situacao;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        public override string? ToString()
        {
            return $"ID: {Id}, Nome: {Nome}, Ultima Compra: {UltimaCompra}, Data de cadastro: {DataCadastro}, Situação: {situacao}";
        }

        public void IngredientMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo Ingrediente: ");
                Console.WriteLine("2 - Encontrar algum Ingrediente: ");
                Console.WriteLine("3 - Alterar algum ingrediente existente: ");
                Console.WriteLine("4 - Imprimir todos os ingredientes: ");
                Console.WriteLine("5 - Sair");
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
            } while (opcao != 5);
        }
    }
}
