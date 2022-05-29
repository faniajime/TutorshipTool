using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public class MenuTutorModel
    {
        public string regionProvince { get; set; }
        public string regionCity { get; set; }
        public string regionDistrict { get; set; }
        public string regionDetails { get; set; }
        public List<Tutoria> tutorTutorships { get; set; }
    }
}