using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes
{

    public class RestrictedCustomer

    {
        List<RestrictedCustomer> ClientesBloqueados = new();
        public Customer Clientes {  get; private set; }
        public string CPF { get; private set; }

        public RestrictedCustomer(string cpf) 
        {
           this.CPF = cpf;
        }

        public void RestrictionsMenu() 
        {
            int opcao, min = 1, max = 3;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |            >      Restrições de Clientes      <             |");
                Console.WriteLine(" |-------------------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Restringir Cliente     |  [ 2 ] Remover Restrição    |");
                Console.WriteLine(" |  [ 3 ] Voltar                 |                             |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine("\nInforme a opção desejada: ");
                string op = Console.ReadLine()!;
                opcao = Clientes.ValidateMenu(op, min, max);

            } while (opcao < min && opcao > max);

            if (opcao == max)
                return;

            int status = Clientes.SearchClient();
            if (status == 0)
                RestrictionsMenu();

            switch (opcao) 
            {
                case 1:
                    BlockCustomer();
                    break;
                case 2:
                    UnlockCustomer();
                    break;
            }

        

        }

        public void BlockCustomer()
        {
            Console.WriteLine("Deseja realmente restringir o cliente?\n[1] Sim [2] Não");
            string op = Console.ReadLine()!;
            int opcao, min = 1, max = 2;
            opcao = Clientes.ValidateMenu(op, min, max);

            if (opcao == 2)
                RestrictionsMenu();
            else
                ClientesBloqueados.Add();
            /*FALTA FINALIZAR O ADICIONAR*/


        }
        public void UnlockCustomer() 
        {
            Console.WriteLine("Deseja realmente remover a restrição do cliente?\n[1] Sim [2] Não");
            string op = Console.ReadLine()!;
            int opcao, min = 1, max = 2;
            opcao = Clientes.ValidateMenu(op, min, max);

            if (opcao == 2)
                RestrictionsMenu();
            else
                ClientesBloqueados.Remove();
            /*FALTA FINALIZAR O REMOVER */
        }
    }
}
