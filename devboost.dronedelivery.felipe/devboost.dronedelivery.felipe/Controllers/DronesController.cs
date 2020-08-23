using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.EF.Entities;
using devboost.dronedelivery.felipe.EF.Data;

namespace devboost.dronedelivery.felipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DronesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDroneFacade _droneFacade;
        public DronesController(DataContext context, IDroneFacade droneFacade)
        {
            _context = context;
            _droneFacade = droneFacade;
        }


        [HttpPost("prepare-drone")]
        public async Task<ActionResult> PrepareDrone()
        {
            return Ok();
        }


        // GET: api/Drones/5
        [HttpGet("GetStatusDrone")]        
        public async Task<ActionResult<List<StatusDroneDTO>>> GetStatusDrone()
        {
            return Ok(await _droneFacade.GetDroneStatusAsync());
        }

        // POST: api/Drones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            drone.Perfomance = (drone.Autonomia / 60.0f) * drone.Velocidade;

            _context.Drone.Add(drone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrone", new { id = drone.Id }, drone);
        }

    }
}
