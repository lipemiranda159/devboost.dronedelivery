using devboost.dronedelivery.felipe.EF.Entities;

namespace devboost.dronedelivery.felipe.DTO
{
    public class DroneStatusDTO
    {
        public Drone Drone { get; set; }
        public int SomaPeso { get; set; }
        public int SomaDistancia { get; set; }
    }
}
