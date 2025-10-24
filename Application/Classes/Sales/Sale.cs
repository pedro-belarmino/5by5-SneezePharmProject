using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Utils;
using Application.Utils.WritersAndReaders;

namespace Application.Classes.Sales
{
    public class Sale
    {
        // Propriedades da venda
        public int Id { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public decimal ValorTotal { get; private set; }
        public string CpfCliente { get; private set; }

        // Lista global de vendas
        public List<Sale> TodasAsVendas { get; private set; } = new();

        // Utils
        private CreateId generateId = new();
        private Writer_Reader objeto = new Writer_Reader();
        private string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        private string arquivo = "Sales.data";

        // Construtor
        public Sale()
        {
            string fullPath = Path.Combine(diretorio, arquivo);
            objeto.Verificador(diretorio, fullPath);

            // Carrega todas as vendas do arquivo
            TodasAsVendas = LerArquivoEColocarNaLista(diretorio, arquivo);
            Console.WriteLine($"✅ {TodasAsVendas.Count} vendas carregadas do arquivo.");
        }

        // Atualizar CPF
        public void UpdateSale(string novoCpf)
        {
            if (string.IsNullOrWhiteSpace(novoCpf))
            {
                Console.WriteLine("CPF inválido. A atualização não foi realizada.");
                return;
            }

            CpfCliente = novoCpf;
            Console.WriteLine($"CPF da venda {Id:D5} atualizado para: {CpfCliente}");
        }

        // Ler arquivo e converter para lista de Sales
        public List<Sale> LerArquivoEColocarNaLista(string diretorio, string nomeArquivo)
        {
            var caminho = objeto.Verificador(diretorio, nomeArquivo);
            var lista = new List<Sale>();

            if (!File.Exists(caminho))
                return lista;

            using (StreamReader sr = new StreamReader(caminho))
            {
                string? linha;
                while ((linha = sr.ReadLine()) != null)
                {
                    if (linha.Length < 24) continue;

                    string idStr = linha.Substring(0, 5);
                    string dataStr = linha.Substring(5, 8);
                    string cpfStr = linha.Substring(13, 11);
                    string valorStr = linha.Substring(24).Trim();

                    if (!int.TryParse(idStr, out int id)) continue;
                    int dia = int.Parse(dataStr.Substring(0, 2));
                    int mes = int.Parse(dataStr.Substring(2, 2));
                    int ano = int.Parse(dataStr.Substring(4, 4));
                    DateOnly dataVenda = new DateOnly(ano, mes, dia);
                    decimal.TryParse(valorStr, out decimal valorTotal);

                    Sale venda = new Sale
                    {
                        Id = id,
                        DataVenda = dataVenda,
                        ValorTotal = valorTotal,
                        CpfCliente = cpfStr
                    };

                    lista.Add(venda);
                }
            }

            return lista;
        }

        // Converte lista de Sales para strings no formato fixo
        public List<string> ConverterSalesParaStrings(List<Sale> listaDeVendas)
        {
            var listaFormatada = new List<string>();
            foreach (var venda in listaDeVendas)
            {
                string idStr = venda.Id.ToString("D5");
                string dataStr = venda.DataVenda.ToString("ddMMyyyy");
                string cpfStr = venda.CpfCliente.PadLeft(11, '0');
                string valorStr = venda.ValorTotal.ToString("00000.00");

                listaFormatada.Add($"{idStr}{dataStr}{cpfStr}{valorStr}");
            }
            return listaFormatada;
        }

        // Menu interativo
        public void Menu()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\n===== MENU DE VENDAS =====");
                Console.WriteLine("1 - Criar nova venda");
                Console.WriteLine("2 - Ler todas as vendas");
                Console.WriteLine("3 - Atualizar CPF de uma venda");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");
                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        // Criar nova venda
                        Console.Write("Data da venda (dd/mm/yyyy): ");
                        DateOnly dataVenda = DateOnly.Parse(Console.ReadLine()!);

                        Console.Write("Valor total: ");
                        decimal valor = decimal.Parse(Console.ReadLine()!);

                        Console.Write("CPF do cliente: ");
                        string cpf = Console.ReadLine()!;

                        // Criar novo ID
                        var listaString = ConverterSalesParaStrings(TodasAsVendas);
                        string novoId = generateId.Create(listaString);

                        Sale novaVenda = new Sale
                        {
                            Id = int.Parse(novoId),
                            DataVenda = dataVenda,
                            ValorTotal = valor,
                            CpfCliente = cpf
                        };

                        TodasAsVendas.Add(novaVenda);

                        // Salvar apenas a nova venda no final do arquivo
                        var novaLinha = ConverterSalesParaStrings(new List<Sale> { novaVenda });
                        File.AppendAllLines(Path.Combine(diretorio, arquivo), novaLinha);

                        Console.WriteLine("✅ Venda criada com sucesso!");
                        break;

                    case "2":
                        // Ler todas as vendas do arquivo
                        Console.WriteLine("\n📋 Lista de Vendas:");
                        var vendasLidas = LerArquivoEColocarNaLista(diretorio, arquivo);

                        if (vendasLidas.Count == 0)
                        {
                            Console.WriteLine("Nenhuma venda registrada.");
                        }
                        else
                        {
                            foreach (var v in vendasLidas)
                            {
                                Console.WriteLine($"ID: {v.Id:D5} | Data: {v.DataVenda:dd/MM/yyyy} | CPF: {v.CpfCliente} | Valor: R$ {v.ValorTotal:F2}");
                            }
                        }
                        break;

                    case "3":
                        // Atualizar CPF
                        Console.Write("Digite o ID da venda a atualizar: ");
                        int id = int.Parse(Console.ReadLine()!);

                        var venda = TodasAsVendas.FirstOrDefault(x => x.Id == id);
                        if (venda == null)
                        {
                            Console.WriteLine("⚠️ Venda não encontrada!");
                            break;
                        }

                        Console.Write("Novo CPF: ");
                        string novoCpf = Console.ReadLine()!;
                        venda.UpdateSale(novoCpf);

                        // Salvar toda a lista no arquivo
                        var listaStringsAtualizada = ConverterSalesParaStrings(TodasAsVendas);
                        File.WriteAllLines(Path.Combine(diretorio, arquivo), listaStringsAtualizada);

                        Console.WriteLine("✅ CPF atualizado e arquivo salvo!");
                        break;

                    case "0":
                        continuar = false;
                        Console.WriteLine("Encerrando o sistema...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
