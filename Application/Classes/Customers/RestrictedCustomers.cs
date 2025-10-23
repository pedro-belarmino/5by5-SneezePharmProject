using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes
{
    public class RestrictedCustomers
    {
        public Customers Customers {  get; set; }


        public RestrictedCustomers(Customers customers) 
        {
            Customers = customers;
        }


    }
}
