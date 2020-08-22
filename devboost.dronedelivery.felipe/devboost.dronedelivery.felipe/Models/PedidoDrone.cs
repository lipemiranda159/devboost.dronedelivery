using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Models
{
    public class PedidoDrone
    {
        public int Id { get; set; }
        public int DroneId { get; set; }
        public int PedidoId { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
