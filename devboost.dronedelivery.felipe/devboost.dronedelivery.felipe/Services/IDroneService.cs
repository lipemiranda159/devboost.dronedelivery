using grupo4.devboost.dronedelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public interface IDroneService
    {
        Task<List<Drone>> GetAll();
    }
}
