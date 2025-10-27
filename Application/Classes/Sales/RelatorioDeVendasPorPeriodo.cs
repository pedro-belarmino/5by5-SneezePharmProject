using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classes.Sales
{
    public class RelatorioDeVendasPorPeriodo
    {
        public void BuscarVendas()
        {
            Console.Write("Digite o mês a ser encontrado (formato MM/yyyy): ");
            string? info = Console.ReadLine()!;

            if (!DateOnly.TryParseExact("01/" + info, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly variavel))
            {
                Console.WriteLine("Mês inválido. Use o formato MM/yyyy");
                return;
            }

            var vendasDoMes = Sale.Sales.FindAll(x => x.DataVenda.Month == variavel.Month && x.DataVenda.Year == variavel.Year
            );

            if (vendasDoMes.Count == 0)
            {
                Console.WriteLine("Nenhuma venda encontrada nesse período.");
                return;
            }

            Console.WriteLine($"\nVendas encontradas em {variavel.Month:D2}/{variavel.Year}:");
            vendasDoMes.ForEach(x => Console.WriteLine(x));
        }
    }
}
