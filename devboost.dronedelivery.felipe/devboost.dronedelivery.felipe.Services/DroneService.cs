using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {
        private readonly DataContext _context;

        public DroneService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Drone>> GetAll()
        {
            return _context.Drone.ToList();
        }

        public async Task<List<DroneStatusDTO>> GetDrones()
        {
            var sqlCommand = GetSqlCommand();

            using SqlConnection conexao = new SqlConnection("server=localhost;database=desafio-drone-db;user id=sa;password=DockerSql2017!");
            var resultado = await conexao.QueryAsync<DroneStatusDTO>(sqlCommand);

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

    }
}
