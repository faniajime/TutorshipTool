using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication156456.Models
{
    public class Sesion {
        public int id { set; get; }
        public int estudiante_id { set; get; }
        public int tutor_id { set; get; }        
        public string curso_id { set; get; }
        [Display(Name = "Modalidad")]
        public string modalidad { set; get; }
        [Display(Name = "Evaluación")]
        public int evaluacion_sesion { set; get; }
        [Display(Name = "Dirección")]
        public string direccion { set; get; }
        [Display(Name = "Enlace")]
        public string enlace { set; get; }
        [Display(Name = "Estado")]
        public string estado_sesion { set; get; }
        [Display(Name = "Inicio")]
        public string fecha_inicio { set; get; }
        [Display(Name = "Finalización")]
        public string fecha_fin { set; get; }
        [Display(Name = "Tutor")]
        public string nombre_tutor { set; get; }
        public string apellido_tutor { set; get; }
        [Display(Name = "Curso")]
        public string nombre_curso { set; get; }
        [Display(Name = "Cantidad de estudiantes")]
        public int cantidad_estudiantes { set; get; }
        [Display(Name = "Tarifa individual")]
        public int tarifa_individual { set; get; }
        [Display(Name = "Tarifa grupal")]
        public int tarifa_grupal { set; get; }
        
    }
}