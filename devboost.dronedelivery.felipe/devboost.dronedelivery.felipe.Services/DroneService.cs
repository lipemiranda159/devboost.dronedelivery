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

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {
        private const string sqlCommand = @"select a.DroneId,
                                         0 as Situacao,
                                         a.Id as PedidoId 
                                  from pedido a
                                  where a.Situacao <> 2
                                  and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)
                                  union
                                  select b.Id as DroneId,
                                         1 as Situacao,
                                         0 as PedidoId
                                  from  Drone b
                                  where b.Id NOT IN  (
                                      select a.DroneId     
                                  from pedido a
                                  where a.Situacao <> 2
                                  and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)
                                  ) ";

        private readonly DataContext _context;
        private readonly string _connectionString;

        public DroneService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(Constants.CONNECTION_STRING_CONFIG);
        }

        public async Task<List<Drone>> GetAll()
        {
            return await _context.Drone.ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<StatusDroneDTO>> GetDroneStatusAsync()
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            var resultado = await conexao.QueryAsync<StatusDroneDTO>(sqlCommand).ConfigureAwait(false);
            return resultado.ToList();
        }

        public async Task<List<DroneStatusDTO>> GetDronesAsync()
        {

            using SqlConnection conexao = new SqlConnection(_connectionString);
            var resultado = await conexao.QueryAsync<DroneStatusDTO>(GetSqlCommand()).ConfigureAwait(false);

            return resultado.ToList();
        }

        private static string GetSqlCommand()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT D.*, ");
            stringBuilder.AppendLine("SUM(P.Peso) AS SomaPeso,");
            stringBuilder.AppendLine("SUM(PD.Distancia) AS SomaDistancia ");
            stringBuilder.AppendLine("FROM dbo.PedidoDrones PD ");
            stringBuilder.AppendLine("JOIN dbo.Drone D");
            stringBuilder.AppendLine("on PD.DroneId = D.Id");
            stringBuilder.AppendLine("JOIN dbo.Pedido P");
            stringBuilder.AppendLine("on PD.DroneId = P.Id");
            stringBuilder.AppendLine("GROUP BY D.Id, D.Autonomia, D.Capacidade, D.Carga, D.Perfomance, D.Velocidade");
            return stringBuilder.ToString();
        }

        public async Task PrepareDrones()
        {
            
        }
    }
}
