using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaAzure.Models
{
    public class Tutoria
    {
        public string curso { get; set; }
        public string tutor { get; set; }
        public string tipo_sesion { get; set; }
        public int cantidad_estudiantes { get; set; }
        public int tarifa_individual { get; set; }
        public int tarifa_grupal { get; set; }
        public int calificacion_tutoria { get; set; }

    }
}