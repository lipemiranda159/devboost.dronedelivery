using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Models
{
    public class StatusDroneDTO
    {
        public int DroneId { get; set; }
        public bool Situacao { get; set; }
        public int PedidoId { get; set; }

    }
}
