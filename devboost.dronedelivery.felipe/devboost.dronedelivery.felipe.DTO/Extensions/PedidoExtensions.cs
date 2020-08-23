using devboost.dronedelivery.felipe.EF.Entities;
using System;
using System.Collections.Generic;

namespace devboost.dronedelivery.felipe.DTO.Extensions
{
    public static class PedidoExtensions
    {
        public static Point GetPoint(this Pedido pedido)
        {
            return new Point(pedido.Latitude, pedido.Longitude);
        }
    }
}
