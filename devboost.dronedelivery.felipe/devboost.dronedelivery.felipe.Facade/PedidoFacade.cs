using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.Facade.Interface;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade
{
    public class PedidoFacade : IPedidoFacade
    {
        private readonly DataContext _dataContext;
        public PedidoFacade(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AssignDrone()
        {
            
        }
    }
}
