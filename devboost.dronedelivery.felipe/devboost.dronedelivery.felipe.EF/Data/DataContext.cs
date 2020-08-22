using devboost.dronedelivery.felipe.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace devboost.dronedelivery.felipe.EF.Data
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
