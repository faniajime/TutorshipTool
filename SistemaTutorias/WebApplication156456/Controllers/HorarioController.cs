using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using WebApplication156456.Handlers;

namespace WebApplication156456.Controllers
{
    public class HorarioController : Controller
    {
        HorarioHandler adminHorario = new HorarioHandler();
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Flat;
            return View(scheduler);
        }
    }
}