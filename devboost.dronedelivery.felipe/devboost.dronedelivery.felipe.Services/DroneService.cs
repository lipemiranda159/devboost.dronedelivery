using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using devboost.dronedelivery.felipe.EF.Entities;
using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Extensions;
using System;
using System.Linq.Expressions;

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {

        private readonly DataContext _context;
        private readonly string _connectionString;
        private readonly ICoordinateService _coordinateService;
        public DroneService(DataContext context,
            IConfiguration configuration,
            ICoordinateService coordinateService)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(Constants.CONNECTION_STRING_CONFIG);
            _coordinateService = coordinateService;
        }

        public async Task<List<Drone>> GetAll()
        {
            return await _context.Drone.ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<StatusDroneDTO>> GetDroneStatusAsync()
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            var resultado = await conexao.QueryAsync<StatusDroneDTO>(GetStatusSqlCommand()).ConfigureAwait(false);
            return resultado.ToList();
        }
        /// <summary>
        /// Retorna os drones que podem ser utilizados para a entrega
        /// </summary>
        /// <param name="distance">Distancia entre o ponto de entrega e o endereço adicionado</param>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public async Task<DroneStatusDTO> GetAvailiableDroneAsync(double distance, Pedido pedido)
        {
            var drones = _context.PedidoDrones.Where(FiltroPedidosEmAberto())
                .Select(d => new
                {
                    distance = _coordinateService.GetKmDistance(d.Pedido.GetPoint(), pedido.GetPoint()),
                    droneId = d.DroneId
                }).OrderBy(p => p.distance);

            if (drones != null)
            {

                foreach (var drone in drones)
                {
                    var resultado = await RetornaDroneStatus(drone.droneId).ConfigureAwait(false);
                    if (ConsegueCarregar(resultado, drone.distance, distance, pedido))
                    {
                        return resultado;
                    }
                    else
                    {
                        var distanciaPedido = resultado.SomaDistancia + distance + drone.distance;
                        await UpdatePedidoDrone(resultado, distanciaPedido)
                            .ConfigureAwait(false);
                    }
                }
                return null;
            }
            else
            {
                await FinalizaPedidosAsync();
                var drone = _context.Drone.FirstOrDefault();
                return new DroneStatusDTO(drone);
            }
        }

        private string GetStatusSqlCommand()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GetSelectPedidos(0, StatusEnvio.AGUARDANDO));
            stringBuilder.AppendLine(" union");
            stringBuilder.Append(GetSelectPedidos(1, StatusEnvio.EM_TRANSITO));
            stringBuilder.AppendLine(" union");
            stringBuilder.AppendLine(" select b.Id as DroneId,");
            stringBuilder.AppendLine(" 1 as Situacao,");
            stringBuilder.AppendLine(" 0 as PedidoId");
            stringBuilder.AppendLine(" from  Drone b");
            stringBuilder.AppendLine(" where b.Id NOT IN  (");
            stringBuilder.AppendLine(" select a.DroneId");
            stringBuilder.AppendLine(" from PedidoDrones a");
            stringBuilder.AppendLine($" where a.StatusEnvio <> {(int)StatusEnvio.FINALIZADO}");
            stringBuilder.AppendLine(" and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)");
            stringBuilder.AppendLine(")");
            return stringBuilder.ToString();
        }

        private string GetSelectPedidos(int situacao, StatusEnvio status)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("select a.DroneId,");
            stringBuilder.AppendLine($"{situacao} as Situacao,");
            stringBuilder.AppendLine("a.Id as PedidoId");
            stringBuilder.AppendLine(" from PedidoDrones a");
            stringBuilder.AppendLine($" where a.StatusEnvio <> {(int)status}");
            stringBuilder.AppendLine(" and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)");
            return stringBuilder.ToString();
        }

        private async Task FinalizaPedidosAsync()
        {

            var pedidos = await _context
                .PedidoDrones
                .Where(p =>
                    p.StatusEnvio == (int)StatusEnvio.EM_TRANSITO &&
                    p.DataHoraFinalizacao >= DateTime.Now)
                .ToListAsync()
                .ConfigureAwait(false);
            if (pedidos.Count > 0)
            {
                foreach (var pedido in pedidos)
                {
                    pedido.StatusEnvio = (int)StatusEnvio.FINALIZADO;
                    _context.PedidoDrones.Update(pedido);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private static Expression<Func<PedidoDrone, bool>> FiltroPedidosEmAberto()
        {
            return p => p.StatusEnvio == (int)StatusEnvio.AGUARDANDO;
        }

        private async Task UpdatePedidoDrone(DroneStatusDTO drone, double distancia)
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            await conexao.ExecuteAsync("UPDATE dbo.PedidoDrones" +
                $" SET StatusPedido ={(int)StatusEnvio.EM_TRANSITO}," +
                $"DataHoraFinalizacao = {drone.Drone.ToTempoGasto(distancia)}" +
                $" WHERE DroneId = {drone.Drone.Id}")
                .ConfigureAwait(false);

        }

        private async Task<DroneStatusDTO> RetornaDroneStatus(int droneId)
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            return (await conexao.QueryAsync<DroneStatusDTO>(GetSqlCommand(droneId))
                .ConfigureAwait(false)).FirstOrDefault();
        }

        private bool ConsegueCarregar(DroneStatusDTO droneStatus,
            double PedidoDroneDistance,
            double DistanciaRetorno,
            Pedido pedido)
        {
            return droneStatus != null
                    && (ValidaDistancia(droneStatus, PedidoDroneDistance, DistanciaRetorno))
                    && ValidaPeso(droneStatus, pedido);
        }

        private static bool ValidaPeso(DroneStatusDTO droneStatus, Pedido pedido)
        {
            return droneStatus.SomaPeso + pedido.Peso < droneStatus.Drone.Capacidade;
        }

        private static bool ValidaDistancia(DroneStatusDTO droneStatus, double PedidoDroneDistance, double DistanciaRetorno)
        {
            return droneStatus.SomaDistancia + DistanciaRetorno + PedidoDroneDistance < droneStatus.Drone.Perfomance;
        }

        private static string GetSqlCommand(int droneId)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT D.*");
            stringBuilder.AppendLine("SUM(P.Peso) AS SomaPeso,");
            stringBuilder.AppendLine("SUM(PD.Distancia) AS SomaDistancia ");
            stringBuilder.AppendLine("FROM dbo.PedidoDrones PD ");
            stringBuilder.AppendLine("JOIN dbo.Drone D");
            stringBuilder.AppendLine("on PD.DroneId = D.Id");
            stringBuilder.AppendLine("JOIN dbo.Pedido P");
            stringBuilder.AppendLine("on PD.DroneId = P.Id");
            stringBuilder.AppendLine($"WHERE PD.DroneId = {droneId}");
            stringBuilder.AppendLine("GROUP BY D.Id, D.Autonomia, D.Capacidade, D.Carga, D.Perfomance, D.Velocidade");
            return stringBuilder.ToString();
        }

        public async Task PrepareDrones()
        {
            var pedidos = _context.PedidoDrones.Where(p => p.StatusEnvio == (int)StatusEnvio.AGUARDANDO);
            foreach (var pedido in pedidos)
            {

            }
        }
    }
}
