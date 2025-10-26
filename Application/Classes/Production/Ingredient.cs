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
        private int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Ingredient.data";
        static string fullPath = Path.Combine(diretorio, file);

        public Ingredient()
        {
            objeto.Verificador(diretorio, fullPath);
            PopularLista();
        }

        public Ingredient(string id, string nome, DateOnly ultimaCompra, DateOnly dataCadastro, char situacao)
        {
            Id = id;
            Nome = nome;
            UltimaCompra = ultimaCompra;
            DataCadastro = dataCadastro;
            this.situacao = situacao;
        }

        public void PopularLista()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string id = line.Substring(0, 6).Trim();
                string nome = line.Substring(6, 20).Trim();
                DateOnly ultimaCompra = DateOnly.ParseExact(line.Substring(26, 8), "ddMMyyyy");
                DateOnly dataCadastro = DateOnly.ParseExact(line.Substring(34, 8), "ddMMyyyy");
                char situacao = line[41];

                Ingredient ing = new Ingredient(id, nome, ultimaCompra, dataCadastro, situacao);

                Ingredients.Add(ing);
            }
            sr.Close();

            if (Ingredients.Count > 0)
                lastId = Ingredients.Select(x => int.Parse(x.Id!.Substring(2, 4))).Max();
            else
            {
                lastId = 0;
            }
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);
            foreach (var ingredient in Ingredients)
            {
                string idFormatado = ingredient.Id!.PadRight(6);
                string nomeFormatado = ingredient.Nome!.PadRight(20);
                string UltimaCompraFormatado = ingredient.UltimaCompra.ToString("ddMMyyyy");
                string DataCadastroFormatado = ingredient.DataCadastro.ToString("ddMMyyyy");

                string dadoFinal = idFormatado + nomeFormatado + UltimaCompraFormatado + DataCadastroFormatado + ingredient.situacao;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        public bool VerificaNome(string nome)
        {
            foreach (char letra in nome)
            {
                if (!char.IsLetterOrDigit(letra))
                {
                    return false;
                }
            }
            return true;
        }

        public void CreateIngredient()
        {
            lastId++;
            string Id = $"AI{lastId:D4}";
            Console.WriteLine();

            Console.Write("Insira o nome do medicamento: ");
            string nome = Console.ReadLine()!;
            while (!VerificaNome(nome))
            {
                Console.WriteLine("Nome inválido, são permitidos apenas caracteres alfanuméricos. tente novamente. ");
                nome = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.Write("Insira a data da última compra do ingrediente DD-MM-AAAA: ");
            DateOnly Data = DateOnly.Parse(Console.ReadLine()!);
            Console.WriteLine();

            Console.Write("Insira a Data de cadastro do ingrediente DD-MM-AAAA: ");
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

            Ingredient? novoIngredient = new(Id, Nome!, Data, DataCadastro, situacao);

            Ingredients.Add(novoIngredient);

            SaveFile();
        }

        public void PrintIngredient()
        {
            foreach (var ingredient in Ingredients)
                Console.WriteLine(ingredient);
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

            Console.Write("Insira o novo nome do medicamento: ");
            string nome = Console.ReadLine()!;
            while (!VerificaNome(nome))
            {
                Console.WriteLine("Nome inválido, são permitidos apenas caracteres alfanuméricos. tente novamente. ");
                nome = Console.ReadLine()!;
            }
            Console.WriteLine();

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
