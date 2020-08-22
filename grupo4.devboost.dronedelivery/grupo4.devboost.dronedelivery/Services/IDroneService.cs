using devboost.dronedelivery.felipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public interface IDroneService
    {
        Task<List<Drone>> GetAll();
    }
}
