using Application.Classes.Production;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Application.Classes.Medicamento
{
    public class Medicine
    {
        Writer_Reader objeto = new Writer_Reader();

        public static List<Medicine> medicines = [];

        public string? Cdb { get; private set; }
        public string? Nome { get; private set; }
        public char Categoria { get; private set; }
        public decimal ValorVenda { get; private set; }
        public DateOnly UltimaVenda { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char situacao { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Medicine.data";
        static string fullPath = Path.Combine(diretorio, file);

        public Medicine()
        {
            objeto.Verificador(diretorio, fullPath);
            PopularListaMedicine();
        }

        public Medicine(string? cdb, string? nome, char categoria, decimal valorVenda, DateOnly ultimaVenda, DateOnly dataCadastro, char situacao)
        {
            Cdb = cdb;
            Nome = nome;
            Categoria = categoria;
            ValorVenda = valorVenda;
            UltimaVenda = ultimaVenda;
            DataCadastro = dataCadastro;
            this.situacao = situacao;
        }

        public bool VerificarCategoria(char categoria)
        {
            if (categoria != 'A' && categoria != 'B' && categoria != 'I' && categoria != 'V')
            {
                return false;
            }
            return true;
        }

        public bool VerificaNome(string nome)
        {
            foreach (char letra in nome)
            {
                if (!char.IsLetterOrDigit(letra))
                {
                    return false;
                }
            }
            return true;
        }

        public bool VerificaValorVenda(decimal valorVenda)
        {
            if (valorVenda <= 0 || valorVenda >= 10000)
            {
                return false;
            }
            return true;
        }

        public bool VerificaSituacao(char situacao)
        {
            if (situacao != 'A' && situacao != 'I')
                return false;
            return true;
        }

        public void PopularListaMedicine()
        {
            StreamReader sr = new StreamReader(fullPath);

            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                string cdb = line.Substring(0, 13).Trim();
                string nome = line.Substring(13, 40).Trim();
                char categoria = line[53];

                decimal valorVenda = decimal.Parse(line.Substring(54, 7)) / 100m;

                DateOnly ultimaVenda = DateOnly.ParseExact(line.Substring(61, 8), "ddMMyyyy");
                DateOnly dataCadastro = DateOnly.ParseExact(line.Substring(69, 8), "ddMMyyyy");
                char situacao = line[77];

                Medicine med = new Medicine(cdb, nome, categoria, valorVenda, ultimaVenda, dataCadastro, situacao);
                medicines.Add(med);
            }
            sr.Close();
        }

        public void SaveFile()
        {
            StreamWriter writer = new StreamWriter(fullPath);

            foreach (var medicine in medicines)
            {
                string cdbFormatado = medicine.Cdb!.PadRight(13);
                string nomeFormatado = medicine.Nome!.PadRight(40);

                int valorInt = (int)(medicine.ValorVenda * 100);
                string valorVendaFormatado = valorInt.ToString("D7");

                string ultimaVendaFormatado = medicine.UltimaVenda.ToString("ddMMyyyy");
                string dataCadastroFormatado = medicine.DataCadastro.ToString("ddMMyyyy");

                string dadoFinal = cdbFormatado + nomeFormatado + medicine.Categoria + valorVendaFormatado + ultimaVendaFormatado + dataCadastroFormatado + medicine.situacao;

                writer.WriteLine(dadoFinal);
            }
            writer.Close();
        }

        public override string ToString()
        {
            return $"CDB: {Cdb}, Nome: {Nome}, Categoria: {Categoria}, Valor: R${ValorVenda:F2}, Última venda: {UltimaVenda:dd/MM/yyyy}, Data de cadastro: {DataCadastro:dd/MM/yyyy}, Situação: {situacao}";
        }

        public int CalcularDV(string codigoBase)
        {
            int somaImpares = 0;
            int somaPares = 0;

            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(codigoBase[i].ToString());
                if ((i + 1) % 2 != 0)
                {
                    somaImpares += digito;
                }
                else
                {
                    somaPares += digito;
                }
            }

            int somaTotal = somaImpares + (somaPares * 3);
            int modulo = somaTotal % 10;
            int dv = modulo == 0 ? 0 : 10 - modulo;

            return dv;
        }

        public string GeradorDeCDB(List<Medicine> medicines)
        {
            string? prefixo = "789";
            string? empresa = "1234";

            int novoCodigo;
            if (medicines.Count > 0)
            {
                novoCodigo = medicines.Where(m => !string.IsNullOrEmpty(m.Cdb) && m.Cdb.Length >= 12).Select(m => int.Parse(m.Cdb!.Substring(7, 5))).DefaultIfEmpty(0).Max() + 1;
            }
            else
                novoCodigo = 1;

            string produtoFormatado = novoCodigo.ToString("D5");

            string codigoBase = prefixo + empresa + produtoFormatado;

            int dv = CalcularDV(codigoBase);

            string CDBFinal = codigoBase + dv;

            return CDBFinal;
        }

        public DateOnly BuscarDataCadastro(List<Medicine> medicines)
        {
            DateOnly dataAgora = DateOnly.FromDateTime(DateTime.Today);

            var ultimoCadastro = medicines.OrderByDescending(x => x.DataCadastro).FirstOrDefault();

            return ultimoCadastro != null ? ultimoCadastro.DataCadastro : dataAgora;
        }

        public void CreateMedicine()
        {
            string? cdb = GeradorDeCDB(medicines);
            Console.WriteLine();

            Console.Write("Insira o nome do medicamento: ");
            string nome = Console.ReadLine()!;
            while (!VerificaNome(nome))
            {
                Console.WriteLine("Nome inválido, são permitidos apenas caracteres alfanuméricos. tente novamente. ");
                nome = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.Write("Informe a categoria do medicamento: (A - Analgésico, B - Antibiótico, I - Anti-inflamatório, V - Vitamina): ");
            string? entradaCat = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(entradaCat) || entradaCat.Length != 1 || !VerificarCategoria(char.ToUpper(entradaCat[0])))
            {
                Console.WriteLine("Categoria inválida, tente novamente.");
                entradaCat = Console.ReadLine();
            }

            char categoria = char.ToUpper(entradaCat[0]);

            Console.Write("Informe o valor de venda: ");
            decimal valorVenda = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
            while (!VerificaValorVenda(valorVenda))
            {
                Console.WriteLine("Valor inválido, o valor deve estar entre 0 e 9999.99");
                valorVenda = decimal.Parse(Console.ReadLine()!);
            }
            Console.WriteLine();

            Console.Write("Insira a data da última vendado Medicamento DD-MM-AAAA: ");
            DateOnly ultimaVenda = DateOnly.Parse(Console.ReadLine()!);
            Console.WriteLine();

            var dataCadastro = BuscarDataCadastro(medicines);
            Console.WriteLine();

            Console.Write("Informe a situação do ingrediente (A - Ativo, I - Inativo): ");
            string? entradaSit = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(entradaSit) || entradaSit.Length != 1 || (char.ToUpper(entradaSit[0]) != 'A' && char.ToUpper(entradaSit[0]) != 'I'))
            {
                Console.WriteLine("Situação inválida, tente novamente.");
                entradaSit = Console.ReadLine();
            }

            char situacao = char.ToUpper(entradaSit[0]);

            Medicine newMedicine = new Medicine(cdb, nome, categoria, valorVenda, ultimaVenda, dataCadastro, situacao);

            medicines.Add(newMedicine);

            SaveFile();
        }

        public Medicine? FindMedicine()
        {
            Console.Write("Informe o CDB do Medicamento a ser encontrado: ");
            string variavel = Console.ReadLine()!;
            while (!VerificarCDB(variavel))
            {
                Console.WriteLine("CDB inválido, tente novamente.");
                variavel = Console.ReadLine()!;
            }
            var medicineMexido = medicines.Find(x => x.Cdb == variavel);
            Console.WriteLine(medicineMexido);
            return medicineMexido;
        }

        public bool VerificarCDB(string cdb)
        {
            var memedio = medicines.Find(x => x.Cdb == cdb);
            return memedio != null;
        }

        public Medicine? UpdateMedicine()
        {
            Medicine UpdatedMedicine = FindMedicine()!;

            Console.Write("Insira o novo nome do medicamento: ");
            UpdatedMedicine.Nome = Console.ReadLine()!;
            while (!VerificaNome(UpdatedMedicine.Nome))
            {
                Console.WriteLine("Nome inválido, são permitidos apenas caracteres alfanuméricos. tente novamente. ");
                UpdatedMedicine.Nome = Console.ReadLine()!;
            }
            Console.WriteLine();

            Console.Write("Informe a nova categoria do medicamento: (A - Analgésico, B - Antibiótico, I - Anti-inflamatório, V - Vitamina): ");
            string? entradaNovaCat = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(entradaNovaCat) || entradaNovaCat.Length != 1 || !VerificarCategoria(char.ToUpper(entradaNovaCat[0])))
            {
                Console.WriteLine("Categoria inválida, tente novamente.");
                entradaNovaCat = Console.ReadLine();
            }

            UpdatedMedicine.Categoria = char.ToUpper(entradaNovaCat[0]);

            do
            {
                Console.Write("Informe a nova situação do Medicamento (A - Ativo, I - Inativo): ");
                string? entradaNovaSit = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entradaNovaSit) && entradaNovaSit.Length == 1 && (char.ToUpper(entradaNovaSit[0]) == 'A' || char.ToUpper(entradaNovaSit[0]) == 'I'))
                {
                    UpdatedMedicine.situacao = char.ToUpper(entradaNovaSit[0]);
                    break;
                }

                Console.WriteLine("Situação inválida, tente novamente.");

            } while (true);

            SaveFile();
            return UpdatedMedicine;


        }

        public void PrintMedicines()
        {
            foreach (var medicine in medicines)
                Console.WriteLine(medicine);
        }

        public void MedicineMenu()
        {
            int opcao;
            do
            {
                Console.WriteLine("Escolha uma opção: ");
                Console.WriteLine("1 - Criar novo Medicamento: ");
                Console.WriteLine("2 - Encontrar algum Medicamento: ");
                Console.WriteLine("3 - Alterar algum Medicamento existente: ");
                Console.WriteLine("4 - Imprimir todos os Medicamentos: ");
                Console.WriteLine("5 - Sair");
                opcao = int.Parse(Console.ReadLine()!);

                switch (opcao)
                {
                    case 1:
                        CreateMedicine();
                        break;
                    case 2:
                        FindMedicine();  
                        break;
                    case 3:
                        UpdateMedicine(); /*OK*/
                        break;
                    case 4:
                        PrintMedicines();
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
