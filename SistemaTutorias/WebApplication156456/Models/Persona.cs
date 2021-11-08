using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class Persona
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contrasena { get; set; }
        public string descripcion { get; set; }
        public string persona_id { get; set; }

        public string ToString => nombre + "\n" + apellido + "\n" + email + "\n" + contrasena + "\n" + descripcion + "\n" + persona_id;
    }
}
