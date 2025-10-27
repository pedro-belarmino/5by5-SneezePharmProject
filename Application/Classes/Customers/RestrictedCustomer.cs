using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Classes
{

    public class RestrictedCustomer

    {
        Writer_Reader objeto = new ();

        private List<RestrictedCustomer> ClientesBloqueados = new ();
        public Customer? Clientes {  get; private set; }
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
                    opcao = Clientes.ValidateMenu(op, min, max);

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
                        return;
                }
            } while (opcao != 0);
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
                opcao = Clientes.ValidateMenu(op, min, max);
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
                opcao = Clientes.ValidateMenu(op, min, max);
            } while (opcao < 1 && opcao > 2);

            if (opcao == 2)
                RestrictionsMenu();
            else
                ClientesBloqueados.Remove(cliente);

            SaveFile();
        }

        public bool SearchClientList(string cpf)
        {
            bool cpfValido = false;
            var cliente = Clientes.SearchCPF(cpf);

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

        public override string ToString()
        {
            return $"CPF: {Cpf}";
        }

    }
}
