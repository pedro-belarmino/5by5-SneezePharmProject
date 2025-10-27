using System;
using System.Collections.Generic;
using System.IO;
using Application.Utils.WritersAndReaders;
using Application.Classes.Medicamento;

namespace Application.Classes.Production
{
    public class Produce
    {
        Writer_Reader objeto = new();

        public static List<Produce> Produces = new List<Produce>();

        public string? Id { get; private set; }
        public DateOnly DataProducao { get; private set; }
        public string? MedicineCdb { get; private set; }
        public int Quantidade { get; private set; }
        private int lastId = 0;

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Produce.data";
        static string fullPath = Path.Combine(diretorio, file);

        public Produce()
        {
            objeto.Verificador(diretorio, fullPath);
            PopularLista();
        }

        public Produce(string id, DateOnly dataProducao, string medicineCdb, int quantidade)
        {
            Id = id;
            MedicineCdb = medicineCdb;
            Quantidade = quantidade;
            DataProducao = dataProducao;
        }

        private void PopularLista()
        {
            StreamReader sr = new(fullPath);

            string line;

            while ((line = sr.ReadLine()!) != null)
            {
                string id = line[..5].Trim();
                DateOnly dataProducao = DateOnly.ParseExact(line.Substring(5, 8), "ddMMyyyy");
                string medicineCdb = line.Substring(13, 13).Trim();
                int quantidade = int.Parse(line.Substring(26, 3).Trim());

                Produce prod = new(id, dataProducao, medicineCdb, quantidade);
                Produces.Add(prod);
            }
            sr.Close();

            if (Produces.Count > 0)
                lastId = Produces.Select(x => int.Parse(x.Id![..5])).Max();
            else
            {
                lastId = 0;
            }
        }

        private void SaveFile()
        {
            StreamWriter writer = new(fullPath);

            foreach (var produce in Produces)
            {
                string idFormatado = int.Parse(produce.Id!).ToString("D5");

                string dataproducaoFormatado = produce.DataProducao.ToString("ddMMyyyy");

                string medicineCdbFormatado = produce.MedicineCdb!.PadRight(13);

                string qtdFormatado = produce.Quantidade.ToString("D3");

                string dadoFinal = idFormatado + dataproducaoFormatado + medicineCdbFormatado + qtdFormatado;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        private void CreateProduce()
        {
            lastId++;
            string id = lastId.ToString("D5");
            Console.WriteLine($"ID: {lastId}");
            Console.WriteLine();

            DateOnly dataProducao = DateOnly.FromDateTime(DateTime.Today);
            Console.WriteLine($"Data de produção: {dataProducao}");
            Console.WriteLine();

            Console.WriteLine("--- LISTA DE MEDICAMENTOS ---");
            Medicine.medicines.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("Informe o CDB do medicamento a ser produzido: ");
            string medicineCdb = Console.ReadLine()!;
            while (!VerificaCDB(medicineCdb))
            {
                Console.WriteLine("CDB inexistente ou Inativo, tente novamente.");
                medicineCdb = Console.ReadLine()!;
            }
            Console.WriteLine();

            var produceExistente = Produces.Find(x => x.MedicineCdb == medicineCdb);

            Console.WriteLine("Informe a quantidade que será fabricada (min 1, max 999): ");
            int qtd = int.Parse(Console.ReadLine()!);
            while (!VerificaQuantidade(qtd))
            {
                Console.WriteLine("Informe uma quantidade válida a ser fabricada: ");
                qtd = int.Parse(Console.ReadLine()!);
            }
            Console.WriteLine();

            if (produceExistente != null)
            {
                Console.WriteLine("Produto já existente, nova produção efetuada.");
            }
            else
            {
                Console.WriteLine("Nova produção registrada.");
            }

            Produce newProduce = new(id, dataProducao, medicineCdb, qtd);

            Produces.Add(newProduce);

            SaveFile();
        }

        private void PrintProduces()
        {
            foreach (var produce in Produces)
            {
                Console.WriteLine(produce);
            }
        }

        private Produce? FindProduce()
        {
            Console.Write("Informe o ID do Produce a ser encontrado: ");
            string variavel = Console.ReadLine()!;

            var produceMexido = Produces.Find(x => x.Id == variavel);

            if (produceMexido == null)
            {
                Console.WriteLine("Produce não encontrado!");
                return null;
            }

            Console.WriteLine(produceMexido);
            return produceMexido;
        }

        private Produce? UpdateProduce()
        {
            var UpdatedProduce = FindProduce();

            if (UpdatedProduce == null)
            {
                Console.WriteLine("Não foi possivel atualizar - Produce não encontrado.");
                return null;
            }

            Console.Write("informe a nova quantidade fabricada: ");
            int novaQtd;
            while (!int.TryParse(Console.ReadLine(), out novaQtd) || !VerificaQuantidade(novaQtd))
            {
                Console.WriteLine("Quantidade inválida, tente novamente.");
            }

            UpdatedProduce.Quantidade = novaQtd;

            SaveFile();

            return UpdatedProduce;
        }

        private static bool VerificaQuantidade(int qtd)
        {
            if (qtd <= 0 || qtd >= 1000)
                return false;
            return true;
        }

        public bool VerificaCDB(string cdb)
        {
            var memedio = Medicine.medicines.Find(x => x.Cdb == cdb && x.situacao == 'A');
            return memedio != null;
        }

        public override string? ToString()
        {
            string idFormatado = Id != null ? Id.PadLeft(5, '0') : "0000";
            return $"ID: {idFormatado}, Data de produção: {DataProducao}, CDB correspondente: {MedicineCdb}, Quantidade fabricada: {Quantidade:D3}";
        }

        public void ProduceMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo Produce: ");
                Console.WriteLine("2 - Encontrar algum Produce: ");
                Console.WriteLine("3 - Alterar algum Produce existente: ");
                Console.WriteLine("4 - Imprimir todos os Produces: ");
                Console.WriteLine("5 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        CreateProduce();
                        break;
                    case 2:
                        FindProduce();
                        break;
                    case 3:
                        UpdateProduce(); /*OK*/
                        break;
                    case 4:
                        PrintProduces();
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
