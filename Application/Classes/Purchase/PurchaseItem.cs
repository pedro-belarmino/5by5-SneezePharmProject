using Application.Classes.Production;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Purchase
{
    public class PurchaseItem
    {

        public static List<PurchaseItem> PurchaseItems= new();

        public string? Id { get; private set; }
        public string? IdCompra { get; private set; }
        public string? Ingrediente { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public decimal TotalItem { get; private set; }
        int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "PurchaseItem.data";
        string fullPath = Path.Combine(diretorio, file);

        public PurchaseItem() 
        {
            Verificador(diretorio, fullPath);
            PopularLista();
        }

        public PurchaseItem(string? id, string? idCompra, string? ingrediente, int quantidade, decimal valorUnitario, decimal totalItem)
        {
            Id = id;
            IdCompra = idCompra;
            Ingrediente = ingrediente;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            TotalItem = totalItem;
        }

        private void SaveFile()
        {
            StreamWriter writer = new(fullPath);

            foreach (var purchaseItem in PurchaseItems)
            {
                string idFormatado = int.Parse(purchaseItem.Id!).ToString("D5");

                string idCompraFormatado = int.Parse(purchaseItem.IdCompra!).ToString("D5");

                string? ingrediente = purchaseItem.Ingrediente?.PadRight(6);

                string qtdFormatado = purchaseItem.Quantidade.ToString("D4");

                string valorUnitario = purchaseItem.ValorUnitario.ToString("000.00", CultureInfo.InvariantCulture).PadLeft(6);

                string totalItem = purchaseItem.TotalItem.ToString("00000.00", CultureInfo.InvariantCulture).PadLeft(8);

                string dadoFinal = idFormatado + idCompraFormatado + ingrediente + qtdFormatado + valorUnitario + totalItem;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        private void PopularLista()
        {
            if (!File.Exists(fullPath)) return;

            using StreamReader sr = new(fullPath);
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                string id = line[..5].Trim();
                string idCompra = line.Substring(5, 5).Trim();
                string ingrediente = line.Substring(10, 6).Trim();
                int quantidade = int.Parse(line.Substring(16, 4).Trim());
                decimal valorUnitario = decimal.Parse(line.Substring(20, 6).Trim(), CultureInfo.InvariantCulture);
                decimal totalItem = decimal.Parse(line.Substring(26, 8).Trim(), CultureInfo.InvariantCulture);

                PurchaseItem purpur = new(id, idCompra, ingrediente, quantidade, valorUnitario, totalItem);

                PurchaseItems.Add(purpur);
            }

            lastId = PurchaseItems.Count > 0 ? PurchaseItems.Max(x => int.Parse(x.Id!)) : 0;
        }

        public string Verificador(string directoryPath, string fullPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(fullPath))
                {
                    using (StreamWriter wr = new StreamWriter(fullPath)) { }
                }
                return fullPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public static bool VerificaPrincipio(string variavel)
        {
            var ingredient = Ingredient.Ingredients.Find(x => x.Id == variavel && x.situacao == 'A');
            return ingredient != null;
        }

        public static bool VerificaAIDCompra(string variavel)
        {
            var idCompra = Purchase.Purchases.Find(x => x.Id == variavel);
            return idCompra != null;
        }

        private static bool VerificaQuantidade(int qtd)
        {
            if (qtd <= 0 || qtd >= 100000)
                return false;
            return true;
        }

        private static bool VerificaValorUnitario(decimal valor)
        {
            if (valor <= 0 && valor >= 1000)
                return false;
            return true;
        }

        public void CreatePurchaseItem()
        {
            lastId++;
            string id = lastId.ToString();
            Console.WriteLine();

            string idCompra = lastId.ToString();

            Console.WriteLine("--- Lista de Princípios Ativos ---");
            Ingredient.Ingredients.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("Insira o ID do princípio ativo utilizado");
            string IdPrincipioAtivo = Console.ReadLine()!;
            while (!VerificaPrincipio(IdPrincipioAtivo))
            {
                Console.WriteLine("Princípio ativo inexistente ou Inativo, tente novamente: ");
                IdPrincipioAtivo = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.WriteLine("Informe a quantidade de Purchase items: ");
            int quantidade = int.Parse(Console.ReadLine()!);
            while (!VerificaQuantidade(quantidade))
            {
                Console.WriteLine("Quantidade inválida, tente novamente.");
                quantidade = int.Parse(Console.ReadLine()!);
            }

            Console.WriteLine("Insira o valor unitário do Produce Item: ");
            decimal valorUni = decimal.Parse(Console.ReadLine()!);
            while (!VerificaValorUnitario(valorUni))
            {
                Console.WriteLine("Valor inválido, deve ser > 0 e < 1000 ");
                valorUni = decimal.Parse(Console.ReadLine()!);
            }

            decimal valorTotal = valorUni * quantidade;

            PurchaseItem purpur = new(id, idCompra, IdPrincipioAtivo, quantidade, valorUni, valorTotal);

            PurchaseItems.Add(purpur);
        }

        private PurchaseItem? FindPurchaseItem()
        {
            Console.Write("Informe o ID do Purchase Item a ser encontrado: ");
            string variavel = Console.ReadLine()!;

            var purchaseItemMexido = PurchaseItems.Find(x => x.Id == variavel);

            if (purchaseItemMexido == null)
            {
                Console.WriteLine("Purchase Item não encontrado!");
                return null;
            }

            Console.WriteLine(purchaseItemMexido);
            return purchaseItemMexido;
        }

        private PurchaseItem? UpdatePurchaseItem()
        {
            var UpdatedPurchaseItem = FindPurchaseItem();

            Console.WriteLine("Selecione o ID do Purchase Item a ser atualizado: ");
            string id = Console.ReadLine()!;

            Console.WriteLine("Informe a nova quantidade: ");
            UpdatedPurchaseItem!.Quantidade = int.Parse(Console.ReadLine()!); 

            SaveFile();

            return UpdatedPurchaseItem;
        }

        private void PrintPurchaseItems()
        {
            foreach (var purchaseItem in PurchaseItems)
            {
                Console.WriteLine(purchaseItem);
            }
        }

        public override string? ToString()
        {
            return $"ID: {Id}, IDCompra: {IdCompra}, Ingredientes: {Ingrediente}, Quantidade: {Quantidade}, Valor Unitário: R${ValorUnitario}, Valor Total: {TotalItem}";
        }

        public void PurchaseItemMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo Purchase: ");
                Console.WriteLine("2 - Encontrar algum Purchase: ");
                Console.WriteLine("3 - Alterar algum Purchase existente: ");
                Console.WriteLine("4 - Imprimir todos os Purchases: ");
                Console.WriteLine("5 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        CreatePurchaseItem();
                        break;
                    case 2:
                        FindPurchaseItem();
                        break;
                    case 3:
                        UpdatePurchaseItem();
                        break;
                    case 4:
                        PrintPurchaseItems();
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
