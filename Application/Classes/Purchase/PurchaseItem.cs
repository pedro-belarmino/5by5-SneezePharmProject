using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Purchase
{
    public class PurchaseItem
    {

        Writer_Reader objeto = new();

        public static List<PurchaseItem> PurchaseItems= new();

        public string? Id { get; private set; }
        public decimal ValorTotal { get; private set; }
        

    }
}
