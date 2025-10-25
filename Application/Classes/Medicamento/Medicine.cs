using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Classes.Medicamento
{
    internal class Medicine
    {
        Writer_Reader objeto = new Writer_Reader();

        public List<Medicine> medicines = new List<Medicine>();

        public string? Cdb { get; private set; }
        public string? Nome { get; private set; }
        public char Categoria { get; private set; }
        public decimal ValorVenda { get; private set; }
        public DateOnly UltimaVenda { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char situacao { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Medicine.data";
        string fullPath = Path.Combine(diretorio, file);

        public Medicine()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine($"Arquivo {file} e diretório criados com sucesso.");
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
        
        public void VerificarCategoria()
        {
            Console.Write("Informe a categoria do ingrediente: (A - Analgésico, B - Antibiótico, I - Anti-inflamatório, V - Vitamina): ");

            char categoria;

            do
            {
                 categoria = char.Parse(Console.ReadLine()!);

                if (categoria != 'A' && categoria != 'B' && categoria != 'I' && categoria != 'V')
                    Console.WriteLine("Situação inválida, tente novamente.");

            } while (categoria != 'A' && categoria != 'B' && categoria != 'I' && categoria != 'V');
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
                decimal valorVenda = decimal.Parse(line.Substring(54, 7).Trim());
                DateOnly ultimaVenda = DateOnly.ParseExact(line.Substring(61, 8), "ddMMyyyy");
                DateOnly dataCadastro = DateOnly.ParseExact(line.Substring(69, 8), "ddMMyyyy");
                char situacao = line[77];

                Medicine med = new Medicine(cdb, nome, categoria, valorVenda, ultimaVenda, dataCadastro, situacao);
                medicines.Add(med);
            }
            sr.Close();
        }
    }
}
