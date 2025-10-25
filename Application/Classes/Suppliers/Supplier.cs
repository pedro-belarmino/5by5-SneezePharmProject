using Application.Classes.Production;
using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Supplier
    {
        Writer_Reader objeto = new Writer_Reader();
        public static List<Supplier> Suppliers  = new List<Supplier>();
        public string Cnpj { get; private set; }
        public string RazaoSocial { get; private set; }
        public string Pais { get; private set; }
        public DateOnly DataAbertura { get; private set; }
        public DateOnly? UltimoFornecimento { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }


        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Suppliers.data";
        string fullPath = Path.Combine(diretorio, file);


        public Supplier()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
        }


        public Supplier(string cnpj, string razaoSocial, string pais, DateOnly dataAbertura, DateOnly? ultimoFornecimento, DateOnly dataCadastro, char situacao)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Pais = pais;
            DataAbertura = dataAbertura;
            UltimoFornecimento = ultimoFornecimento;
            DataCadastro = dataCadastro;
        }

        private static bool TemEsteCNPJ(string cnpj)
        {
            return Suppliers.Exists(c => c.Cnpj == cnpj);
        }

        private static bool ValidarCnpj(string cnpj)
        {
            bool ehApenasNumero = cnpj.All(char.IsDigit);
            char[] letras = cnpj.ToCharArray();

            if (letras.Length != 14 || ehApenasNumero == false)
            {
                Console.WriteLine("Inválido! Tem que ser apenas número e conter 14 digitos.");
                return false;
            }

            if (TemEsteCNPJ(cnpj))
            {
                Console.WriteLine("Inválido! Este CNPJ já foi cadastrado.");
                return false;
            }

            int aux1, multiplicador1, soma1 = 0, dv1inicio = 5, dvfim1 = 9;
            int aux2, dv2inicio = 6, soma2 = 0, multiplicador2, dvfim2 = 9;
            bool valido = false;

            for (int i = 0; i < 12; i++)
            {
                aux1 = int.Parse(letras[i].ToString());

                if (i < 4)
                {
                    multiplicador1 = aux1 * dv1inicio;
                    soma1 += multiplicador1;
                    dv1inicio--;
                }
                else
                {
                    multiplicador1 = aux1 * dvfim1;
                    soma1 += multiplicador1;
                    dvfim1--;
                }
            }

            int restoDV1 = soma1 % 11;
            int vd1;

            if (restoDV1 < 2)
                vd1 = 0;
            else
                vd1 = 11 - restoDV1;


            if (int.Parse(letras[12].ToString()) == vd1)
            {
                for (int i = 0; i < 13; i++)
                {
                    aux2 = int.Parse(letras[i].ToString());

                    if (i < 5)
                    {
                        multiplicador2 = aux2 * dv2inicio;
                        soma2 += multiplicador2;
                        dv2inicio--;
                    }
                    else
                    {
                        multiplicador2 = aux2 * dvfim2;
                        soma2 += multiplicador2;
                        dvfim2--;
                    }
                }

                int restoDV2 = soma2 % 11;
                int vd2;

                if (restoDV2 < 2)
                    vd2 = 0;
                else
                    vd2 = 11 - restoDV2;

                if (int.Parse(letras[13].ToString()) == vd2)
                {
                    valido = true;
                    return valido;
                }
                else
                {
                    valido = false;
                    return valido;
                }
            }
            else
                return valido;
        }

        private static bool ValidarRazaoSocial(string nomeRazaoSocial)
        {
            if (string.IsNullOrEmpty(nomeRazaoSocial))
            {
                Console.WriteLine("A entrada não pode ser vazia");
                return false;
            }
            else if (nomeRazaoSocial.Length > 50)
            {
                Console.WriteLine($"Limite de caracteres atingido! Use até 50 caracteres.");
                return false;
            }
            else
            {
                nomeRazaoSocial.PadRight(50);
                return true;
            }
        }

        private static bool ValidarPais(string pais)
        {
            if (string.IsNullOrEmpty(pais))
            {
                Console.WriteLine("A entrada não pode ser vazia");
                return false;
            }
            else if (pais.Length > 20)
            {
                Console.WriteLine($"Limite de caracteres atingido! Use até 50 caracteres.");
                return false;
            }
            else
            {
                pais.PadRight(20);
                return true;
            }
        }

        private static bool ValidarSituacao(char situacao)
        {
            if (situacao is not 'A' && situacao is not 'I')
            {
                Console.WriteLine("Inválido! Informe apenas [A] ou [I]");
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool ValidarData(DateOnly data)
        {
            if (data > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("A data informada não pode ser maior que a data atual.");
                return false;
            }
            return true;
        }

        private static void CadastrarFornecedor()
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
            DateOnly dataTemp;
            bool conversor = DateOnly.TryParse(Console.ReadLine(), out dataTemp);
            while (!conversor || ValidarData(dataTemp) == false)
            {
                Console.Write("Data inválida. Informe novamente: ");
                conversor = DateOnly.TryParse(Console.ReadLine(), out dataTemp);
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

            Supplier Fornecedores = new Supplier(cnpj, razaoSocial, pais, dataTemp, dataUltimoFornecimento, dataCadastro, situacao);
            Suppliers.Add(Fornecedores);
        }

        private static void ListarFornecedor()
        {
            foreach (var fornecedor in Suppliers)
            {
                Console.WriteLine(fornecedor.ToString());
            }
        }

        private static Supplier FiltrarFornecedorCNPJ()
        {
            Console.Write("Informe o CNPJ que deseja buscar: ");
            string cnpj = Console.ReadLine();

            Console.WriteLine(Supplier.Suppliers.Find(c => c.Cnpj == cnpj).ToString());

            return Supplier.Suppliers.Find(c => c.Cnpj == cnpj);
        }

        public override string ToString()
        {
            return $"\nCNPJ: {Cnpj}" +
                    $"\nRazão Social: {RazaoSocial}" +
                    $"\nPaís: {Pais}" +
                    $"\nData de Abertura: {DataAbertura}" +
                    $"\nData do última fornecimento: {UltimoFornecimento}" +
                    $"\nData de cadastro: {DataCadastro}" +
                    $"\nSituação: {Situacao}";
        }

        private static void AtualizarFornecedor()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |              >      Fornecedor      <             |");
                Console.WriteLine(" |---------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Alterar Situação  |  [ 2 ] Razão Social    |");
                Console.WriteLine(" |  [ 3 ] Voltar            |                        |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine();
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpj = Console.ReadLine();

                        if (TemEsteCNPJ(cnpj) == true)
                        {
                            Console.WriteLine(Supplier.Suppliers.Find(c => c.Cnpj == cnpj).ToString());
                            var buscaSituacao = Supplier.Suppliers.Find(c => c.Cnpj == cnpj);
                            Console.Write("\nInforme a situação atualizado [A] Ativo [I] Inativo: ");
                            char situacao = char.Parse(Console.ReadLine().ToString());
                            while (ValidarSituacao(situacao) == false)
                            {
                                Console.Write("Informe novamente");
                                situacao = char.Parse(Console.ReadLine());
                            }
                            buscaSituacao.Situacao = situacao;
                            Console.WriteLine("\nA Situação foi atualizado com sucesso!");
                            break;
                        }
                        Console.WriteLine("Inválido! CNPJ não encontrado");
                        break;
                    case 2:
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpjr = Console.ReadLine();

                        if (TemEsteCNPJ(cnpjr) == true)
                        {
                            Console.WriteLine(Supplier.Suppliers.Find(c => c.Cnpj == cnpjr).ToString());
                            var buscaRazaoSocial = Supplier.Suppliers.Find(c => c.Cnpj == cnpjr);
                            Console.Write("\nInforme a Razão Social: ");
                            string razaoSocial = Console.ReadLine();
                            while (ValidarRazaoSocial(razaoSocial) == false)
                            {
                                Console.Write("Informe novamente: ");
                                razaoSocial = Console.ReadLine();
                            }
                            buscaRazaoSocial.RazaoSocial = razaoSocial;
                            Console.WriteLine("\nA Razão Social foi atualizado com sucesso!");
                            break;
                        }
                        Console.WriteLine("Inválido! CNPJ não encontrado");
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Opção Inválida. Tente novamente.");
                        break;
                }
                if (opcao == 3)
                    break;
                Console.Write("\nPressione Enter para prosseguir ");
                Console.ReadLine();
            } while (opcao != 3);
        }


        public static void MenuPrincipal()
        {
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |                   >      Fornecedor      <                  |");
                Console.WriteLine(" |-------------------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Cadastrar Fornecedor  |  [ 2 ] Atualizar Fornecedor  |");
                Console.WriteLine(" |  [ 3 ] Listar Fornecedores   |  [ 4 ] Filtrar Fornecedor    |");
                Console.WriteLine(" |  [ 5 ] Voltar                |                              |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
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
