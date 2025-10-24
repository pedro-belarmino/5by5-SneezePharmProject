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
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public DateOnly UltimaCompra { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }

        public Customer(string cpf, string nome, DateOnly dataNascimento, string telefone, DateOnly ultimaCompra, DateOnly dataCadastro, char situacao)
        {
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            UltimaCompra = ultimaCompra;
            DataCadastro = dataCadastro;
            Situacao = situacao;
        }

        public string AjustarLimite(string propriedade, int limite)
        {
            string propPreenchida = propriedade;

            if (string.IsNullOrEmpty(propriedade))
                Console.WriteLine("A entrada não pode ser vazia");
            else if (propriedade.Length > limite)
                Console.WriteLine($"Limite de caracteres atingido! Use até {limite} caracteres.");
            else if (propriedade.Length < limite)
                propPreenchida = propriedade.PadRight(limite);

            return propPreenchida;
        }

        // public bool ValidarCPF(string cpf) 
        // {

        // }



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

