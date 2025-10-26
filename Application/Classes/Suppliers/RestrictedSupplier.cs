using Application.Utils.WritersAndReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Classes.Suppliers
{
    public class RestrictedSupplier
    {
        Writer_Reader objeto = new Writer_Reader();     // objeto para criar oo arquivo
        public static List<RestrictedSupplier> FornecedoresRestritos = new List<RestrictedSupplier>();
        public string Cnpj { get; private set; }

        static string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        static string file = "RestrictedSuppliers.data";
        string fullPath = Path.Combine(diretorio, file);


        // Método Construtor: Criando o arquivo (caso não exista), e popularizando o conforme os parâmetos, e tamanhos determinados
        public RestrictedSupplier()
        {
            objeto.Verificador(diretorio, fullPath);
            Console.WriteLine("Arquivo e diretório criados com sucesso.");
            PopularLista();
        }


        // Método Construtor > com o único parâmetro
        public RestrictedSupplier(string cnpj)
        {
            Cnpj = cnpj;
        }


        // Método: Popular a lista (Leiutura do arquivo) > aplicando os parâmetros do Fornecedor, e tamanhos pré-estipulados
        public void PopularLista()
        {
            StreamReader sr = new StreamReader(fullPath);

            string linha;
            while ((linha = sr.ReadLine()!) != null)
            {
                string cnpj = linha.Substring(0, 14).Trim();

                Supplier fornecedor = Supplier.FiltrarFornecedorCNPJ(cnpj);

                if (fornecedor != null)
                {
                    RestrictedSupplier fornecedorRestrito = new RestrictedSupplier(fornecedor.Cnpj);
                    FornecedoresRestritos.Add(fornecedorRestrito);
                }
                else
                    Console.WriteLine($"CNPJ: {cnpj} não encontrado!");

            }
            sr.Close();
        }


        // Método: Salvar a lista (Escrita do arquivo) > aplicando os parâmetros do Fornecedor, e tamanhos pré-estipulados
        private void SalvarLista()
        {
            StreamWriter sw = new StreamWriter(fullPath, false);

            foreach (RestrictedSupplier fornecedorRestrito in FornecedoresRestritos)
            {
                string linha = fornecedorRestrito.Cnpj.PadRight(14);
                sw.WriteLine(linha);
            }
            sw.Close();
        }


        // Método: Busca o fornecedor pelo CNPJ, e retorna ele
        public static RestrictedSupplier BuscarFornecedorRestritoPorCNPJ(string cnpj)
        {
            var fornecedorRestrito = FornecedoresRestritos.Find(c => c.Cnpj == cnpj);

            if (fornecedorRestrito is not null)
            {
                fornecedorRestrito.ToString();
                return fornecedorRestrito;
            }
            return null;
        }


        // Método: Modelo de impressão dos dados do Fornecedor
        public override string ToString()
        {
            Supplier fornecedor = Supplier.FiltrarFornecedorCNPJ(Cnpj);
            return fornecedor.ToString();
        }


        // Método: Validar a Escolha   |     Retorna false se não for igual a 'S' ou 'N'
        private static bool ValidarEscolha(char escolha)
        {
            if (escolha is not 'S' && escolha is not 'N')
            {
                Console.Write("\nInválido! Digite apenas [S] ou [N].");
                return false;
            }
            else
            {
                return true;
            }
        }


        // Método: Imprimir lista       | Retorna uma mensagem caso a lista esteja zerada
        public static void ListarFornecedor()
        {
            if (FornecedoresRestritos.Count == 0)
                Console.WriteLine("Não há fornecedores restritos na lista");

            foreach (var fornecedorRestrito in FornecedoresRestritos)
            {
                Console.WriteLine(fornecedorRestrito.ToString());
            }
        }


        // Método: Registrar Fornecedor como Restrito
        public static void RegistrarFornecedorRestrito()
        {
            Console.Write("Informe o CNPJ: ");
            string cnpj = Console.ReadLine();
            Supplier busca = Supplier.FiltrarFornecedorCNPJ(cnpj);

            while (busca == null)
            {
                Console.WriteLine("CNPJ não encontrado!");
                Console.Write("Digite [S] para sair ou tente novamente: ");
                cnpj = Console.ReadLine().ToUpper();
                if (cnpj == "S")
                    return;
                busca = Supplier.FiltrarFornecedorCNPJ(cnpj);
            }

            busca.ToString();
            Console.WriteLine("Deseja realmente adicionar o fornecedor acima à lista de restritos?");
            Console.Write("Digite [S] pra sim ou [N] pra não: ");
            string entrada = Console.ReadLine().ToUpper(); ;
            bool aux = char.TryParse(entrada, out char escolha);
            while (ValidarEscolha(escolha) == false)
            {
                Console.Write("Tente novamente: ");
                entrada = Console.ReadLine();
                aux = char.TryParse(entrada, out escolha);
            }

            if (escolha == 'S')
            {
                Console.WriteLine("\nRegistro concluído com sucesso!");
                RestrictedSupplier fornecedor = new RestrictedSupplier(busca.Cnpj);
                FornecedoresRestritos.Add(fornecedor);
                fornecedor.SalvarLista();
                return;
            }
            else
                Console.WriteLine("\nRegistro não efetuado!");
            Console.Write("\nPressione Enter para prosseguir ");
            Console.ReadLine();
        }


        // Método: Excluir fornecedor restrito da lista
        private static void DeletarFornecedorRestrito()
        {
            Console.Write("Informe o CNPJ do fornecedor a ser removido: ");
            string cnpj = Console.ReadLine();

            RestrictedSupplier busca = BuscarFornecedorRestritoPorCNPJ(cnpj);

            while (busca == null)
            {
                Console.WriteLine("\nCNPJ não encontrado!");
                Console.Write("Digite [S] para sair ou tente novamente: ");
                cnpj = Console.ReadLine().ToUpper();
                if (cnpj == "S")
                    return;
                busca = BuscarFornecedorRestritoPorCNPJ(cnpj);
            }

            Console.WriteLine(busca.ToString());
            Console.WriteLine("Tem certeza de que deseja excluir o fornecedor da lista de restritos?");
            Console.Write("Digite [S] pra sim ou [N] pra não: ");
            string entrada = Console.ReadLine().ToUpper();
            bool aux = char.TryParse(entrada, out char escolha);
            while (ValidarEscolha(escolha) == false)
            {
                Console.Write("Tente novamente: ");
                entrada = Console.ReadLine();
                aux = char.TryParse(entrada, out escolha);
            }

            if (escolha == 'S')
            {
                RestrictedSupplier.FornecedoresRestritos.Remove(busca);
                Console.WriteLine("\nExclusão concluída com sucesso!");
                busca.SalvarLista();
                return;
            }
            else
                Console.WriteLine("\nExclusão não efetuada!");
            Console.Write("\nPressione Enter para prosseguir ");
            Console.ReadLine();
        }


        // Método: Menu Principal  > Aplicado o CRUD
        public static void MenuPrincipal()
        {
            RestrictedSupplier fornecedorRestrito = new RestrictedSupplier();
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |            >      Fornecedores Restritos      <           |");
                Console.WriteLine(" |-----------------------------------------------------------|");
                Console.WriteLine(" |  [ 1 ] Registrar Fornecedor  |  [ 2 ] Remover Fornecedor  |");
                Console.WriteLine(" |  [ 3 ] Listar Fornecedores   |  [ 4 ] Filtrar Fornecedor  |");
                Console.WriteLine(" |  [ 5 ] Voltar                |                            |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine();
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        RegistrarFornecedorRestrito();
                        break;
                    case 2:
                        DeletarFornecedorRestrito();
                        break;
                    case 3:
                        ListarFornecedor();
                        break;
                    case 4:
                        Console.Write("Informe o CNPJ que deseja buscar: ");
                        string cnpj = Console.ReadLine();
                        BuscarFornecedorRestritoPorCNPJ(cnpj);
                        if (BuscarFornecedorRestritoPorCNPJ(cnpj) == null)
                            Console.WriteLine("\nCNPJ não encontrado!");
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Opção Inválida. Tente novamente.");
                        break;
                }
                if (opcao == 5)
                    break;
                Console.Write("\nPressione Enter para prosseguir ");
                Console.ReadLine();
            } while (opcao != 5);
        }
    }
}
