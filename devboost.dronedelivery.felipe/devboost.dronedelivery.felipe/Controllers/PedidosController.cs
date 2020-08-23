using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Entities;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPedidoFacade _pedidoFacade;

        public PedidosController(DataContext context, IPedidoFacade pedidoFacade)
        {
            _context = context;
            _pedidoFacade = pedidoFacade;
        }


        [HttpPost("assign-drone")]
        public async Task<ActionResult> AssignDrone()
        {
            await _pedidoFacade.AssignDrone();
            return Ok();
        }


        // POST: api/Pedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.DataHoraInclusao = DateTime.Now;
            pedido.Situacao = (int)StatusPedido.AGUARDANDO;
            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }


    }
}
