using devboost.dronedelivery.felipe.Models;
using Geolocation;
using devboost.dronedelivery.felipe.Data;
using devboost.dronedelivery.felipe.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public class PedidoService : IPedidoService
    {
        private const double LATITUDE_BASE = -23.5880684;
        private const double LONGITUDE_BASE = -46.6564195;
        private readonly IDroneService _droneService;

        public PedidoService(IDroneService droneService)
        {
            _droneService = droneService;
        }

        public async Task<DroneDTO> DroneAtendePedido(Pedido pedido)
        {
            var distance = GeoCalculator.GetDistance(LATITUDE_BASE, LONGITUDE_BASE, pedido.Latitude, pedido.Longitude, 1, DistanceUnit.Kilometers) * 2;

            var drones = await _droneService.GetAll();

            var buscaDrone = drones.FirstOrDefault(PodeSerSuportado(pedido, distance));

            if (buscaDrone == null)
                return null;

            return new DroneDTO(buscaDrone, distance);

        }

        private static Func<Drone, bool> PodeSerSuportado(Pedido pedido, double distance)
        {
            return d => d.Perfomance >= distance && d.Capacidade >= pedido.Peso;
        }
    }
}
