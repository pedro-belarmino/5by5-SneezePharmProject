using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Customer
    {
        Writer_Reader objeto = new Writer_Reader();
        public static List<Customer> Clientes = new List<Customer>();
        public string? Cpf { get; private set; }
        public string? Nome { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public string? Telefone { get; private set; }
        public DateOnly? UltimaCompra { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "Customers.data";
        string fullPath = Path.Combine(diretorio, file);

        /*TRATAR A OPÇÃO 5 DO MENU PARA ADICIONAR TUDO NO ARQUIVO ANTES DE FECHAR*/

        public Customer() 
        {
            string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
            string fullPath = Path.Combine(diretorio, file);

            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");

        }

        public Customer(string cpf, string nome, DateOnly dataNascimento, string telefone, DateOnly? ultimaCompra, DateOnly dataCadastro, char situacao)
        {
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            UltimaCompra = null;
            DataCadastro = dataCadastro;
            Situacao = situacao;
        }


        public static void ClientMenu()
        {
            int opcao;
            do
            {
                int min = 1, max = 5;
                do
                {
                    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                    Console.WriteLine(" |                    >      Cliente      <                    |");
                    Console.WriteLine(" |-------------------------------------------------------------|");
                    Console.WriteLine(" |  [ 1 ] Cadastrar Cliente      |  [ 2 ] Atualizar Cliente    |");
                    Console.WriteLine(" |  [ 3 ] Listar Clientes        |  [ 4 ] Filtrar Cliente      |");
                    Console.WriteLine(" |  [ 5 ] Voltar                 |                             |");
                    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                    Console.WriteLine("\nInforme a opção desejada: ");
                    string op = Console.ReadLine()!;
                    opcao = ValidateMenu(op, min, max);

                } while (opcao < min && opcao > max);

                switch (opcao)
                {
                    case 1:
                        CreatClient();
                        break;
                    case 2:
                        UpdateClient();
                        break;
                    case 3:
                        ListClients();
                        break;
                    case 4:
                        int status = SearchClient();
                        if (status == 0)
                            ClientMenu();
                        break;
                    case 5:
                        /*SALVAR ARQUIVO AQUI*/
                        return;
                }
            } while (opcao != 5);
        }
        public static void CreatClient()
        {
            Console.WriteLine("Cadastro de Cliente");
            bool cpfValido = false, idadeValida = false, telefoneValido = false;
            string nome, cpf, telefone;
            char situacao;
            DateOnly dataNascimento;

            do
            {
                Console.WriteLine("Informe o CPF: ");
                cpf = Console.ReadLine()!;
                cpfValido = ValidateCPF(cpf);

            } while (cpfValido == false);

            do
            {
                Console.WriteLine("Informe o nome: ");
                nome = (AdjustCharacterLimit((Console.ReadLine()!), 50));
            } while (nome.Length != 50);

            do
            {
                Console.WriteLine("Informe a data de nascimento (DD/MM/AAAA): ");
                string data = Console.ReadLine()!;

                bool dataValida = DateOnly.TryParseExact(data, "dd/MM/yyyy", out dataNascimento);

                if (dataValida)
                    idadeValida = ValidateAge(dataNascimento);
                else
                    Console.WriteLine("Formato de data inválido.");      

            } while (idadeValida == false);

            do
            {
                Console.WriteLine("Informe o telefone com DDD:  ");
                telefone = Console.ReadLine()!;
                telefoneValido = ValidatePhone(telefone);

            } while (telefoneValido == false);

            DateOnly? ultimaCompra = null;

            DateOnly dataCadastro = DateOnly.FromDateTime(DateTime.Now);

            do
            {
                Console.WriteLine("Informe a situação do cliente: \n[A] Ativo [I] Inativo ");
                string s = Console.ReadLine()!.ToUpper();

                 situacao = ValidateSituation(s);

            } while (situacao != 'I' && situacao != 'A');

            Customer cliente = new Customer(cpf, nome, dataNascimento, telefone, ultimaCompra, dataCadastro, situacao);

            Clientes.Add(cliente);

            Console.Clear();

        }
        public static bool ValidateCPF(string cpf)
        {
            bool cpfValido = false;

            string cpfAjustado = cpf.PadLeft(11, '0');
               
            if (SearchCPF(cpfAjustado) is null)
            {
                bool apenasNumero = cpfAjustado.All(char.IsDigit);
                if (apenasNumero)
                    cpfValido = ValidateCpfMathematically(cpfAjustado);
                else
                    Console.WriteLine("CPF deve conter apenas números");
            }
            else
                Console.WriteLine("CPF já cadastrado");

            return cpfValido;
        }
        public static bool ValidateCpfMathematically(string cpf)
        {
            char[] cpfConvertidoChar = cpf.ToCharArray();

            bool todosIguais = false, validacao1 = false, cpfValido = false;

            if (cpfConvertidoChar.Length > 0)
            {
                todosIguais = cpfConvertidoChar.All(c => c == cpfConvertidoChar[0]);
            }

            if (todosIguais == false)
            {
                int numero1, contador1 = 10, soma1 = 0;
                for (int i = 0; i < cpfConvertidoChar.Length - 2; i++)
                {
                    numero1 = int.Parse(cpfConvertidoChar[i].ToString());
                    int multiplicacao1 = numero1 * contador1;
                    soma1 += multiplicacao1;
                    contador1--;
                }
                int resto1 = soma1 % 11;
                int digVerificador1 = 11 - resto1;

                if (digVerificador1 < 2 || digVerificador1 >= 10)
                    if ((int.Parse(cpfConvertidoChar[9].ToString())) == 0)
                    {
                        validacao1 = true;
                    }
                if (digVerificador1 == (int.Parse(cpfConvertidoChar[9].ToString())))
                    validacao1 = true;

                if (validacao1 == true)
                {
                    int numero2, contador2 = 11, soma2 = 0;
                    for (int i = 0; i < cpfConvertidoChar.Length - 1; i++)
                    {
                        numero2 = int.Parse(cpfConvertidoChar[i].ToString());
                        int multiplicacao2 = numero2 * contador2;
                        soma2 += multiplicacao2;
                        contador2--;
                    }
                    int resto2 = soma2 % 11;
                    int digVerificador2 = 11 - resto2;

                    if (digVerificador2 < 2)
                        if (cpfConvertidoChar[10] == 0)
                            cpfValido = true;
                    if (digVerificador2 == (int.Parse(cpfConvertidoChar[10].ToString())))
                        cpfValido = true; 
                }
            }
            if (cpfValido == false)
                Console.WriteLine("CPF inválido");

            return cpfValido;
        }
        public static string AdjustCharacterLimit(string nome, int limite)

        {
            if (string.IsNullOrEmpty(nome))
                Console.WriteLine("A entrada não pode ser vazia");
            else if (nome.Length > limite)
                Console.WriteLine($"Limite de caracteres atingido! Use até {limite} caracteres.");
            else if (nome.Length < limite)
                nome = nome.PadRight(limite);

            return nome;
        }
        public static bool ValidateAge(DateOnly dataNascimento)
        {
            bool idadeValida = false;

            bool dataFutura = dataNascimento > (DateOnly.FromDateTime(DateTime.Now));

            if (dataFutura)
                Console.WriteLine("Data inválida! Data de nascimento não pode ser futura.");

            int maioridade = DateTime.Now.Year - dataNascimento.Year;
            if (maioridade < 18 && dataFutura == false)
            {
                Console.WriteLine("\nVenda proibida para menores de 18 anos!\n");
                idadeValida = false;
                ClientMenu();
            }
            if (maioridade > 18 && dataFutura == false)
                idadeValida = true;

            return idadeValida;
        }
        public static bool ValidatePhone(string telefone)
        {
            bool telefoneValido = false;

            if (telefone.Length != 12)
                Console.WriteLine("Telefone deve conter 3 dígitos do DDD + 9 dígitos do número");
            else
            {
                telefoneValido = telefone.All(char.IsDigit);
                if (telefoneValido == false)
                    Console.WriteLine("Telefone deve conter apenas números");
            }

            return telefoneValido;
        }

        public static int ValidateMenu(string opcao, int min, int max) 
        {
            int o = 0;
            bool opcaoValida = opcao.All(char.IsDigit);
            if (opcaoValida)
            {
                opcaoValida = int.TryParse(opcao, out o);   
                if (opcaoValida == false)
                    Console.WriteLine("Escolha uma opcao valida do menu");
            }
            
            return o;

        }
        public static char ValidateSituation(string situacao) 
        {
            char s;
            bool situacaoValida = char.TryParse(situacao, out s);

                if (s != 'I' && s != 'A')
                {
                    Console.WriteLine("Situação deve ser [A] Ativo ou [I] Inativo");
                }
            return s;
        }

        public static void UpdateClient()
        {
            Console.Clear();
            int opcao;
            int min = 1, max = 3;
            do {
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |                >      Cliente      <                |");
                Console.WriteLine(" |-----------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Alterar Situação  |  [ 2 ] Alterar Telefone  |");
                Console.WriteLine(" |  [ 3 ] Voltar            |                          |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine("\nInforme a opção desejada: ");
                string op = Console.ReadLine()!;

                opcao = ValidateMenu(op, min, max);
            } while (opcao < min && opcao > max);


            if (opcao == 3)
                return;

                Console.Clear();
            Console.WriteLine("Informe o CPF para atualização: ");
            var pessoa = SearchCPF(Console.ReadLine()!);
            while (pessoa is null) 
            {
                Console.WriteLine("CPF não encontrado na relação de clientes"); 
                Console.WriteLine("Informe o CPF para atualização: ");
                pessoa = SearchCPF(Console.ReadLine()!);
            }

            Console.Clear();
            Console.WriteLine(pessoa.ToString()+ "\n");

            switch (opcao) 
            {
                case 1:
                    char situacao;
                    do
                    {
                        Console.WriteLine("Informe a situação do cliente: \n[A] Ativo [I] Inativo");
                        string s = Console.ReadLine()!.ToUpper();
                        situacao = ValidateSituation(s);

                        if(situacao == 'I' || situacao == 'A')
                            pessoa.Situacao = situacao;
                            Console.WriteLine("A Situação foi atualizado com sucesso");
                    } while (situacao != 'I' && situacao != 'A');
                    break;
                case 2:
                    bool telefoneValido;
                    do {
                        Console.WriteLine("Informe o telefone com DDD: ");
                        string t = Console.ReadLine()!;
                        telefoneValido = ValidatePhone(t);
                        if(telefoneValido)
                            pessoa.Telefone = t;
                            Console.WriteLine("O telefone foi atualizado com sucesso");
                    } while (telefoneValido == false);
                    break;
            }
  
        }
        public static void UpdateSaleDate(string cpf, DateOnly data) 
        {
            var pessoa = SearchCPF(cpf);

            pessoa.UltimaCompra = data;
        }

        public static Customer? SearchCPF(string cpf) 
        {
            return Clientes.Find(c => c.Cpf == cpf);
        }
        public static void ListClients()
        {
            Console.Clear();

            if (!Clientes.Any())
                Console.WriteLine("\nLista Vazia!\n");

            foreach (var cliente in Clientes)
            {
                Console.WriteLine(cliente.ToString() + "\n");
            }
        }
        public static int SearchClient() 
        {
            Console.Clear();
            Console.WriteLine("Informe o CPF para busca: ");
            string cpf = Console.ReadLine()!;

            int status = 0;
            var cliente = SearchCPF(cpf);

            if (cpf is null || cliente is null)
            {
                Console.WriteLine("CPF não encontrado");
                return status;
            }
            else
                Console.WriteLine(cliente.ToString());
            return status = 1;
            
        }
        public override string ToString()
        {
            return $"CPF: {Cpf}" +
                    $"\nNome: {Nome}" +
                    $"\nData de nascimento: {DataNascimento}" +
                    $"\nTelefone: {Telefone}" +
                    $"\nData da última venda: {UltimaCompra}" +
                    $"\nData de cadastro: {DataCadastro}" +
                    $"\nSituação: {Situacao}";
        }
        public string ToFile()
        {
            return $"{Cpf}{Nome}{DataNascimento}{Telefone}{UltimaCompra}{DataCadastro}{Situacao}";
        }
    }
}

