using devboost.dronedelivery.felipe.Models;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public interface IPedidoService
    {
        Task<DroneDTO> DroneAtendePedido(Pedido pedido);
    }
}
