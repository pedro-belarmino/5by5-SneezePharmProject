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
using Application.Classes.Medicamento;
using Application.Utils;
using Application.Utils.WritersAndReaders;

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
        RelatorioDeVendasPorPeriodo relatorioPorPeriodo = new();
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
                Console.WriteLine("5 - Mostrar Relatorio: ");
                Console.WriteLine("6 - Sair");
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
                        relatorioPorPeriodo.BuscarVendas();
                        break;
                    case 6:
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



        public void RelatorioVendaMedicamentos()
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine("         Relatório de Medicamentos Mais Vendidos         ");
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine();

            List<string> cdbs = new List<string>();

            Console.WriteLine("   CDB   \t    Nome    \tQuantidade\n");

            foreach (var v in Sales)
            {
                foreach (var item in v.RelatorioDeVendasPorCDB)
                {
                    if (string.IsNullOrEmpty(item.Cdb) || cdbs.Contains(item.Cdb))
                        continue;

                    int qtdd = 0;
                    foreach (var venda in Sales)
                    {
                        foreach (var m in venda.RelatorioDeVendasPorCDB)
                        {
                            if (m.Cdb == item.Cdb)
                                qtdd++;
                        }
                    }
                    Console.WriteLine($"{item.Cdb}\t{item.Nome}\t{qtdd}");
                    cdbs.Add(item.Cdb);
                }
            }
        }
    }
}
