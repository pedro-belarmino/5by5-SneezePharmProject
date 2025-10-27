using Application.Classes.Production;
using Application.Utils.WritersAndReaders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Compra
{
    public class PurchaseItem
    {
        Writer_Reader objeto = new Writer_Reader();
        public List<PurchaseItem> PurchaseItems = new List<PurchaseItem>();
        public string? Id { get; private set; }
        public string? Nome { get; private set; }
        public string IdIngrediente { get; private set; }   //recebe do ingrediente
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public decimal TotalItem => Quantidade * ValorUnitario;
        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Ingredient.data";
        string fullPath = Path.Combine(diretorio, file);
        public string GerarIdUnico()
        {
            Random random = new Random();
            string novoId;
            do
            {
                novoId = random.Next(0, 100000).ToString("D5");
            }
            // Repete se o ID ja tiver na lista
            while (PurchaseItems.Any(x => x.Id == novoId));
            return novoId;
        }
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
            if (!File.Exists(fullPath))
                return;
            using (StreamReader sr = new StreamReader(fullPath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))    //verifica se a string é null,vazia ou tem caracteres de espaço em branco
                        continue;                           //pule para a próx iteração
                    string id = line.Substring(0, 5).Trim();
                    string nome = line.Substring(5, 25).Trim();
                    string idIngrediente = line.Substring(25, 31).Trim();
                    string quantidadeStr = line.Substring(31, 35).Trim();
                    string valorUnitarioStr = line.Substring(35, 43).Trim();
                    string totalItemStr = line.Substring(43, 51).Trim();
                    //tratanto inteiro para string
                    int quantidade = int.TryParse(quantidadeStr, out int q) ? q : 0;
                    //tratando decimal para string
                    decimal valorUnitario = decimal.TryParse(valorUnitarioStr,
                        System.Globalization.NumberStyles.Any, System.Globalization.
                        CultureInfo.InvariantCulture, out decimal vu) ? vu : 0m;
                    PurchaseItem purchase = new PurchaseItem(id, nome, idIngrediente, quantidade, valorUnitario);
                    PurchaseItems.Add(purchase);
                }
            }
        }
        public PurchaseItem()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
            Verificador();
        }
        public PurchaseItem(string id, string nome, string idIngrediente, int quantidade, decimal valorUnitario)
        {
            if (quantidade <= 0 || quantidade > 9999)
                throw new ArgumentException("Quantidade deve ser entre 1 e 9999.");
            if (valorUnitario <= 0 || valorUnitario > 999.99m)
                throw new ArgumentException("Valor unitário inválido.");
            Id = id;
            Nome = nome;
            IdIngrediente = idIngrediente;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
        public void CreatePurchaseItem()
        {
            string Id = GerarIdUnico();
            Console.WriteLine("ID gerado: " + Id);
            Console.WriteLine();
            Console.Write("Insira o nome do item: ");
            string Nome = Console.ReadLine()!;
            Console.WriteLine();
            Console.Write("Insira o ID do ingrediente: ");
            string idIngrediente = Console.ReadLine()!;
            Ingredient.Ingredients.ForEach(x => Console.WriteLine(x));
            Console.Write("Insira a quantidade de itens: ");
            int Quantidade = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Insira o valor unitário do item: ");
            decimal ValorUnitario = decimal.Parse(Console.ReadLine());
            Console.WriteLine();
            PurchaseItem NovoPurchaseItem = new(Id, Nome, idIngrediente, Quantidade, ValorUnitario);
            Console.WriteLine($"Total do item: {NovoPurchaseItem.TotalItem:F2}");
            PurchaseItems.Add(NovoPurchaseItem);
            SaveFile();
        }
        public PurchaseItem? FindPurchaseItem()
        {
            Console.Write("Informe o ID do Item a ser encontrado: ");
            string variavel = Console.ReadLine()!;
            var itemProcurado = PurchaseItems.Find(x => x.Id == variavel);
            Console.WriteLine(itemProcurado);
            return itemProcurado;
        }
        public PurchaseItem? UpdatePurchaseItem()
        {
            var updatedPurchaseItem = FindPurchaseItem();
            if (updatedPurchaseItem == null)
                return null;
            Console.Write("Informe o novo nome do item: ");
            updatedPurchaseItem.Nome = Console.ReadLine()!;
            Console.Write("Informe a nova quantidade: ");
            updatedPurchaseItem.Quantidade = int.Parse(Console.ReadLine()!);
            SaveFile();
            return updatedPurchaseItem;
        }
        public void PrintPurchaseItem()
        {
            foreach (var purchaseItem in PurchaseItems)
                Console.WriteLine(purchaseItem);
        }
        public void SaveFile()
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var purchaseItem in PurchaseItems)
                {
                    string idFormatado = purchaseItem.Id!.PadRight(5);
                    string nomeFormatado = purchaseItem.Nome!.PadRight(20);
                    string idIngredienteFormatado = purchaseItem.IdIngrediente.PadRight(6);
                    string quantidadeFormatado = purchaseItem.Quantidade.ToString("D4");
                    string valorUnitarioFormatado = purchaseItem.ValorUnitario.ToString("F2").PadLeft(8);
                    string totalItemFormatado = purchaseItem.TotalItem.ToString("F2").PadLeft(8);
                    string dadoFinal = idFormatado + nomeFormatado + idIngredienteFormatado + quantidadeFormatado + valorUnitarioFormatado + totalItemFormatado;
                    writer.WriteLine(dadoFinal);
                }
            }
        }
        public override string ToString()
        {
            return $"ID: {Id}, Nome: {Nome}  IdIngrediente:  {IdIngrediente} Quantidade: {Quantidade}  ValorUnitario: {ValorUnitario}  TotalItem: {TotalItem:2}";
        }
        public void PurchaseItemMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo item: ");
                Console.WriteLine("2 - Encontrar algum item: ");
                Console.WriteLine("3 - Alterar algum item existente: ");
                Console.WriteLine("4 - Imprimir todos os itens: ");
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
                        PrintPurchaseItem();
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