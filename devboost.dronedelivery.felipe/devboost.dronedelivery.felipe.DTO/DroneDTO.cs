
namespace devboost.dronedelivery.felipe.DTO
{
    public class DroneDTO
    {
        public DroneDTO(DroneStatusDTO droneStatus, double distancia)
        {
            DroneStatus = droneStatus;
            Distancia = distancia;
        }
        public DroneStatusDTO DroneStatus { get; set; }

        public double Distancia { get; set; }
    }
}
