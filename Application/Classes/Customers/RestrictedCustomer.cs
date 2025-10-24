using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes
{
    public class RestrictedCustomers
    {
        public Customer Customers {  get; set; }


        public RestrictedCustomers(Customer customers) 
        {
            Customers = customers;
        }


    }
}
