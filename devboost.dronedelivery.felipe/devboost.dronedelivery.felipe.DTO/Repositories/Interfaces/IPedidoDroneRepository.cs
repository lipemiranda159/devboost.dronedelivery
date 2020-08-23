using devboost.dronedelivery.felipe.EF.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.DTO.Repositories.Interfaces
{
    public interface IPedidoDroneRepository
    {
        Task UpdatePedidoDrone(DroneStatusDto drone, double distancia);
        Task<List<PedidoDrone>> RetornaPedidosEmAberto();
        Task<List<PedidoDrone>> RetornaPedidosParaFecharAsync();
        Task UpdatePedido(PedidoDrone pedido);
    }
}
