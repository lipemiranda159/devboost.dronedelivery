using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.EF;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<DroneDTO> DroneAtendePedido(Pedido pedido);
    }
}
