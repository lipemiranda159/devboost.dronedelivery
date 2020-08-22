using System;

namespace devboost.dronedelivery.felipe.Models
{
    public class PedidoDrone
    {
        public int Id { get; set; }
        public int DroneId { get; set; }
        public Drone Drone { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public int Distancia { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
