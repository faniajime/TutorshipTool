using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class Curso
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string detalles { get; set; }
        public string area_especialidad { get; set; }
        public virtual ICollection<Tutor> tutores { get; set; }
    }
}