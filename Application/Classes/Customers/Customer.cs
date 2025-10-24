using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Customer
    {
        public List<Customer> Cliente { get; set; } = new();
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public DateOnly? UltimaCompra { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }

        public Customer(string cpf, string nome, DateOnly dataNascimento, string telefone, DateOnly dataCadastro, char situacao)
        {
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            UltimaCompra = null;
            DataCadastro = dataCadastro;
            Situacao = situacao;
        }

        public void CadastrarCliente()
        {
            Console.WriteLine("Cadastro de Cliente");
            bool cpfValido = false, idadeValida = false, telefoneValido = false, situacaoValida = false, dtCadastroValida = false;
            string nome;
            do
            {
                Console.WriteLine("Informe o CPF: ");
                string cpf = Console.ReadLine()!;
                ValidarCPF(cpf);
                
            } while (cpfValido == false);

            do
            {
                Console.WriteLine("Informe o nome: ");
                nome = (AjustarLimite((Console.ReadLine()!), 50));
            } while (nome.Length < 50);

            do
            {
                Console.WriteLine("Informe a data de nascimento: ");
                DateOnly dataNascimento = DateOnly.Parse(Console.ReadLine()!);
                if(dataNascimento > (DateOnly.FromDateTime(DateTime.Now))) 
                    Console.WriteLine("Inválida! Data de nascimento não pode ser futura."); 
                int maioridade = DateTime.Now.Year - dataNascimento.Year;
                if (maioridade < 18)
                { 
                    Console.WriteLine("Venda proibida para menores de 18 anos!");
                    CadastrarCliente(); 
                }   
                else
                    idadeValida = true;

            } while (idadeValida == false);

            do
            {
                Console.WriteLine("Informe o telefone com DDD:  ");
                string telefone = Console.ReadLine()!;
                if (telefone.Length < 12)
                    Console.WriteLine("Telefone deve conter 3 dígitos do DDD + 9 dígitos do número");
                else
                    telefoneValido = true;
            } while (telefoneValido == false);

            DateOnly ultimaCompra;

            do
            {
                Console.WriteLine();
                DateOnly dataCadastro = DateOnly.Parse(Console.ReadLine()!);
                if (dataCadastro > (DateOnly.FromDateTime(DateTime.Now)))
                    Console.WriteLine("Inválida! Data de cadastro não pode ser futura");
                else
                    dtCadastroValida = true;
            } while (dtCadastroValida == false);

            do
            {
                Console.WriteLine();
                char situacao = (char.Parse(Console.ReadLine()!));                
                if (situacao != 'I' && situacao != 'A')
                    Console.WriteLine("Situação deve ser A para Ativo ou I para Inativo");
            } while (situacaoValida == false);

        }

        public Customer PesquisarCPF(string cpf) 
        {
            return Cliente.Find(c => c.Cpf == cpf);
        }

        public bool ValidarCPF(string cpf)
        {
            bool cpfValido = false;

            if (PesquisarCPF(cpf) is null)
            {
                if (cpf.Length == 11)
                {
                    bool apenasNumero = cpf.All(char.IsDigit);
                    if (apenasNumero)
                        cpfValido = ValidarMatematicamenteCPF(cpf);
                }
                if (cpfValido == false)
                {
                    Console.WriteLine("CPF inválido! Ele deve conter 11 números");
                }
            }
            else
                Console.WriteLine("CPF já cadastrado");

            return cpfValido;
        }

        public bool ValidarMatematicamenteCPF(string cpf)
        {
            char[] cpfConvertidoChar = cpf.ToCharArray();

            bool todosIguais = false, validacao1 = false, validacao2 = false, cpfValido = false;

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
                {
                    if ((int.Parse(cpfConvertidoChar[9].ToString())) == 0)
                    {
                        validacao1 = true;
                    }
                }
                else if (digVerificador1 == (int.Parse(cpfConvertidoChar[9].ToString())))
                {
                    validacao1 = true;
                }

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
                    {
                        if (cpfConvertidoChar[10] == 0)
                        {
                            validacao2 = true;
                            cpfValido = true;
                        }
                    }
                    else if (digVerificador2 == (int.Parse(cpfConvertidoChar[10].ToString())))
                    {
                        validacao2 = true;
                        cpfValido = true;
                    }
                }
            }
            else
            {
                return cpfValido;
            }

            return cpfValido;
        }

        public string AjustarLimite(string nome, int limite)

        {
            if (string.IsNullOrEmpty(nome))
                Console.WriteLine("A entrada não pode ser vazia");
            else if (nome.Length > limite)
                Console.WriteLine($"Limite de caracteres atingido! Use até {limite} caracteres.");
            else if (nome.Length < limite)
                nome = nome.PadRight(limite);

            return nome;
        }

        public string ToFile()
        {
            return $"{Cpf}{Nome}{DataNascimento}{Telefone}{UltimaCompra}{DataCadastro}{Situacao}";
        }

        public override string ToString()
        {
            return $"CPF: {Cpf}" +
                $"\nNome: {Nome}" +
                $"\nData de Nascimento: {DataNascimento}" +
                $"\nTelefone: {Telefone}" +
                $"\nÚltima Compra: {UltimaCompra}" +
                $"\nData de Cadastro: {DataCadastro}" +
                $"\nSituação: {Situacao}";
        }
    }
}

