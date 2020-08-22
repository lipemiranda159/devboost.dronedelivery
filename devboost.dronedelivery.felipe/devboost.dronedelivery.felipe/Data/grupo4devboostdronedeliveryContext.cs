using Microsoft.EntityFrameworkCore;
using devboost.dronedelivery.felipe.Models;

namespace devboost.dronedelivery.felipe.Data
{
    public class grupo4devboostdronedeliveryContext : DbContext
    {
        public grupo4devboostdronedeliveryContext (DbContextOptions<grupo4devboostdronedeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<Drone> Drone { get; set; }

        public DbSet<PedidoDrone> PedidoDrones { get; set; }
    }
}
