using Microsoft.EntityFrameworkCore;

namespace devboost.dronedelivery.felipe.Data
{
    public class grupo4devboostdronedeliveryContext : DbContext
    {
        public grupo4devboostdronedeliveryContext (DbContextOptions<grupo4devboostdronedeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<devboost.dronedelivery.felipe.Models.Pedido> Pedido { get; set; }

        public DbSet<devboost.dronedelivery.felipe.Models.Drone> Drone { get; set; }
    }
}
