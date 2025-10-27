using System;
using System.Globalization;
using System.Linq;
namespace Application.Classes.Sales
{
    public class RelatorioDeVendasPorPeriodo
    {
        public void BuscarVendas()
        {
            DateOnly variavel;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Digite o mês a ser encontrado (formato MM/yyyy): ");
                Console.ResetColor();
                string? info = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(info))
                {
                    Console.WriteLine("Entrada vazia. Tente novamente.");
                    continue;
                }
                if (DateOnly.TryParseExact("01/" + info, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out variavel))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Mês inválido. Use o formato MM/yyyy (ex: 10/2025)");
                Console.ResetColor();
            }
            var vendasDoMes = Sale.Sales
                .FindAll(x => x.DataVenda.Month == variavel.Month && x.DataVenda.Year == variavel.Year);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"===== RELATÓRIO DE VENDAS — {variavel.Month:D2}/{variavel.Year} =====");
            Console.ResetColor();
            if (vendasDoMes.Count == 0)
            {
                Console.WriteLine("\nNenhuma venda encontrada nesse período.");
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                return;
            }
            decimal totalPeriodo = 0;
            foreach (var venda in vendasDoMes)
            {
                Console.WriteLine(venda);
                totalPeriodo += venda.ValorTotal;
            }
            Console.WriteLine("----------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total vendido no período: R$ {totalPeriodo:F2}");
            Console.ResetColor();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }
    }
}