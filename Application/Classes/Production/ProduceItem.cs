using System;
using System.IO;
using System.Linq;
using Application.Utils.WritersAndReaders;

namespace Application.Prod
{
    public class ProduceItem
    {
        private Writer_Reader objeto = new Writer_Reader();

        public int IdProducao { get; set; }
        public string CodigoIngrediente { get; set; } = string.Empty;
        public int QuantidadePrincipio { get; set; }

        private readonly string diretorio = "C:\\Projects\\5by5-SneezePharmProject\\Application\\Diretorios\\";
        private readonly string arquivoProduce = "ProduceItem.data";
        private readonly string arquivoIngrediente = "Ingredient.data";

        // Esse é apenas um construtor sem parâmetros pra criar o arquivo caso ele não exista na hora da execução.
        public ProduceItem()
        {
            string fullPathProduce = Path.Combine(diretorio, arquivoProduce);
            objeto.Verificador(diretorio, fullPathProduce);
        }

        // Essa é a classe que cria e salva o objeto 'item de produção' no arquivo.
        public ProduceItem(int quantidadePrincipio)
        {
            if (quantidadePrincipio > 99999)
                throw new ArgumentException("Quantidade máxima é 99999.");

            QuantidadePrincipio = quantidadePrincipio;

            string fullPathProduce = Path.Combine(diretorio, arquivoProduce);
            string fullPathIngredient = Path.Combine(diretorio, arquivoIngrediente);

            objeto.Verificador(diretorio, fullPathProduce);

            IdProducao = ObterProximoId(fullPathProduce);

            CodigoIngrediente = ObterCodigoIngrediente(fullPathIngredient);

            Salvar(fullPathProduce);

            Console.WriteLine($"Item {IdProducao} criado e salvo com sucesso!");
        }

        // Aqui que existe a lógica de achar o ID nos primeiros 5 dígitos da linha, quando ele achar o maior ID, ele faz o próximo na ordem certa pra não dar pau.
        private int ObterProximoId(string fullPath)
        {
            if (!File.Exists(fullPath))
                return 1;

            var linhas = File.ReadAllLines(fullPath);
            if (linhas.Length == 0)
                return 1;

            string ultimaLinha = linhas.Last();

            if (ultimaLinha.Length < 5)
                return 1;

            string idStr = ultimaLinha.Substring(0, 5);

            if (int.TryParse(idStr, out int ultimoId))
                return ultimoId + 1;
            else
                return 1;
        }

        // Esse aqui puxa o código do Ingredient (calma que ainda vou fazer a outra classe)
        private string ObterCodigoIngrediente(string fullPathIngredient)
        {
            if (!File.Exists(fullPathIngredient))
            {
                Console.WriteLine("Arquivo Ingredient.data não encontrado!");
                return "000000";
            }

            string? primeiraLinha = File.ReadLines(fullPathIngredient).FirstOrDefault();
            if (string.IsNullOrEmpty(primeiraLinha))
            {
                Console.WriteLine("Arquivo Ingredient.data está vazio!");
                return "000000";
            }

            return primeiraLinha.Length >= 6
                ? primeiraLinha.Substring(0, 6)
                : primeiraLinha.PadRight(6, '0');
        }

        // Esse aqui salva as alteracões individualmente no meu arquivo ProduceItem.data, ainda vou fazer a parte de leitura assim que abrir o arquivo.
        private void Salvar(string fullPathProduce)
        {
            string idFormatado = IdProducao.ToString("D5");
            string quantidadeFormatada = QuantidadePrincipio.ToString("D5");

            string linha = $"{idFormatado}{CodigoIngrediente}{quantidadeFormatada}";

            using (StreamWriter sw = new StreamWriter(fullPathProduce, append: true))
            {
                sw.WriteLine(linha);
            }

            Console.WriteLine($"Linha salva: {linha}");
        }


        // Vou bolar um to string pra mostrar bonitinho no console.
        //public override string? ToString()
        //{
        //    return $"";
        //}
    }
}
