using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Suppliers
{
    public class RestrictedSupplier
    {
        Writer_Reader objeto = new Writer_Reader();
        public static List<RestrictedSupplier> FornecedoresRestritos = new List<RestrictedSupplier>();
        public string Cnpj { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "RestructedSuppliers.data";
        string fullPath = Path.Combine(diretorio, file);


        public RestrictedSupplier()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
            PopularLista();
        }


        public RestrictedSupplier(string cnpj)
        {
            Cnpj = cnpj;
        }

        public void PopularLista()
        {
            StreamReader sr = new StreamReader(fullPath);

            string linha;
            while ((linha = sr.ReadLine()!) != null)
            {
                string cnpj = linha.Substring(0, 14).Trim();
                string razaoSocial = linha.Substring(14, 50).Trim();
                string pais = linha.Substring(64, 20).Trim();
                DateOnly abertura = DateOnly.ParseExact(linha.Substring(84, 8), "ddMMyyyy");

                string ultimoFornecimentoString = linha.Substring(92, 8).Trim();
                DateOnly ultimoFornecimento;

                if (string.IsNullOrEmpty(ultimoFornecimentoString))
                    ultimoFornecimento = DateOnly.MinValue;
                else
                {
                    if (!DateOnly.TryParseExact(ultimoFornecimentoString, "ddMMyyyy", out ultimoFornecimento))
                        ultimoFornecimento = DateOnly.MinValue;
                }
                DateOnly dataCadastro = DateOnly.ParseExact(linha.Substring(100, 8), "ddMMyyyy");
                char situacao = linha[108];

                RestrictedSupplier fornecedorRestrito = new RestrictedSupplier(cnpj, razaoSocial, pais, abertura, ultimoFornecimento, dataCadastro, situacao);

                FornecedoresRestritos.Add(fornecedorRestrito);
            }
            sr.Close();
        }


        private void SalvarLista()
        {
            StreamWriter sw = new StreamWriter(fullPath, false);

            foreach (RestrictedSupplier fornecedorRestrito in FornecedoresRestritos)
            {
                string ultimoFornecimentoString;
                if (fornecedorRestrito.UltimoFornecimento == DateOnly.MinValue)
                    ultimoFornecimentoString = "";
                else
                    ultimoFornecimentoString = fornecedorRestrito.UltimoFornecimento.ToString("ddMMyyyy");

                string linha = fornecedorRestrito.Cnpj.PadRight(14) +
                               fornecedorRestrito.RazaoSocial.PadRight(50) +
                               fornecedorRestrito.Pais.PadRight(20) +
                               fornecedorRestrito.DataAbertura.ToString("ddMMyyyy") +
                               ultimoFornecimentoString.PadRight(8) +
                               fornecedorRestrito.DataCadastro.ToString("ddMMyyyy") +
                               fornecedorRestrito.Situacao;

                sw.WriteLine(linha);
            }
            sw.Close();
        }








        private static Supplier FiltrarFornecedorCNPJRestrito()
        {
            Console.Write("Informe o CNPJ que deseja buscar: ");
            string cnpj = Console.ReadLine();

            Console.WriteLine(Supplier.Suppliers.Find(c => c.Cnpj == cnpj).ToString());

            return Supplier.Suppliers.Find(c => c.Cnpj == cnpj);
        }

        private static void CadastrarFornecedorRestrito()
        {
            Console.Write("Informe o CNPJ: ");
            string cnpj = Console.ReadLine();
            while (ValidarCnpj(cnpj) == false)
            {
                Console.Write("CNPJ inválido! Tente novamente ou [S] para sair: ");
                cnpj = Console.ReadLine().ToUpper();
                if (cnpj == "S")
                    return;
            }
            Console.Write("\nInforme a Razão Social: ");
            string razaoSocial = Console.ReadLine();
            while (ValidarRazaoSocial(razaoSocial) == false)
            {
                Console.Write("Informe novamente: ");
                razaoSocial = Console.ReadLine();
            }
            Console.Write("\nInforme o País: ");
            string pais = (Console.ReadLine());
            while (ValidarPais(pais) == false)
            {
                Console.Write("Informe novamente: ");
                pais = Console.ReadLine();
            }

            Console.Write("\nData de abertura: ");
            DateOnly dataAbertura = DateOnly.Parse(Console.ReadLine());
            while (ValidarData(dataAbertura) == false)
            {
                Console.Write("Informe novamente: ");
                dataAbertura = DateOnly.Parse(Console.ReadLine());
            }
            DateOnly? dataUltimoFornecimento = null;


            DateOnly dataCadastro = DateOnly.FromDateTime(DateTime.Now);
            Console.Write("\nData de cadastro: " + dataCadastro);
            Console.Write("\nSituação [A] Ativo [I] Inativo: ");
            char situacao = char.Parse(Console.ReadLine().ToString());
            while (ValidarSituacao(situacao) == false)
            {
                Console.Write("Informe novamente");
                situacao = char.Parse(Console.ReadLine());
            }

            Supplier Fornecedores = new Supplier(cnpj, razaoSocial, pais, dataAbertura, dataUltimoFornecimento, dataCadastro, situacao);
            Suppliers.Add(Fornecedores);
        }





        public static void MenuPrincipal()
        {
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
                Console.WriteLine(" |                       >      Fornecedores Restritos      <                     |");
                Console.WriteLine(" |--------------------------------------------------------------------------------|");
                Console.WriteLine(" | [ 1 ] Registro de Fornecedores Restritos  |  [ 2 ] Remover Fornecedor Restrito |");
                Console.WriteLine(" | [ 3 ] Listar Fornecedores                 |  [ 4 ] Filtrar Fornecedor Restrito |");
                Console.WriteLine(" | [ 5 ] Voltar                              |                                    |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine();
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        CadastrarFornecedor();
                        break;
                    case 2:
                        AtualizarFornecedor();
                        break;
                    case 3:
                        ListarFornecedor();
                        break;
                    case 4:
                        FiltrarFornecedorCNPJ();
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Opção Inválida. Tente novamente.");
                        break;
                }
                if (opcao == 5)
                    break;
                Console.WriteLine("\nPressione Enter para prosseguir");
                Console.ReadLine();
            } while (opcao != 5);
        }



    }
}
