using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using Microsoft.AspNet.Identity;
using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Context;
using WebApplication156456.Handlers;
using WebApplication156456.Models;

namespace WebApplication156456.Controllers
{
    public class DisponibilidadSesionController : Controller
    {
        // GET: DisponibilidadSesion
        TutoriaHandler tutoriaHandler = new TutoriaHandler();
        SesionHorarioContext db = new SesionHorarioContext();
        HorarioContext tutorHorario = new HorarioContext();
        public ActionResult Index(int tutoriaId, string courseID, string courseName)
        {
            Tutoria tutoria = tutoriaHandler.GetTutoria(tutoriaId);
            var scheduler = new DHXScheduler(this);
            var sesionID = tutoriaHandler.getFuturasSesiones(tutoria.id);
            startCalendar(scheduler, tutoria);
            ViewBag.tutoria = tutoria;
            ViewBag.courseName = courseName;
            ViewBag.courseID = courseID;

            return View(scheduler);
        }

        public void startCalendar(DHXScheduler scheduler, Tutoria tutoria)
        {
            scheduler.Skin = DHXScheduler.Skins.Flat;
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Limit);
            scheduler.Extensions.Add(SchedulerExtensions.Extension.ActiveLinks);
            scheduler.Config.limit_time_select = true;
            scheduler.Config.collision_limit = 1;
            scheduler.Config.limit_drag_out = true;
            scheduler.Config.drag_move = false;
            DateTime today = DateTime.Now;
            scheduler.InitialDate = today;// the initial data of Scheduler
            scheduler.Config.check_limits = true;
            scheduler.Config.first_hour = 7;//the minimum value of the hour scale
            scheduler.Config.last_hour = 21;//the maximum value of the hour scale
            scheduler.Config.time_step = 30;//the scale interval for the time selector in the lightbox
            scheduler.Config.limit_time_select = true;//sets max and min values for the time selector in the lightbox to the values of the last_hour and first_hour options
            scheduler.Config.multi_day = false;
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            var disponibilidadTutor = tutorHorario.Horarios.Where(tutor => tutor.person_id == tutoria.tutor_pid);

            foreach (var Horario in disponibilidadTutor)
            {

                var day = Horario.start_date.DayOfWeek;
                System.Diagnostics.Debug.WriteLine(day);
                var start = Horario.start_date.Hour * 60 + Horario.start_date.Minute;
                var end = Horario.end_date.Hour * 60 + Horario.end_date.Minute;

                scheduler.TimeSpans.Add(new DHXMarkTime()
                {
                    Day = day, // marks each Saturday with the 'green_section' style
                    Zones = new List<Zone>() { new Zone { Start = start, End = end } },

                    CssClass = "green_section"
                }); ;
            }
        }

        public ContentResult Data(List<int> sesionID)
        {
            var sesiones = db.sesiones.Where(sesion => sesionID.Contains(sesion.id)).ToList();
            return new SchedulerAjaxData(sesiones);
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = DHXEventsHelper.Bind<SesionHorario>(actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        db.sesiones.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        db.Entry(changedEvent).State = EntityState.Deleted;
                        break;
                    default:// "update"  
                        db.Entry(changedEvent).State = EntityState.Modified;
                        break;
                }
                db.SaveChanges();
                action.TargetId = changedEvent.id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}




