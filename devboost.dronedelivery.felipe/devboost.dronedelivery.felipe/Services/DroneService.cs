using devboost.dronedelivery.felipe.Models;
using devboost.dronedelivery.felipe.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Text;

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {
        private readonly grupo4devboostdronedeliveryContext _context;

        public DroneService(grupo4devboostdronedeliveryContext context)
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
