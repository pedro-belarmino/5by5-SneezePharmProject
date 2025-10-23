using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Classes.Sales;

namespace Application
{
    public class CreateId
    {
        public string Create(List<string> list)
        {
            if (list.Count == 0)
                return "00001";

            string lastLine = list[list.Count - 1];
            string lastIdText = lastLine.Substring(0, 5);
            int lastId = int.Parse(lastIdText);
            int newId = lastId + 1;

            return newId.ToString("D5");
        }


    }
}



