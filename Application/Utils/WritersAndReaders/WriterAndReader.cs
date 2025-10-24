using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Utils.WritersAndReaders
{
    public class Writer_Reader
    {
        /*
        Como usar o verificador: 
        instancia um objeto 'objeto' no topo da classe:********

        Writer_Reader objeto = new Writer_Reader();

        Cria uma variável antes do construtor com o file:**********

        string file = "ProduceItem.data";

        Chama o construtor da classe sem nenhum parâmetro, dentro dele cola o diretório abaixo:********

        string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        string fullPath = Path.Combine(diretorio, file);

        depois cola o código com o nome do arquivo que será criado:**********

        objeto.Verificador(diretorio, fullPath);
        Console.WriteLine("Arquivo e diretório criados com sucesso.");

        exemplo funcionando: 

        //////////////////////////////////////
        string file = "ProduceItem.data";

        public ProduceItem() 
        {
            string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
            string fullPath = Path.Combine(diretorio, file);

            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
        }
        /////////////////////////////////////
        */
        public string Verificador(string directoryPath, string fullPath)
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
                return fullPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return "";
            }
        }



        public List<string> LerArquivoEColocarNaLista(string diretorio, string nomeArquivo)//nao funciona mais essa bomba
        {
            var caminho = Verificador(diretorio, nomeArquivo);
            var linhas = new List<string>();

            using (StreamReader sr = new StreamReader(caminho))
            {
                string linha;
                while ((linha = sr.ReadLine()!) != null)
                {
                    linhas.Add(linha);
                }
            }

            return linhas;
        }


        public void LerListaEColocarNoArquivo(List<string> l, string diretorio, string nomeArquivo)// essa aqui eu não sei ainda mas tamo ai vendo no que da quem sabe né temos que ser otimistas VAMO TIMEEEEEEE
        {
            var caminho = Verificador(diretorio, nomeArquivo);
            StreamWriter sw = new StreamWriter(caminho);

            using (sw)
            {
                foreach (string c in l)
                {
                    sw.WriteLine(c);
                    sw.Close();
                }
            }
        }

    }
}
