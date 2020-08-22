using System;

namespace devboost.dronedelivery.felipe.EF.Entities
{
    public class PedidoDrone
    {
        public int Id { get; set; }
        public int DroneId { get; set; }
        public Drone Drone { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public double Distancia { get; set; }
        public int StatusEnvio { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
