using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Prod
{
    public class ProduceItem
    {
        Ingredient ingredient { get; set; }
        public int Id { get; set; }
        public string Principio { get; set; }
        public int QuantidadePrincipio { get; set; }

        public ProduceItem(Ingredient ingredient, int id, Ingredient principio, int quantidadePrincipio)
        {
            this.ingredient = ingredient;
            Id = id;
            Principio = ingredient.Id;
            QuantidadePrincipio = quantidadePrincipio;
        }
    }
}
