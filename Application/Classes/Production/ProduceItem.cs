using Application.Classes;
using Application.Classes.Medicamento;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Production
{
    public class ProduceItem
    {
        readonly Writer_Reader objeto = new();

        public static List<ProduceItem> ProduceItems = [];

        public string? Id { get; private set; }
        public string? IdProducao { get; private set; }
        public string? IdPrincipio { get; private set; }
        public int QuantidadePrincipio { get; private set; }
        private int lastId = 0;

        readonly static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        readonly static string file = "ProduceItem.data";
        readonly static string fullPath = Path.Combine(diretorio, file);

        public ProduceItem()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine($"Arquivo {file} e diretório criados com sucesso.");
            PopularListaProduceItem();
        }

        public ProduceItem(string? id, string? idProducao, string? idPrincipio, int quantidadePrincipio)
        {
            Id = id;
            IdProducao = idProducao;
            IdPrincipio = idPrincipio;
            QuantidadePrincipio = quantidadePrincipio;
        }

        public void PopularListaProduceItem()
        {
            StreamReader sr = new(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string id = line.Substring(0, 5).Trim();
                string idProducao = line.Substring(5, 5).Trim();
                string idPrincipio = line.Substring(10, 6).Trim(); ;
                int quantidade = int.Parse(line.Substring(16, 4).Trim());

                ProduceItem produceItem = new(id, idProducao, idPrincipio, quantidade);

                ProduceItems.Add(produceItem);
            }
            sr.Close();

            if (ProduceItems.Count > 0)
                lastId = ProduceItems.Select(x => int.Parse(x.Id!.Substring(0, 5))).Max();
            else
            {
                lastId = 0;
            }
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);

            foreach (var produceItem in ProduceItems)
            {
                string IdFormatado = produceItem.Id!.PadLeft(5, '0');
                string IdProducaoFormatado = produceItem.IdProducao!.PadRight(5);
                string IdPrincipioFormatado = produceItem.IdPrincipio!.PadRight(6);
                string quantidadeFormatado = produceItem.QuantidadePrincipio!.ToString("0000");

                string dadoFinal = IdFormatado + IdProducaoFormatado + IdPrincipioFormatado + quantidadeFormatado;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        public void CreateProduceItem()
        {
            lastId++;
            string Id = $"{lastId}";
            Console.WriteLine();

            Console.WriteLine("--- Lista de Produces ---");
            Produce.Produces.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("Insira o ID do item produzido: ");
            string IdProducao = Console.ReadLine()!;
            while (!VerificaIdProducao(IdProducao))
            {
                Console.WriteLine("ID incorreto ou inexistente, tente novamente.");
                IdProducao = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.WriteLine("--- Lista de Princípios Ativos ---");
            Ingredient.Ingredients.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("Insira o ID do princípio ativo utilizado");
            string IdPrincipioAtivo = Console.ReadLine()!;
            while (!VerificaPrincipio(IdPrincipioAtivo))
            {
                Console.WriteLine("Princípio ativo inválido ou inexistente, tente novamente: ");
                IdPrincipioAtivo = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.WriteLine("Informe a quantidade do princípio ativo utilizado: ");
            int quantidadePrincipio = int.Parse(Console.ReadLine()!);
            while (!VerificaQuantidade(quantidadePrincipio))
            {
                Console.WriteLine("Quantidade inválida, tente novamente: ");
                quantidadePrincipio = int.Parse(Console.ReadLine()!);
            }
            Console.WriteLine();

            ProduceItem newProduceItem = new(Id, IdProducao, IdPrincipioAtivo, quantidadePrincipio);

            ProduceItems.Add(newProduceItem);

            SaveFile();
        }

        private void PrintProduceItems()
        {
            foreach (var produceItem in ProduceItems)
            {
                Console.WriteLine(produceItem);
            }
        }

        private ProduceItem? FindProduceItem()
        {
            Console.Write("Informe o ID do Produce Item a ser encontrado: ");
            string variavel = Console.ReadLine()!;
            var produceItemMexido = ProduceItems.Find(x => x.Id == variavel);
            Console.WriteLine(produceItemMexido);
            return produceItemMexido;
        }

        private ProduceItem UpdateProduceItem()
        {
            ProduceItem UpdatedProduceItem = FindProduceItem()!;

            Console.Write("informe a nova quantidade fabricada: ");
            UpdatedProduceItem.QuantidadePrincipio = int.Parse(Console.ReadLine()!);

            SaveFile();
            return UpdatedProduceItem;
        }

        private static bool VerificaQuantidade(int qtd)
        {
            if (qtd <= 0 || qtd >= 10000)
                return false;
            return true;
        }

        public static bool VerificaPrincipio(string Id)
        {
            var ingredient = Ingredient.Ingredients.Find(x => x.Id == Id && x.situacao == 'A');
            return ingredient != null;
        }

        public static bool VerificaIdProducao(string Id)
        {
            var produce = Produce.Produces.Find(x => x.Id == Id);
            return produce != null;
        }

        public override string? ToString()
        {
            string idFormatado = Id!.PadLeft(5, '0');
            return $"ID: {idFormatado}, IDProdução: {IdProducao}, IDPrincípio: {IdPrincipio}, Quantidade de princípios: {QuantidadePrincipio}";
        }

        public void ProduceItemMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo Produce Item: ");
                Console.WriteLine("2 - Encontrar algum Produce Item: ");
                Console.WriteLine("3 - Alterar algum Produce Item existente: ");
                Console.WriteLine("4 - Imprimir todos os Produce items: ");
                Console.WriteLine("5 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        CreateProduceItem();
                        break;
                    case 2:
                        FindProduceItem();
                        break;
                    case 3:
                        UpdateProduceItem();
                        break;
                    case 4:
                        PrintProduceItems();
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
