using devboost.dronedelivery.felipe.Data;
using devboost.dronedelivery.felipe.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
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
