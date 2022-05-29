using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class Cursos
    {
        [Display(Name = "Siglas del curso")]
        public string id { get; set; }
        [Display(Name="Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Detalles")]
        public string detalles { get; set; }
        [Display(Name = "Área")]
        public string area_especialidad { get; set; }
        public virtual ICollection<Tutor> tutores { get; set; }
        public string Filter { get; set; }

       
    }
}