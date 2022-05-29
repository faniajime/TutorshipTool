using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication156456.Models
{
    public class AsistenteModel { 
        public string nombre_estudiante { set; get; }
        public string apellido_estudiante { set; get; }
        public int id_estudiante { set; get; }
        public int id_sesion { set; get; }
        public int evaluacion_individual { set; get; }
        public int evaluacion_tutor { set; get; }
    }
}