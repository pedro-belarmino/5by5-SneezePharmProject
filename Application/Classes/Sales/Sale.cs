using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Application.Utils;
using Application.Utils.WritersAndReaders;

namespace Application.Classes.Sales
{
    public class Sale
    {


        Writer_Reader objeto = new Writer_Reader();
        public List<Sale> Sales = new List<Sale>();

        public string IdVenda { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public string ClienteCPF { get; private set; }
        public decimal ValorTotal { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Sales.data";
        string fullPath = Path.Combine(diretorio, file);

        private int lastId = 0;

        public Sale()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório da venda criados com sucesso.");
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
            string ID = $"{lastId:D4}";
            System.Console.WriteLine();


            System.Console.WriteLine("Insira a data da venda (dd/mm/yyyy)");
            DateOnly data = DateOnly.Parse(Console.ReadLine()!);

            System.Console.WriteLine("Insira o cpf do cliente");
            string cpf = Console.ReadLine()!;
            System.Console.WriteLine();

            System.Console.WriteLine("valor total");
            decimal vt = decimal.Parse(Console.ReadLine()!);

            Sale newSale = new(ID, data, cpf, vt);

            Sales.Add(newSale);
            SaveFile();
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
            PopularListaVendas();
        }

        public void PopularListaVendas()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                // Extrair partes da linha conforme posições
                string idVenda = line.Substring(0, 5).Trim();
                DateOnly dataVenda = DateOnly.ParseExact(line.Substring(5, 8), "ddMMyyyy");
                string clienteCPF = line.Substring(13, 11);


                decimal valorTotalDecimal = decimal.Parse(line.Substring(24, 11));
                decimal valorTotalDecimalComPonto = valorTotalDecimal / 100;

                // Criar objeto Sale
                Sale venda = new Sale(idVenda, dataVenda, clienteCPF, valorTotalDecimalComPonto);

                // Adicionar à lista
                Sales.Add(venda);
            }
            sr.Close();


            if (Sales.Count > 0)
                lastId = Sales.Select(x => int.Parse(x.IdVenda!.Substring(0, 4))).Max();
            else
            {
                lastId = 0;
            }

        }



        public void PrintSale() //printar
        {
            foreach (var s in Sales)
                Console.WriteLine(s);
        }


        public Sale FindSale()
        {
            System.Console.WriteLine("Informe o ID da venda");
            string i = Console.ReadLine()!;
            var vendaEncontrada = Sales.Find(j => j.IdVenda == i);
            System.Console.WriteLine(vendaEncontrada);
            return vendaEncontrada!;
        }


        public Sale UpdateSale()
        {
            Sale vendaNova = FindSale();

            System.Console.WriteLine("Informe a data correta da compra (dd/mm/yyyy)");
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
                System.Console.WriteLine("##### MENU VENDA ####");
                Console.ResetColor();

                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar nova venda: ");
                Console.WriteLine("2 - Encontrar venda: ");
                Console.WriteLine("3 - Alterar venda: ");
                Console.WriteLine("4 - Imprimir todas as vendas: ");
                Console.WriteLine("5 - Sair");
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
            $"ID: {IdVenda}, Data da Venda: {DataVenda}, CPF do Clinte: {ClienteCPF}, Valor total: {ValorTotal} ";
        }
    }
}
