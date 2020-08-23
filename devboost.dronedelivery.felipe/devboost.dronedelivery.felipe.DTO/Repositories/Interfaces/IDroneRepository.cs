using devboost.dronedelivery.felipe.EF.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.DTO.Repositories.Interfaces
{
    public interface IDroneRepository
    {
        Task<List<StatusDroneDto>> GetDroneStatusAsync();
        Task<DroneStatusDto> RetornaDroneStatus(int droneId);
        Drone RetornaDrone();
        Task SaveDrone(Drone drone);


    }
}
