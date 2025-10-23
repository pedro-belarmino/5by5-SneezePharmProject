using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Customers
    {
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public DateOnly UltimaCompra { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }


        public Customers(string cpf, string nome, DateOnly dataNascimento, string telefone, DateOnly ultimaCompra, DateOnly dataCadastro, char situacao)
            {
                Cpf = cpf;
                Nome = nome;
                DataNascimento = dataNascimento;
                Telefone = telefone;
                UltimaCompra = ultimaCompra;
                DataCadastro = dataCadastro;
                Situacao = situacao;
            }
        

        public bool ValidarCPF(string cpf) 
        {
            
        }



    }
}
