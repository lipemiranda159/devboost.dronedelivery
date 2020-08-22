using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Entities;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade
{
    public class PedidoFacade : IPedidoFacade
    {
        private readonly DataContext _dataContext;
        private readonly IPedidoService _pedidoService;
        public PedidoFacade(DataContext dataContext, IPedidoService pedidoFacade)
        {
            _dataContext = dataContext;
            _pedidoService = pedidoFacade;

        }
        public async Task AssignDrone()
        {

            var pedidos = await _dataContext.Pedido.Where(FiltraPedidos()).ToArrayAsync();
            foreach (var pedido in pedidos)
            {
                var drone = await _pedidoService.DroneAtendePedido(pedido);
                pedido.Situacao = (int)StatusPedido.AGUARDANDO_ENVIO;
                pedido.DroneId = drone.Drone.Id;
                pedido.DataUltimaAlteracao = DateTime.Now;
                _dataContext.Pedido.Update(pedido);
                await _dataContext.SaveChangesAsync();
            }
        }

        private Expression<Func<Pedido, bool>> FiltraPedidos()
        {
            return p => p.Situacao == (int)StatusPedido.AGUARDANDO;
        }
    }
}
