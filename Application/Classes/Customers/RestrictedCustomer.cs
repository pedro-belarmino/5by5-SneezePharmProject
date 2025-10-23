using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes
{
    public class RestrictedCustomer
    {
        public Customer Customers {  get; set; }


        public RestrictedCustomer(Customer customers) 
        {
            Customers = customers;
        }


    }
}
