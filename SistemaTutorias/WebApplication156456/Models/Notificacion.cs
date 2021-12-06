using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication156456.Models
{
    public class Notificacion {
        public int idNotificacion { set; get; }
        public string mensajeNotificacion { set; get; }
        public string fechaNotificacion { set; get; }
    }
}