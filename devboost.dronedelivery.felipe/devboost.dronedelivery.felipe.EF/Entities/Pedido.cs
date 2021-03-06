﻿using System;
using System.ComponentModel.DataAnnotations;

namespace devboost.dronedelivery.felipe.EF.Entities
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        public int Peso { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public int Situacao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
