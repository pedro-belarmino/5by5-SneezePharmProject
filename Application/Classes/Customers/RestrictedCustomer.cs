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
        Writer_Reader objeto = new Writer_Reader();

        List<RestrictedCustomer> ClientesBloqueados = new();
        public Customer Clientes {  get; private set; }
        public string Cpf { get; private set; }
        private RestrictedCustomer cliente;


        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "RestrictedCustomers.data ";
        string fullPath = Path.Combine(diretorio, file);

        public RestrictedCustomer()
        {
            string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
            string fullPath = Path.Combine(diretorio, file);

            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");

        }
        public RestrictedCustomer(string cpf) 
        {
           this.Cpf = cpf;
        }

        public void RestrictionsMenu() 
        {
            int opcao, min = 1, max = 5;
            

            do
            {
                Console.Clear();
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

            if (opcao == max)
                return;

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
            }
        }

        public void BlockCustomer()
        {
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
            } while (status == false || cpf != "5");

            int opcao, min = 1, max = 2;
            do
            {
                Console.WriteLine("\nDeseja realmente restringir o cliente?\n[1] Sim [2] Não");
                string op = Console.ReadLine()!;
                opcao = Clientes.ValidateMenu(op, min, max);
            } while (opcao < 1 && opcao > 2);

            if (opcao == 2)
                RestrictionsMenu();
            else
                cliente = new RestrictedCustomer(cpf);
                ClientesBloqueados.Add(cliente);
        }

        public void UnlockCustomer()
        {
            int opcao, min = 1, max = 2;
            string cpf;
            bool status;
            
            do
            {
                Console.WriteLine("Informe o CPF: ");
                Console.WriteLine("Para voltar digite 5");
                cpf = Console.ReadLine()!;
                if (cpf == "5")
                    RestrictionsMenu();

                var cliente = SearchRestrictedClient(cpf);
                if (cliente is null)
                    Console.WriteLine("Cliente não consta na lista de restrições");
                else
                    Console.WriteLine($"{cliente.ToString()} encontrado.");
                    
            } while (cliente is null || cpf != "5");

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
        }

        public bool SearchClientList(string cpf)
        {
            bool cpfValido = false;
            var cliente = Clientes.SearchCPF(cpf);

            if (cliente is null)
                Console.WriteLine("CPF não encontrado");
            else
                cpfValido = true;
                Console.WriteLine(cliente.ToString());
            return cpfValido;
        }

        public void ShowRestrictedClient() 
        {
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

        public RestrictedCustomer SearchRestrictedClient(string cpf) 
        {
            return ClientesBloqueados.Find(c => c.Cpf == cpf);
        }

        public void ListRestrictedClients()
        {
            foreach (var cliente in ClientesBloqueados)
            {
                Console.WriteLine(cliente.ToString());
            }
        }

        public override string ToString()
        {
            return $"CPF: {Cpf}";
        }

        public string ToFile() 
        {
            return Cpf;
        }
    }
}
