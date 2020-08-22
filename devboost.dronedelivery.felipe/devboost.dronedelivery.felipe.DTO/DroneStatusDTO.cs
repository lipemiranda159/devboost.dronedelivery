using devboost.dronedelivery.felipe.EF;

namespace devboost.dronedelivery.felipe.DTO
{
    public class DroneStatusDTO
    {
        public Drone Drone { get; set; }
        public int SomaPeso { get; set; }
        public int SomaDistancia { get; set; }
    }
}
