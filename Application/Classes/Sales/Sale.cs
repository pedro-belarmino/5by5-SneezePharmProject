using Application.Classes.Medicamento;
using Application.Utils;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Reflection.Metadata.Ecma335;

using System.Xml;


namespace Application.Classes.Sales
{
    public class Sale
    {
        Writer_Reader objeto = new Writer_Reader();
        public static List<Sale> Sales = new List<Sale>();

        public string IdVenda { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public string ClienteCPF { get; private set; }
        public decimal ValorTotal { get; private set; }
        public List<Medicine> RelatorioDeVendasPorCDB { get; private set; } = new List<Medicine>();

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Sales.data";
        string fullPath = Path.Combine(diretorio, file);

        private int lastId = 0;
        public Sale()
        {
            objeto.Verificador(diretorio, fullPath);
            Verificador();
        }

        public Sale(string i, DateOnly d, string c, decimal v)
        {
            this.IdVenda = i;
            this.DataVenda = d;
            this.ClienteCPF = c;
            this.ValorTotal = v;
        }

        public void CreateSale()
        {
            lastId++;
            string ID = $"{lastId:D5}";

            Console.WriteLine("Insira o cpf do cliente");
            string cpf = Console.ReadLine()!;
            string cpfAjustado = cpf.PadLeft(11, '0');

            if (!personExists(cpfAjustado))
            {
                Console.WriteLine("CPF não encontrado");
                return;
            }

            Console.WriteLine("Insira a data da venda (dd/mm/yyyy)");
            DateOnly data = DateOnly.Parse(Console.ReadLine()!);

            decimal vu;
            while (true)
            {
                Console.WriteLine("Valor unitário (máximo permitido: 9999.99)");
                if (!decimal.TryParse(Console.ReadLine(), out vu))
                {
                    Console.WriteLine("Valor inválido. Tente novamente.");
                    continue;
                }

                if (vu > 9999.99m)
                {
                    Console.WriteLine("O valor unitário não pode ultrapassar 9999.99. Digite novamente.");
                    continue;
                }

                if (vu < 0)
                {
                    Console.WriteLine("O valor unitário não pode ser negativo. Digite novamente.");
                    continue;
                }

                break;
            }

            int qtd;
            while (true)
            {
                Console.WriteLine("Insira a quantidade");
                if (!int.TryParse(Console.ReadLine(), out qtd) || qtd <= 0)
                {
                    Console.WriteLine("Quantidade inválida. Digite um número inteiro positivo.");
                    continue;
                }

                break;
            }

            decimal vt = vu * qtd;

            while (vt > 99999999.99m)
            {
                Console.WriteLine("O valor total não pode ultrapassar 99.999.999,99.");
                Console.WriteLine("Digite novamente o valor unitário ou a quantidade.");

                Console.WriteLine("Valor unitário (máximo permitido: 9999.99)");
                vu = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Insira a quantidade");
                qtd = int.Parse(Console.ReadLine()!);

                vt = vu * qtd;

                if (vu > 9999.99m)
                {
                    Console.WriteLine("O valor unitário não pode ultrapassar 9999.99. Digite novamente.");
                    continue;
                }
            }

            Sale newSale = new(ID, data, cpfAjustado, vt);
            Sales.Add(newSale);
            SaveFile();
            UpdateSaleDateAtClient(cpfAjustado, data);
        }


        Customer person = new Customer();

        private bool personExists(string cpf)
        {
            var normalized = Customer.NormalizeCpf(cpf);
            var pessoa = person.SearchCPF(normalized);
            return pessoa != null;
        }

        public void UpdateSaleDateAtClient(string cpf, DateOnly dataVenda)
        {
            person.UpdateSaleDate(cpf, dataVenda);
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
            PopularListaVendas();
        }

        public void PopularListaVendas()
        {
            StreamReader sr = new StreamReader(fullPath);
            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string idVenda = line.Substring(0, 5).Trim();
                DateOnly dataVenda = DateOnly.ParseExact(line.Substring(5, 8), "ddMMyyyy");
                string clienteCPF = line.Substring(13, 11);
                decimal valorTotalDecimal = decimal.Parse(line.Substring(24, 11));
                decimal valorTotalDecimalComPonto = valorTotalDecimal / 100;
                Sale venda = new Sale(idVenda, dataVenda, clienteCPF, valorTotalDecimalComPonto);
                Sales.Add(venda);
            }
            sr.Close();

            if (Sales.Count > 0)
                lastId = Sales.Select(x => int.Parse(x.IdVenda!.Substring(0, 4))).Max();
            else
                lastId = 0;
        }

        public void PrintSale()
        {
            foreach (var s in Sales)
                Console.WriteLine(s);
        }

        public Sale FindSale()
        {
            Console.WriteLine("Informe o ID da venda");
            string i = Console.ReadLine()!;
            var vendaEncontrada = Sales.Find(j => j.IdVenda == i);
            Console.WriteLine(vendaEncontrada);
            return vendaEncontrada!;
        }

        public Sale UpdateSale()
        {
            Sale vendaNova = FindSale();
            Console.WriteLine("Informe a data correta da compra (dd/mm/yyyy)");
            vendaNova.DataVenda = DateOnly.Parse(Console.ReadLine()!);
            SaveFile();
            return vendaNova;
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);
            foreach (var s in Sales)
            {
                string idFormatado = s.IdVenda!.PadLeft(5);
                string dataVendaFormadatada = s.DataVenda!.ToString("ddMMyyyy");
                string clienteCPFFormatado = s.ClienteCPF!.PadRight(11);
                string valorTotalFormatado = s.ValorTotal.ToString("00000000.00", CultureInfo.InvariantCulture);
                string dadoFinal = idFormatado + dataVendaFormadatada + clienteCPFFormatado + valorTotalFormatado;
                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        public void SaleMenu()
        {
            int op;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("===== MENU VENDA =====");
                Console.ResetColor();

                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar nova venda: ");
                Console.WriteLine("2 - Encontrar venda: ");
                Console.WriteLine("3 - Alterar venda: ");
                Console.WriteLine("4 - Imprimir todas as vendas: ");
                Console.WriteLine("5 - Mostrar Relatorio de venda: ");
                Console.WriteLine("6 - Mostrar Relatorio de compra ");
                Console.WriteLine("7 - Sair");
                op = int.Parse(Console.ReadLine()!);

                switch (op)
                {
                    case 1:
                        CreateSale();
                        break;
                    case 2:
                        FindSale();
                        break;
                    case 3:
                        UpdateSale();
                        break;
                    case 4:
                        PrintSale();
                        break;
                    case 5:
                        BuscarVendas();
                        break;
                    case 6:
                        RelatorioVendaMedicamentos();
                        break;
                    case 7:
                        SaveFile();
                        return;
                    default:
                        Console.WriteLine("Informe uma opção válida.");
                        break;
                }
            } while (true);
        }

        public override string? ToString()
        {
            return " --------------------------------------------------------------------------------- \n " +
            $"ID: {IdVenda}, Data da Venda: {DataVenda}, CPF do Cliente: {ClienteCPF}, Valor total: {ValorTotal} ";
        }







        public void BuscarVendas()
        {
            DateOnly variavel;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Digite o mês e ano (MM/yyyy): ");
                Console.ResetColor();
                string? info = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(info))
                {
                    Console.WriteLine("Entrada vazia. Tente novamente.");
                    continue;
                }

                if (DateOnly.TryParseExact("01/" + info, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out variavel))
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Mês inválido. Use o formato MM/yyyy (ex: 10/2025).");
                Console.ResetColor();
            }

            var vendasDoMes = Sales
                .Where(x => x.DataVenda.Month == variavel.Month && x.DataVenda.Year == variavel.Year)
                .ToList();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"===== RELATÓRIO DE VENDAS — {variavel.Month:D2}/{variavel.Year} =====");
            Console.ResetColor();

            if (vendasDoMes.Count == 0)
            {
                Console.WriteLine("\nNenhuma venda encontrada nesse período.");
            }
            else
            {
                decimal totalPeriodo = 0;
                foreach (var venda in vendasDoMes)
                {
                    Console.WriteLine(venda);
                    totalPeriodo += venda.ValorTotal;
                }

                Console.WriteLine("----------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Total vendido no período: R$ {totalPeriodo:F2}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }






        public void RelatorioVendaMedicamentos()
        {
            string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
            string file = "SaleItens.data"; // Arquivo que deve conter CDB e quantidade
            string fullPath = Path.Combine(diretorio, file);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("Arquivo de vendas de medicamentos (SalesItems.data) não encontrado!");
                return;
            }


            Dictionary<string, int> vendasPorMedicamento = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(fullPath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length < 13) continue;
                    string cdb = line.Substring(0, 13).Trim();

                    if (!vendasPorMedicamento.ContainsKey(cdb))
                        vendasPorMedicamento[cdb] = 0;

                    vendasPorMedicamento[cdb]++;
                }
            }

            // Agora, buscar os nomes dos medicamentos pelo arquivo Medicine.data
            string fileMedicines = Path.Combine(diretorio, "Medicine.data");
            Dictionary<string, string> nomesMedicamentos = new Dictionary<string, string>();
            if (File.Exists(fileMedicines))
            {
                using (StreamReader sr = new StreamReader(fileMedicines))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length < 53) continue;
                        string cdb = line.Substring(0, 13).Trim();
                        string name = line.Substring(13, 40).Trim(); ;
                    }
                }
            }

            var relatorio = vendasPorMedicamento
                .Select(v => new
                {
                    Cdb = v.Key,
                    Quantidade = v.Value
                })
                .OrderByDescending(x => x.Quantidade)
                .ToList();

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine("    Relatório de Medicamentos Mais Vendidos    ");
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine();
            Console.WriteLine("   CDB\t\tQuantidade");
            Console.WriteLine("---------------------------------------------");

            foreach (var med in relatorio)
            {
                Console.WriteLine($"{med.Cdb,-15}{med.Quantidade}");
            }

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"Total de medicamentos listados: {relatorio.Count}");
        }


    }
}
