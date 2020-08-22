using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public class DroneService : IDroneService
    {
        private readonly grupo4devboostdronedeliveryContext _context;

        public DroneService(grupo4devboostdronedeliveryContext context)
        {
            _context = context;
        }

        public async Task<List<Drone>> GetAll()
        {
            return _context.Drone.ToList();
        }

    }
}
