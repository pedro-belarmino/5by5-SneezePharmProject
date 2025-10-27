using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Classes
{

    public class RestrictedCustomer

    {
        Writer_Reader objeto = new();
        private List<RestrictedCustomer> ClientesBloqueados = new ();
        public string? Cpf { get; private set; }


        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "RestrictedCustomers.data ";
        static string fullPath = Path.Combine(diretorio, file);

        public RestrictedCustomer()
        {
            objeto.Verificador(diretorio, fullPath);
            PopularLista();
        }
        public RestrictedCustomer(string cpf) 
        {
           this.Cpf = cpf;
        }



        private void PopularLista()
        {
            StreamReader sr = new(fullPath);

            string line;

            while ((line = sr.ReadLine()!) != null)
            {
                string cpf = line.Substring(0, 11).Trim();

                RestrictedCustomer clienteBloqueado = new(cpf);
                ClientesBloqueados.Add(clienteBloqueado);
            }
            sr.Close();

        }

        private void SaveFile()
        {
            StreamWriter writer = new(fullPath);

            foreach (var cpf in ClientesBloqueados)
            {
                string cpfFormatado = cpf.Cpf!;

                writer.WriteLine(cpfFormatado);
                
            }
            writer.Close();
        }



        public void RestrictionsMenu() 
        {
            int opcao, min = 1, max = 5;
            do
            {
                do
                {
                    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                    Console.WriteLine(" |            >      Restrições de Clientes      <             |");
                    Console.WriteLine(" |-------------------------------------------------------------|");
                    Console.WriteLine(" |  [ 1 ] Restringir Cliente     |  [ 2 ] Remover Restrição    |");
                    Console.WriteLine(" |  [ 3 ] Listar Restritos       |  [ 4 ] Filtrar Cliente      |");
                    Console.WriteLine(" |  [ 5 ] Voltar                 |                             |");
                    Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                    Console.WriteLine("\nInforme a opção desejada: ");
                    string op = Console.ReadLine()!;
                    opcao = ValidateMenu(op, min, max);

                } while (opcao < min && opcao > max);


                switch (opcao)
                {
                    case 1:
                        BlockCustomer();
                        break;
                    case 2:
                        UnlockCustomer();
                        break;
                    case 3:
                        ListRestrictedClients();
                        break;
                    case 4:
                        ShowRestrictedClient();
                        break;
                    case 5:
                        SaveFile();
                        Customer Cliente = new Customer();
                        Cliente.ClientMenu();
                        break;
                }
            } while (opcao != 5);
        }

        public void BlockCustomer()
        {
            Console.Clear();
            string cpf;
            bool status;
            do
            {
                Console.WriteLine("Informe o CPF: ");
                Console.WriteLine("Para voltar digite 5");
                cpf = Console.ReadLine()!;
                if (cpf == "5")
                    RestrictionsMenu();

                status = SearchClientList(cpf);
            } while (status == false);

            int opcao, min = 1, max = 2;
            do
            {
                Console.WriteLine("\nDeseja realmente restringir o cliente?\n[1] Sim [2] Não");
                string op = Console.ReadLine()!;
                opcao = ValidateMenu(op, min, max);
            } while (opcao < 1 && opcao > 2);

            if (opcao == 2)
                RestrictionsMenu();
            

            RestrictedCustomer ClienteBloqueado = new RestrictedCustomer(cpf);
            ClientesBloqueados.Add(ClienteBloqueado);
            SaveFile();

        }

        public void UnlockCustomer()
        {
            int opcao, min = 1, max = 2;
            string cpf;

            Console.Clear();
            Console.WriteLine("Informe o CPF: ");
            Console.WriteLine("Para voltar digite 5");
            cpf = Console.ReadLine()!;
            var cliente = SearchRestrictedClient(cpf);
            if (cpf == "5")
                RestrictionsMenu();

            while(cliente is null && cpf != "5")
            {
                Console.WriteLine("Cliente não consta na lista de restrições");
                Console.WriteLine("Informe o CPF: ");
                Console.WriteLine("Para voltar digite 5");
                cpf = Console.ReadLine()!;
                cliente = SearchRestrictedClient(cpf);
            }
            if (cpf == "5")
                RestrictionsMenu();

                Console.WriteLine($"{cliente.ToString()} encontrado.");
                    
            do
            {
                Console.WriteLine("\nDeseja realmente remover a restrição do cliente?\n[1] Sim [2] Não");
                string op = Console.ReadLine()!;
                opcao = ValidateMenu(op, min, max);
            } while (opcao < 1 && opcao > 2);

            if (opcao == 2)
                RestrictionsMenu();
            else
                ClientesBloqueados.Remove(cliente);

            SaveFile();
        }

        public bool SearchClientList(string cpf)
        {

            Customer buscaCliente = new Customer();
           
            bool cpfValido = false;
            var cliente = buscaCliente.SearchCPF(cpf);

            if (cliente is null)
                Console.WriteLine("CPF não encontrado");
            else
            {
                cpfValido = true;
                Console.WriteLine(cliente.ToString());
            }
            return cpfValido;
        }

        public  void ShowRestrictedClient() 
        {
            Console.Clear();
            Console.WriteLine("Informe CPF: ");
            string cpf = Console.ReadLine()!;

            if (cpf.Length < 11)
                cpf.PadLeft(11, '0');

            var cliente = SearchRestrictedClient(cpf);

            if (cliente is null)
                Console.WriteLine("Cpf não consta na lista de restrições");
            else
                Console.WriteLine($"{cliente.ToString()} está restrito!");

        }

        public RestrictedCustomer? SearchRestrictedClient(string cpf) 
        {
            return ClientesBloqueados.Find(c => c.Cpf == cpf);
        }

        public void ListRestrictedClients()
        {
            Console.Clear();

            if (!ClientesBloqueados.Any())
                Console.WriteLine("\nLista Vazia!\n");

            foreach (var cliente in ClientesBloqueados)
            {
                Console.WriteLine(cliente.ToString());
            }
        }

        public int ValidateMenu(string opcao, int min, int max)
        {
            int o = 0;
            bool opcaoValida = opcao.All(char.IsDigit);
            if (opcaoValida)
            {
                opcaoValida = int.TryParse(opcao, out o);
                if (opcaoValida == false)
                    Console.WriteLine("Escolha uma opcao valida do menu");
            }

            return o;

        }

        public override string ToString()
        {
            return $"CPF: {Cpf}";
        }

    }
}
