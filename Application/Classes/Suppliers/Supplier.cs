using Application.Classes.Production;
using Application.Classes.Suppliers;
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
        Writer_Reader objeto = new Writer_Reader();     // objeto para criar o arquivo
        public static List<Supplier> Suppliers = new List<Supplier>();
        public string Cnpj { get; private set; }
        public string RazaoSocial { get; private set; }
        public string Pais { get; private set; }
        public DateOnly DataAbertura { get; private set; }
        public DateOnly UltimoFornecimento { get; set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }


        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Suppliers.data";
        string fullPath = Path.Combine(diretorio, file);

        // Método Construtor: Criando o arquivo (caso não exista), e popularizando conforme os parâmetos, e tamanhos determinados
        public Supplier()
        {
            objeto.Verificador(diretorio, fullPath);
            PopularLista();
        }


        // Método Construtor > com parâmetros
        public Supplier(string cnpj, string razaoSocial, string pais, DateOnly dataAbertura, DateOnly ultimoFornecimento, DateOnly dataCadastro, char situacao)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Pais = pais;
            DataAbertura = dataAbertura;
            UltimoFornecimento = ultimoFornecimento;
            DataCadastro = dataCadastro;
            Situacao = situacao;
        }


        // Método: Popular a lista (Leiutura do arquivo) > aplicando os parâmetros do Fornecedor, e tamanhos pré-estipulados + formato de data
        public void PopularLista()
        {
            Suppliers.Clear();
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

                Supplier fornecedor = new Supplier(cnpj, razaoSocial, pais, abertura, ultimoFornecimento, dataCadastro, situacao);
                Suppliers.Add(fornecedor);
            }
            sr.Close();
        }


        // Método: Salvar a lista (Escrita do arquivo) > aplicando os parâmetros do Fornecedor, e tamanhos pré-estipulados
        private void SalvarLista()
        {
            StreamWriter sw = new StreamWriter(fullPath, false);

            foreach (Supplier fornecedor in Suppliers)
            {
                string ultimoFornecimentoString;
                if (fornecedor.UltimoFornecimento == DateOnly.MinValue)
                    ultimoFornecimentoString = "";
                else
                    ultimoFornecimentoString = fornecedor.UltimoFornecimento.ToString("ddMMyyyy");

                string linha = fornecedor.Cnpj.PadRight(14) +
                               fornecedor.RazaoSocial.PadRight(50) +
                               fornecedor.Pais.PadRight(20) +
                               fornecedor.DataAbertura.ToString("ddMMyyyy") +
                               ultimoFornecimentoString.PadRight(8) +
                               fornecedor.DataCadastro.ToString("ddMMyyyy") +
                               fornecedor.Situacao;

                sw.WriteLine(linha);
            }
            sw.Close();
        }


        // Método: Verificar se existe algum CNPJ com base ao parâmetro
        public static bool TemEsteCNPJ(string cnpj)
        {
            return Suppliers.Exists(c => c.Cnpj == cnpj);
        }


        // Método: Validar o CNPJ
        public static bool ValidarCnpj(string cnpj)
        {
            bool ehApenasNumero = cnpj.All(char.IsDigit);
            char[] letras = cnpj.ToCharArray();

            if (letras.Length != 14 || ehApenasNumero == false)
            {
                Console.WriteLine("\nInválido! Tem que ser apenas número e conter 14 digitos.");
                return false;
            }

            if (TemEsteCNPJ(cnpj))
            {
                Console.WriteLine("\nInválido! Este CNPJ já foi cadastrado.");
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
                    Console.WriteLine("\nCNPJ inválido!");
                    return valido;
                }
            }
            else
                return valido;
        }


        // Método: Validar Razão Social   |     Retorna false se: contém acima de 50 digitos; ou se for nulo
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
                return true;
        }


        // Método: Validar País   |     Retorna false se: contém acima de 20 digitos; ou se for nulo
        private static bool ValidarPais(string pais)
        {
            if (string.IsNullOrEmpty(pais))
            {
                Console.WriteLine("A entrada não pode ser vazia");
                return false;
            }
            else if (pais.Length > 20)
            {
                Console.WriteLine($"Limite de caracteres atingido! Use até 20 caracteres.");
                return false;
            }
            else
                return true;
        }


        // Método: Validar Situação   |     Retorna false se não for igual a 'A' ou 'i'
        private static bool ValidarSituacao(char situacao)
        {
            if (situacao is not 'A' && situacao is not 'I')
            {
                Console.Write("\nInválido! Informe apenas [A] ou [I]: ");
                return false;
            }
            else
            {
                return true;
            }
        }


        // Método: Validar data   |     Retorna false se a data for maior que a data atual
        private static bool ValidarData(DateOnly data)
        {
            if (data > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("A data informada não pode ser maior que a data atual.");
                return false;
            }
            return true;
        }


        // Método: Cadastrar fornecedor
        private static void CadastrarFornecedor()
        {
            Console.Write("Informe o CNPJ: ");
            string cnpj = Console.ReadLine()!;
            while (ValidarCnpj(cnpj) == false)
            {
                Console.Write("Digite [S] para sair ou tente novamente: ");
                cnpj = Console.ReadLine()!.ToUpper();
                if (cnpj == "S")
                    return;
            }

            Console.Write("\nInforme a Razão Social: ");
            string razaoSocial = Console.ReadLine()!;
            while (ValidarRazaoSocial(razaoSocial!) == false)
            {
                Console.Write("Tente novamente: ");
                razaoSocial = Console.ReadLine()!;
            }

            Console.Write("\nInforme o País: ");
            string pais = (Console.ReadLine()!);
            while (ValidarPais(pais!) == false)
            {
                Console.Write("Tente novamente: ");
                pais = Console.ReadLine()!;
            }

            Console.Write("\nData de abertura: ");
            DateOnly dataAbertura;
            bool conversor = DateOnly.TryParse(Console.ReadLine(), out dataAbertura);
            while (!conversor || ValidarData(dataAbertura) == false)
            {
                Console.Write("Data inválida. Informe novamente: ");
                conversor = DateOnly.TryParse(Console.ReadLine(), out dataAbertura);
            }

            DateOnly dataCadastro = DateOnly.FromDateTime(DateTime.Now);
            Console.Write("\nData de cadastro: " + dataCadastro);

            Console.Write("\n\nSituação [A] Ativo [I] Inativo: ");
            string entSt = Console.ReadLine()!;
            bool aux = char.TryParse(entSt, out char situacao);
            while (ValidarSituacao(situacao) == false)
            {
                entSt = Console.ReadLine()!;
                aux = char.TryParse(entSt, out situacao);
            }

            Supplier fornecedor = new Supplier(cnpj, razaoSocial, pais, dataAbertura, DateOnly.MinValue, dataCadastro, situacao);
            Suppliers.Add(fornecedor);
            fornecedor.SalvarLista();

            Console.WriteLine("\nFornecedor cadastrado com sucesso!");
            Console.Write("\nPressione Enter para prosseguir ");
            Console.ReadLine();
        }


        // Método: Imprimir lista       | Retorna uma mensagem caso a lista esteja zerada
        public static void ListarFornecedor()
        {
            if (Suppliers.Count == 0)
                Console.WriteLine("Não há fornecedores cadastrados na lista");

            foreach (var fornecedor in Suppliers)
            {
                Console.WriteLine(fornecedor.ToString());
            }
        }


        // Método: Busca o fornecedor pelo CNPJ, e retorna ele
        public static Supplier FiltrarFornecedorCNPJ(string cnpj)
        {
            return Suppliers.Find(c => c.Cnpj == cnpj);
        }


        // Método: Modelo de impressão dos dados do Fornecedor
        public override string ToString()
        {
            string ultimoFornecimentoString;

            if (UltimoFornecimento == DateOnly.MinValue)
                ultimoFornecimentoString = "Não há fornecimento ainda";
            else
                ultimoFornecimentoString = UltimoFornecimento.ToString("dd/MM/yyyy");

            return $"\nCNPJ: {Cnpj}" +
                    $"\nRazão Social: {RazaoSocial}" +
                    $"\nPaís: {Pais}" +
                    $"\nData de Abertura: {DataAbertura:dd/MM/yyyy}" +
                    $"\nData do última fornecimento: {ultimoFornecimentoString}" +
                    $"\nData de cadastro: {DataCadastro:dd/MM/yyyy}" +
                    $"\nSituação: {Situacao}";
        }


        // Método: Atualizar Fornecedor           | Opções: Situação e Razão Social
        public void AtualizarFornecedor()
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
                string entrada = Console.ReadLine()!;
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpj = Console.ReadLine()!;
                        var fornec = FiltrarFornecedorCNPJ(cnpj)!;
                        if (fornec is not null)
                        {
                            Console.Write("\nInforme a situação atualizado [A] Ativo [I] Inativo: ");
                            string entSt = Console.ReadLine()!;
                            bool aux = char.TryParse(entSt, out char situacao);
                            while (ValidarSituacao(situacao) == false)
                            {
                                Console.Write("Tente novamente: ");
                                entrada = Console.ReadLine()!;
                                aux = char.TryParse(entrada, out situacao);
                            }
                            fornec.Situacao = situacao;
                            Console.WriteLine("\nSituação atualizada com sucesso!");
                            fornec.SalvarLista();
                            break;
                        }
                        Console.WriteLine("\nCNPJ não encontrado!");
                        break;
                    case 2:
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpjR = Console.ReadLine()!;
                        var fornecNome = FiltrarFornecedorCNPJ(cnpjR);
                        if (fornecNome is not null)
                        {
                            Console.Write("\nInforme a Razão Social: ");
                            string razaoSocial = Console.ReadLine()!;
                            while (ValidarRazaoSocial(razaoSocial) == false)
                            {
                                Console.Write("Tente novamente: ");
                                razaoSocial = Console.ReadLine()!;
                            }
                            fornecNome.RazaoSocial = razaoSocial;
                            Console.WriteLine("\nRazão Social atualizada com sucesso!");
                            fornecNome.SalvarLista();
                            break;
                        }
                        Console.WriteLine("\nCNPJ não encontrado!");
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


        // Método: Menu Principal  > Aplicado o CRUD
        public void MenuPrincipal()
        {
            Supplier fornecedor = new Supplier();
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |                   >      Fornecedor      <                  |");
                Console.WriteLine(" |-------------------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Cadastrar Fornecedor  |  [ 2 ] Atualizar Fornecedor  |");
                Console.WriteLine(" |  [ 3 ] Listar Fornecedores   |  [ 4 ] Filtrar Fornecedor    |");
                Console.WriteLine(" |  [ 5 ] Fornecedor Restrito   |  [ 6 ] Voltar                |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine()!;
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
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpj = Console.ReadLine();
                        var Filtro = FiltrarFornecedorCNPJ(cnpj);
                        if (FiltrarFornecedorCNPJ(cnpj) == null)
                            Console.WriteLine("\nCNPJ não encontrado!");
                        else
                            Console.WriteLine(Filtro);
                        break;
                    case 5:
                        RestrictedSupplier.MenuPrincipal();
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Opção Inválida. Tente novamente.");
                        break;
                }
                if (opcao == 6)
                    break;
                if (opcao != 1)
                {
                    Console.Write("\nPressione Enter para prosseguir ");
                    Console.ReadLine();
                }
            } while (opcao != 6);
        }
    }
}
