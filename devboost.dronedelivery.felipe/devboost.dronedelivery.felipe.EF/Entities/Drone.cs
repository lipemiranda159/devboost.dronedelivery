using System.ComponentModel.DataAnnotations;

namespace devboost.dronedelivery.felipe.EF
{
    public class Drone
    {
        [Key]
        public int Id { get; set; }
        public int Capacidade { get; set; }
        public int Velocidade { get; set; }
        public int Autonomia { get; set; }
        public int Carga { get; set; }
        public float Perfomance { get; set; }
    }
}
