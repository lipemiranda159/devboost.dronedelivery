using devboost.dronedelivery.felipe.EF.Entities;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.DTO.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task SavePedidoAsync(Pedido pedido);
        
    }
}
