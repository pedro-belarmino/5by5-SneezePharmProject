using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Suppliers
{
    public class RestrictedSupplier
    {
        public string Cnpj { get; set; }


        public RestrictedSupplier(string cnpj)
        {
            Cnpj = cnpj;
        }


    }
}
