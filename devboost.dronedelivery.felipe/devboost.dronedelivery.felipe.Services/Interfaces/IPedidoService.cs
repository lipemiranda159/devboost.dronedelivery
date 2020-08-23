using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.EF.Entities;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<DroneDto> DroneAtendePedido(Pedido pedido);
    }
}
