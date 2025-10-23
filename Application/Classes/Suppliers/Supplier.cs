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
        public DateOnly UltimoFornecimento { get; private set; }
        public DateOnly DataCadastro { get; private set; }
        public char Situacao { get; private set; }

        public static List<Supplier> Suppliers  = new List<Supplier>();


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


        private static bool TemEsteCNPJ(string cnpj)
        {
            return Suppliers.Exists(c => c.Cnpj == cnpj);
        }

        public bool ValidarCnpj(string cnpj)
        {
            char[] letras = cnpj.ToCharArray();
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

                if (int.Parse(letras[13].ToString()) == vd2 && !TemEsteCNPJ(cnpj))
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


        public override string ToString()
        {
            return "";
        }


    }
}
