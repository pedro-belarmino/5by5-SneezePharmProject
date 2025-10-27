using Application.Classes.Production;
using Application.Classes.Suppliers;
using Application.Compra; 
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Application.Classes
{
    public class Purchase
    {
        Writer_Reader objeto = new Writer_Reader();

        public List<Purchase> Purchases = new List<Purchase>();

        //"carrinho de compra"
        private List<PurchaseItem> ItensDaCompra = new List<PurchaseItem>();

        public string Id { get; private set; }
        public DateOnly DataCompra { get; private set; }
        public string CnpjFornecedor { get; private set; }
        public decimal ValorTotal { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Purchase.data";
        string fullPath = Path.Combine(diretorio, file);

        public Purchase()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório verificados com sucesso.");
            Verificador();
        }

        public Purchase(string id, DateOnly dataCompra, string cnpjFornecedor, decimal valorTotal)
        {
            Id = id;
            DataCompra = dataCompra;
            CnpjFornecedor = cnpjFornecedor;
            ValorTotal = valorTotal;
        }


        public void Verificador()
        {
            try
            {
                if (!Directory.Exists(diretorio))
                    Directory.CreateDirectory(diretorio);

                if (!File.Exists(fullPath))
                    using (StreamWriter wr = new StreamWriter(fullPath)) { }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            PopularLista();
        }


        public string GerarIdUnico()
        {
            Random random = new Random();
            string novoId;
            do
            {
                novoId = random.Next(0, 99999).ToString("D5");
            } while (Purchases.Exists(p => p.Id == novoId));

            return novoId;
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
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string id = line.Substring(0, 5).Trim();
                    string dataStr = line.Substring(5, 13).Trim();
                    string cnpj = line.Substring(13, 27).Trim();
                    string valorTotalStr = line.Substring(27, 38).Trim();

                    DateOnly dataCompra = DateOnly.ParseExact(dataStr, "ddMMyyyy");
                    decimal valorTotal = decimal.TryParse(valorTotalStr, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out decimal vt) ? vt : 0m;

                    Purchase p = new Purchase(id, dataCompra, cnpj, valorTotal);
                    Purchases.Add(p);
                }
            }
        }

        public void CreatePurchase()
        {
            Supplier fornecedor = new Supplier();   

            Console.Write("Informe o CNPJ do fornecedor: ");
            string cnpj = Console.ReadLine()!;



            //verifica se fornecedor esta bloqueado
            new RestrictedSupplier();
            bool bloqueado = RestrictedSupplier.FornecedoresRestritos.Any(f => f.Cnpj.Trim() == cnpj);

            if (bloqueado)
            {
                Console.WriteLine("Este fornecedor está bloqueado. Compra cancelada.");
                return;
            }

            // verifica fornecedor na lista normal
            new Supplier();
            var fornecedorExistente = Supplier.Suppliers.Find(f => f.Cnpj.Trim() == cnpj);

            int tempoAbertura = fornecedorExistente.DataAbertura.Year - DateTime.Now.Year;
            if (tempoAbertura < 2)
            {
                Console.WriteLine("Fornecedor tem menos de 2 anos de empresa aberta.");
                return;
            }

            if (fornecedorExistente == null)
            {
                Console.WriteLine("Fornecedor não encontrado. Compra cancelada.");
                return;
            }

            if (fornecedorExistente.Situacao != 'A')
            {
                Console.WriteLine("Fornecedor está inativo. Compra cancelada.");
                return;
            }

            string novoId = GerarIdUnico();
            DateOnly dataCompra = DateOnly.FromDateTime(DateTime.Now);

            Console.WriteLine("\n--- Adicione até 3 itens à compra ---");
            PurchaseItem purchaseItem = new PurchaseItem();

            int contador = 0;
            while (contador < 3)
            {
                Console.Write("Deseja adicionar um item? (S/N): ");
                string opc = Console.ReadLine()!.ToUpper();
                if (opc != "S") break;

                purchaseItem.CreatePurchaseItem();
                var ultimoItem = purchaseItem.PurchaseItems.LastOrDefault();
                if (ultimoItem != null)
                {
                    ItensDaCompra.Add(ultimoItem);
                    contador++;
                }
            }

            if (ItensDaCompra.Count == 0)
            {
                Console.WriteLine("Nenhum item adicionado. Compra cancelada.");
                return;
            }

            decimal valorTotal = 0;
            foreach (var item in ItensDaCompra)
            valorTotal += item.TotalItem;

            if (valorTotal >= 100000000m)
            {
                Console.WriteLine("Valor total excede o limite permitido (máx. 11 dígitos).");
                return;
            }

            fornecedorExistente.UltimoFornecimento = dataCompra;    ///////////////////////

            Purchase novaCompra = new Purchase(novoId, dataCompra, cnpj, valorTotal);
            Purchases.Add(novaCompra);

            SaveFile();
            Console.WriteLine("Compra criada com sucesso!");
        }

        public Purchase? FindPurchase()
        {
            Console.Write("Informe o ID da compra: ");
            string id = Console.ReadLine()!;
            var compra = Purchases.Find(p => p.Id == id);
            if (compra == null)
                Console.WriteLine("Compra não encontrada.");
            else
                Console.WriteLine(compra);
            return compra;
        }

        public void UpdatePurchase()
        {
            var compra = FindPurchase();
            if (compra == null) return;

            Console.WriteLine("Deseja adicionar ou remover itens? (A/R): ");
            string opc = Console.ReadLine()!.ToUpper();


            if (opc == "A" && ItensDaCompra.Count < 3)
            {
                PurchaseItem operacao = new PurchaseItem();
                operacao.CreatePurchaseItem();
                var novoItem = operacao.PurchaseItems.LastOrDefault();
                if (novoItem != null)
                ItensDaCompra.Add(novoItem);
            }
            else if (opc == "R")
            {
                Console.Write("Informe o ID do item a remover: ");
                string idItem = Console.ReadLine()!;
                var item = ItensDaCompra.Find(i => i.Id == idItem);
                if (item != null)
                ItensDaCompra.Remove(item);
            }
            //somando os valores
            compra.ValorTotal = ItensDaCompra.Sum(i => i.TotalItem);
            SaveFile();
        }

        public void PrintPurchases()
        {
            foreach (var compra in Purchases)
                Console.WriteLine(compra);
        }
        public void SaveFile()
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var compra in Purchases)
                {
                    string idFormatado = compra.Id.PadRight(5);
                    string dataFormatada = compra.DataCompra.ToString("ddMMyyyy");
                    string cnpjFormatado = compra.CnpjFornecedor.PadRight(14);
                    string valorTotalFormatado = compra.ValorTotal.ToString("F2", CultureInfo.InvariantCulture).PadLeft(11);

                    string linha = idFormatado + dataFormatada + cnpjFormatado + valorTotalFormatado;
                    writer.WriteLine(linha);
                }
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, Data: {DataCompra:dd/MM/yyyy}, Fornecedor: {CnpjFornecedor}, Valor Total: {ValorTotal:F2}";
        }

        public void RelatorioComprasFornecedor() //extra
        {
            Console.Write("Informe o CNPJ do fornecedor: ");
            string cnpj = Console.ReadLine()!.Trim();

            
            var fornecedorExistente = Supplier.Suppliers.Find(f => f.Cnpj.Trim() == cnpj);

            if (fornecedorExistente == null)
            {
                Console.WriteLine("Fornecedor não encontrado.");
                return;
            }

            //retorna uma nova lista contendo todas as compras.
            var comprasDoFornecedor = Purchases.FindAll(p => p.CnpjFornecedor.Trim() == cnpj);

            if (comprasDoFornecedor.Count == 0)
            {
                Console.WriteLine($"Não há compras registradas para o fornecedor {fornecedorExistente.RazaoSocial}.");
                return;
            }

            Console.WriteLine($"\nRelatório de Compras do Fornecedor: {fornecedorExistente.RazaoSocial}");
            decimal valorTotal = 0;

            foreach (var compra in comprasDoFornecedor)
            {
                Console.WriteLine(compra); // Usa o ToString() da classe Purchase
                valorTotal += compra.ValorTotal;
            }

            Console.WriteLine($"\nNúmero de compras: {comprasDoFornecedor.Count}");
            Console.WriteLine($"Valor total comprado: {valorTotal:F2}");
        }

        public void PurchaseMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("\nMENU DE COMPRAS");
                Console.WriteLine("1 - Criar nova compra");
                Console.WriteLine("2 - Encontrar compra");
                Console.WriteLine("3 - Atualizar compra");
                Console.WriteLine("4 - Listar todas as compras");
                Console.WriteLine("5 - Relatório de compras por fornecedor");
                Console.WriteLine("6 - Sair");
                Console.Write("Escolha uma opção: ");
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
                        RelatorioComprasFornecedor();
                            break;
                    case 6:
                        SaveFile();
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            } while (opcao != 6);
        }
    }
}

