using Application.Classes.Production;
using Application.Classes.Suppliers;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Purchase
{
    public class Purchase
    {
        Writer_Reader objeto = new();

        public List<Purchase> Purchases = new();

        public string? Id { get; private set; }
        public string? IdCompra { get; private set; }
        public DateOnly DataCompra { get; private set; }
        public string? FornecedorCNPJ { get; private set; }
        public decimal ValorTotal { get; private set; }
        private int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Purchase.data";
        static string fullPath = Path.Combine(diretorio, file);

        public Purchase() 
        {
            objeto.Verificador(diretorio, file);
            PopularLista();
        }

        public Purchase(string? id, string? idCompra, DateOnly dataCompra, string? fornecedorCNPJ, decimal valorTotal)
        {
            Id = id;
            IdCompra = idCompra;
            DataCompra = dataCompra;
            FornecedorCNPJ = fornecedorCNPJ;
            ValorTotal = valorTotal;
        }

        private void PopularLista()
        {
            StreamReader sr = new(fullPath);

            string line;

            while ((line = sr.ReadLine()!) != null)
            {
                string id = line[..5].Trim();

                string idCompra = line.Substring(5,5).Trim();

                DateOnly dataCompra = DateOnly.ParseExact(line.Substring(10, 8), "ddMMyyyy");

                string fornecedorCNPJ = line.Substring(18, 14).Trim();

                decimal valorTotal = decimal.Parse(line.Substring(32, 8).Trim());

                Purchase purchase = new(id, idCompra, dataCompra, fornecedorCNPJ, valorTotal);

                Purchases.Add(purchase);
            }
            sr.Close();

            if (Purchases.Count > 0)
                lastId = Purchases.Select(x => int.Parse(x.Id![..5])).Max();
            else
            {
                lastId = 0;
            }
        }

        private void SaveFile()
        {
            StreamWriter writer = new(fullPath);

            foreach (var purchase in Purchases)
            {
                string idFormatado = int.Parse(purchase.Id!).ToString("D5");

                string idCompraFormatado = int.Parse(purchase.IdCompra!).ToString("D5");

                string dataCompraFormatado = purchase.DataCompra.ToString("ddMMyyyy");

                string fornecedorCNPJ = purchase.FornecedorCNPJ!.PadRight(14);

                string valorTotal = purchase.ValorTotal.ToString("00000.00",CultureInfo.InvariantCulture).PadLeft(8);

                string dadoFinal = idFormatado + idCompraFormatado + dataCompraFormatado + fornecedorCNPJ + valorTotal;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        //public static bool TemEsteCNPJ(string cnpj)
        //{
        //    return Supplier.Suppliers.Any(c => c.Cnpj == cnpj && c.Cnpj is not RestrictedSupplier.FornecedoresRestritos.Cnpj);
        //}

        public static bool TemEsteCNPJ(string val)
        {
            bool existe = Supplier.Suppliers.Any(c => c.Cnpj == val);
            bool bloqueado = RestrictedSupplier.FornecedoresRestritos.Any(x => x.Cnpj == val);

            return existe && !bloqueado;
        }

        private void CreatePurchase()
        {
            lastId++;
            string id = lastId.ToString("D5");
            Console.WriteLine();

            string idCompra = lastId.ToString("D5");
            Console.WriteLine();

            DateOnly dataCompra = DateOnly.FromDateTime(DateTime.Today);
            Console.WriteLine();

            Console.WriteLine("Lista de fornecedores: ");
            Supplier.Suppliers.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("Selecione o CNPJ do fornecedor: ");
            string fornecedorCNPJ = Console.ReadLine()!;

            while (!TemEsteCNPJ(fornecedorCNPJ))
            {
                Console.WriteLine("CNPJ não encontrado, tente novamente.");
                fornecedorCNPJ = Console.ReadLine()!;
            }

            Console.WriteLine("Lista de itens de compra: ");
            PurchaseItem.PurchaseItems.ForEach(x => Console.WriteLine(x));

            int count = 0;
            decimal total = 0;

            Console.WriteLine("Selecione os ID's dos itens comprados: ");
            while (count < 3)
            {
                Console.Write("Vai adicionar um item à compra? (S/N): ");
                string val = Console.ReadLine()!.ToUpper();

                if (val == "N")
                    break;

                Console.Write("Selecione o ID do item: ");
                string idComprar = Console.ReadLine()!;

                var item = PurchaseItem.PurchaseItems.Find(x => x.Id == idComprar);
                if (item != null)
                {
                    total += item.ValorTotal;
                    count++;
                }
                else
                {
                    Console.WriteLine("Item não encontrado!");
                }
            }   

            Purchase pur = new(id, idCompra, dataCompra, fornecedorCNPJ, total);

            Purchases.Add(pur);

            var fornecedor = Supplier.Suppliers.Find(x => x.Cnpj == fornecedorCNPJ);
            if (fornecedor != null)
            {
                fornecedor.UltimoFornecimento = DateOnly.FromDateTime(DateTime.Today);
            }

            SaveFile();
        }

        private Purchase? FindPurchase()
        {
            Console.Write("Informe o ID do Purchase a ser encontrado: ");
            string variavel = Console.ReadLine()!;

            var purchaseMexido = Purchases.Find(x => x.Id == variavel);

            if (purchaseMexido == null)
            {
                Console.WriteLine("Produce não encontrado!");
                return null;
            }

            Console.WriteLine(purchaseMexido);
            return purchaseMexido;
        }

        private Purchase? UpdatePurchase()
        {
            var UpdatedPurchase = FindPurchase();

            Supplier.Suppliers.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("Selecione o CNPJ do fornecedor correto: ");
            string fornecedorNovoCNPJ = Console.ReadLine()!;

            while (!TemEsteCNPJ(fornecedorNovoCNPJ))
            {
                Console.WriteLine("CNPJ não encontrado, tente novamente.");
                fornecedorNovoCNPJ = Console.ReadLine()!;
            }

            UpdatedPurchase!.FornecedorCNPJ = fornecedorNovoCNPJ;

            SaveFile();

            return UpdatedPurchase;
        }

        private void PrintPurchases()
        {
            foreach (var purchase in Purchases)
            {
                Console.WriteLine(purchase);
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, CompraID: {IdCompra}, Data: {DataCompra:dd/MM/yyyy}, Fornecedor: {FornecedorCNPJ}, Total: {ValorTotal:C}";
        }

        public void PurchaseMenu()
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
                        CreatePurchase();
                        break;
                    case 2:
                        FindPurchase();
                        break;
                    case 3:
                        UpdatePurchase();
                        break;
                    case 4:
                        PrintPurchases();
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
