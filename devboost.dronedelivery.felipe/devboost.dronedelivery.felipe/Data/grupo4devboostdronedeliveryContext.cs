using Microsoft.EntityFrameworkCore;
using devboost.dronedelivery.felipe.Models;

namespace devboost.dronedelivery.felipe.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<Drone> Drone { get; set; }

        public DbSet<PedidoDrone> PedidoDrones { get; set; }
    }
}
