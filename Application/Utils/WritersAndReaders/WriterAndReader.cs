using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.WritersAndReaders
{
    public class Writer_Reader
    {
        public void Verificador(string directoryPath, string fullPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(fullPath))
                {
                    using (StreamWriter wr = new StreamWriter(fullPath)) { }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        public static void PopularLista<T>(string fullPath, List<T> lista) where T : class
        {
            using (StreamReader sr = new StreamReader(fullPath))
            {
                string line;
                while ((line = sr.ReadLine()!) != null)
                {
                    
                }
            }
        }
    }
}
