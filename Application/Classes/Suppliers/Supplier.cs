using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Supplier
    {

        public string Cnpj { get; private set; }
        public string RazaoSocial { get; private set; }
        public string Pais { get; private set; }
        public DateOnly DataAbertura { get; private set; }
        public DateOnly? UltimoFornecimento { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }

        public static List<Supplier> Suppliers  = new List<Supplier>();


        public Supplier(string cnpj, string razaoSocial, string pais, DateOnly dataAbertura, DateOnly? ultimoFornecimento, DateOnly dataCadastro, char situacao)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Pais = pais;
            DataAbertura = dataAbertura;
            UltimoFornecimento = ultimoFornecimento;
            DataCadastro = dataCadastro;
            Situacao = situacao;
        }


        private static bool TemEsteCNPJ(string cnpj)
        {
            return Suppliers.Exists(c => c.Cnpj == cnpj);
        }

        private bool ValidarCnpj(string cnpj)
        {
            bool ehApenasNumero = cnpj.All(char.IsDigit);
            char[] letras = cnpj.ToCharArray();

            if (letras.Length != 14 || ehApenasNumero == false)
            {
                Console.WriteLine("A entrada não pode ser maior que 14 caracteres.");
                return false;
            }

            if (TemEsteCNPJ(cnpj))
            {
                Console.WriteLine("Este CNPJ já foi cadastrado.");
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


        private bool ValidarRazaoSocial(string nomeRazaoSocial)
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

        private bool ValidarPais(string pais)
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

        private bool ValidarSituacao(char situacao)
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

        private bool ValidarData(DateOnly data)
        {
            if (data > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("A data informada não pode ser maior que a data atual.");
                return false;
            }
            return true;
        }



        public void CadastrarFornecedor()
        {
            Console.Write("Informe o CNPJ: ");
            string cnpj = Console.ReadLine();
            while (!ValidarCnpj(cnpj))
            {
                Console.Write("Inválido! Informe o CNPJ ou [S] pra sair: ");
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


            Console.Write("\nData de cadastro: ");
            DateOnly dataCadastro = DateOnly.Parse(Console.ReadLine());
            while (ValidarData(dataCadastro) == false)
            {
                Console.Write("Informe novamente: ");
                dataCadastro = DateOnly.Parse(Console.ReadLine());
            }
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



        public override string ToString()
        {
            return "";
        }


    }
}
