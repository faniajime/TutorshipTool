using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class Tutor
    {
        public string id { get; set; }
        public string region_provinc { get; set; }
        public string region_canton { get; set; }
        public string region_distr { get; set; }
        public string region_detalles { get; set; }
        public List<DiaHorarioModel> horario { get; set; }

    }
}