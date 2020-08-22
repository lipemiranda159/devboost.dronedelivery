using devboost.dronedelivery.felipe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public interface IDroneService
    {
        Task<List<Drone>> GetAll();
    }
}
