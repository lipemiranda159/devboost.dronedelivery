﻿namespace devboost.dronedelivery.felipe.Models
{
    public class DroneDTO
    {
        public DroneDTO(Drone drone, double distancia)
        {
            this.Drone = drone;
            this.Distancia = distancia;
        }
        public Drone Drone { get; set; }

        public double Distancia { get; set; }
    }
}
