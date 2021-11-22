using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication156456.Models
{
    public class Sesion {
        public int id { set; get; }
        public int estudiante_id { set; get; }
        public int tutor_id { set; get; }
        public string curso_id { set; get; }
        public string modalidad { set; get; }
        public int evaluacion_sesion { set; get; }
        public string direccion { set; get; }
        public string enlace { set; get; }
        public string estado_sesion { set; get; }
        public string fecha_inicio { set; get; }
        public string fecha_fin { set; get; }
    }
}