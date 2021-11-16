using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class DiaHorarioModel
    { 
        public int id { get; set; }
        public int dia { get; set; }

        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }

        public bool disponible { get; set; }
    }
}