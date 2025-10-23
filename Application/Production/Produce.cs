using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Prod
{
    public class Produce
    {
        public int Id { get; set; }
        public DateOnly DataProducao { get; set; }
        public string Medicamento { get; set; }
        public int Quantidade { get; set; }

        public Produce(int id, DateOnly dataProducao, string medicamento, int quantidade)
        {
            Id = id;
            DataProducao = dataProducao;
            Medicamento = medicamento;
            Quantidade = quantidade;
        }
    }
}
