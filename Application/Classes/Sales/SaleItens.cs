using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Application.Utils.WritersAndReaders;

namespace Application
{
    public class SaleItens
    {
        Writer_Reader objeto = new();

        public List<SaleItens> SaleItensList = new();
        public string Id { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int TotalItem { get; private set; }
        public string CDBMedicamento { get; private set; }

        private int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "SaleItens.data";
        string fullPath = Path.Combine(diretorio, file);

        public SaleItens()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório de itens de venda criados com sucesso.");
            Verificador();
        }

        public SaleItens(string i, int q, decimal v, string m, int t)
        {
            this.Id = i;
            this.Quantidade = q;
            this.ValorUnitario = v;
            this.CDBMedicamento = m;
            this.TotalItem = t;
        }

        public void CreateSaleItem()
        {
            lastId++;
            string ID = $"{lastId:D5}";
            Console.WriteLine();

            Console.WriteLine("Informe a quantidade do item:");
            int qtd = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Informe o valor unitário:");
            decimal valorUnit = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

            Console.WriteLine("Informe o código do medicamento:");
            string codMedicamento = Console.ReadLine()!;

            int totalItem = (int)(qtd * valorUnit);

            SaleItens newItem = new(ID, qtd, valorUnit, codMedicamento, totalItem);

            SaleItensList.Add(newItem);
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }

            PopularListaItens();
        }

        public void PopularListaItens()
        {
            StreamReader sr = new StreamReader(fullPath);
            string line;

            while ((line = sr.ReadLine()!) != null)
            {

                string id = line.Substring(0, 5).Trim();
                int qtd = int.Parse(line.Substring(5, 5).Trim());
                decimal valorUnitario = decimal.Parse(line.Substring(10, 10).Trim(), CultureInfo.InvariantCulture);
                string cdbMedicamento = line.Substring(20, 10).Trim();
                int totalItem = int.Parse(line.Substring(30, 10).Trim());

                SaleItens item = new SaleItens(id, qtd, valorUnitario, cdbMedicamento, totalItem);
                SaleItensList.Add(item);
            }
            sr.Close();

            if (SaleItensList.Count > 0)
                lastId = SaleItensList.Select(x => int.Parse(x.Id)).Max();
            else
                lastId = 0;
        }

        public void PrintSaleItens()
        {
            foreach (var s in SaleItensList)
                Console.WriteLine(s);
        }

        public SaleItens FindSaleItem()
        {
            Console.WriteLine("Informe o ID do item de venda:");
            string i = Console.ReadLine()!;
            var itemEncontrado = SaleItensList.Find(j => j.Id == i);
            Console.WriteLine(itemEncontrado);
            return itemEncontrado!;
        }

        public SaleItens UpdateSaleItem()
        {
            SaleItens itemAtual = FindSaleItem();

            Console.WriteLine("Informe a nova quantidade:");
            itemAtual.Quantidade = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Informe o novo valor unitário:");
            itemAtual.ValorUnitario = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

            itemAtual.TotalItem = (int)(itemAtual.Quantidade * itemAtual.ValorUnitario);

            SaveFile();
            return itemAtual;
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);
            foreach (var s in SaleItensList)
            {
                string idFormatado = s.Id.PadLeft(5);
                string qtdFormatada = s.Quantidade.ToString("D5");
                string valorUnitarioFormatado = s.ValorUnitario.ToString("0000000.00", CultureInfo.InvariantCulture).PadLeft(10);
                string cdbMedicamentoFormatado = s.CDBMedicamento.PadRight(10);
                string totalItemFormatado = s.TotalItem.ToString("D10");

                string linha = idFormatado + qtdFormatada + valorUnitarioFormatado + cdbMedicamentoFormatado + totalItemFormatado;

                writer.WriteLine(linha);
            }
            writer.Close();
        }

        public void SaleItensMenu()
        {
            int op;

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("##### MENU ITENS DE VENDA ####");
                Console.ResetColor();

                Console.WriteLine("1 - Criar novo item de venda");
                Console.WriteLine("2 - Encontrar item de venda");
                Console.WriteLine("3 - Atualizar item de venda");
                Console.WriteLine("4 - Imprimir todos os itens");
                Console.WriteLine("5 - Sair");

                op = int.Parse(Console.ReadLine()!);

                switch (op)
                {
                    case 1:
                        CreateSaleItem();
                        break;
                    case 2:
                        FindSaleItem();
                        break;
                    case 3:
                        UpdateSaleItem();
                        break;
                    case 4:
                        PrintSaleItens();
                        break;
                    case 5:
                        SaveFile();
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            } while (true);
        }

        public override string ToString()
        {
            return " --------------------------------------------------------------------------------- \n " +
                   $"ID: {Id}, Quantidade: {Quantidade}, Valor Unitário: {ValorUnitario}, CDB: {CDBMedicamento}, Total Item: {TotalItem}";
        }
    }
}
