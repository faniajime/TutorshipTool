﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaAzure.Models
{
    public class Tutoria
    {
        public int id { get; set; }
        public int tutorID { get; set; }
        public string cursoid { get; set; }
        public string tipo_sesion { get; set; }
        public int cantidad_estudiantes { get; set; }
        public int tarifa_individual { get; set; }
        public int tarifa_grupal { get; set; }
        public double calificacion_tutoria { get; set; }
        public string tutor_nombre { get; set; }
        public string tutor_apellidos { get; set; }
        public string disponibilidad { get; set; }
        public string nombre { get; set; }
        public string area { get; set; }
        public int stars { get; set; }

    }
}