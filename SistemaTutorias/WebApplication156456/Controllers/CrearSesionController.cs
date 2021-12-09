using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using Microsoft.AspNet.Identity;
using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Context;
using WebApplication156456.Handlers;
using WebApplication156456.Models;

namespace WebApplication156456.Controllers
{
    public class CrearSesionController : Controller
    {
        // GET: DisponibilidadSesion
        TutoriaHandler tutoriaHandler = new TutoriaHandler();
        SesionHorarioContext db = new SesionHorarioContext();
        HorarioContext tutorHorario = new HorarioContext();
        SesionHandler sesiones = new SesionHandler();
        MenuTutorHandler tutorHandler = new MenuTutorHandler();
        public ActionResult Index(int tutoriaId, string courseID, string courseName)
        {
            Tutoria tutoria = tutoriaHandler.GetTutoria(tutoriaId);
            ViewBag.tutoria = tutoria;
            ViewBag.courseName = courseName;
            ViewBag.courseID = courseID;
            ViewBag.tutoriaID = tutoria.id;
            TempData["tutorID"] = tutoria.tutorID;
            TempData["tutoriaID"] = tutoriaId;
            TempData["courseID"] = courseID;
            var exito = (string)TempData["exito"];
            TempData["modalidad"] = tutoria.tipo_sesion;
            ViewBag.exito = exito;
            var lista_sesiones = sesiones.getOpenSesions(tutoriaId);
            List<Sesion> sesiones_vacias = new List<Sesion>();
            foreach (Sesion ses in lista_sesiones)
            {
                if (ses.cantidad_estudiantes < tutoria.cantidad_estudiantes)
                {
                    sesiones_vacias.Add(ses);
                }
            }
            ViewBag.sesiones = sesiones_vacias;
            ViewBag.max_est = tutoria.cantidad_estudiantes;
            ViewBag.tarifa = tutoria.tarifa_grupal;

            return View();
        }
        public ActionResult Agendar(int tutoriaID)
        {
            if (tutoriaID.Equals(null)) {
                tutoriaID = (int)TempData["tutoriaID"];
            }
            ViewBag.agendarError = (string)TempData["agendarError"];
            Tutoria tutoria = tutoriaHandler.GetTutoria(tutoriaID);
            ViewBag.tutoria = tutoria;
            var scheduler = new DHXScheduler(this);
            startCalendar(scheduler, tutoria);
            TempData["tutorID"] = tutoria.tutorID;
            TempData["tutoriaID"] = tutoriaID;
            ViewBag.scheduler = scheduler;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTutorship(Sesion sesion)
        {

            sesion.estudiante_id = sesiones.getStudentID(User.Identity.GetUserId());

            SesionHorario actionValues = (SesionHorario)TempData["cal"];
            sesion.tutor_id = (int)TempData["tutorID"];
            sesion.tutoria_id = (int)TempData["tutoriaID"];
            sesion.curso_id = (string)TempData["courseID"];
            sesion.direccion = "";
            sesion.enlace = "";
            sesion.estado_sesion = "Esperando Respuesta";
            if (sesion.privacidad == "Privada")
            {
                sesion.contrasena = Regex.Replace(System.Web.Security.Membership.GeneratePassword(7, 0), @"[^a-zA-Z0-9]", m => "9");
            }

            if (Equals(sesion.modalidad, null))
            {
                sesion.modalidad = (string)TempData["modalidad"];
            }
            if(!(actionValues is null))
            {
                sesion.fecha_inicio = Convert.ToString(actionValues.start_date);
                sesion.fecha_fin = Convert.ToString(actionValues.end_date);
                sesion.descripcion = Convert.ToString(actionValues.text);
            }
            else
            {
                TempData["agendarError"] = "fallo";
                return RedirectToAction("Agendar", "CrearSesion", new { tutoriaID = (int)TempData["tutoriaID"] });
            }

            if (Equals(sesion.fecha_inicio, null) || Equals(sesion.fecha_fin, null) || Equals(sesion.modalidad, null) || Equals(sesion.privacidad, null))
            {
                TempData["agendarError"] = "fallo";
                return RedirectToAction("Agendar", "CrearSesion", new { tutoriaID = (int)TempData["tutoriaID"] });


            }
            int result = sesiones.crearSesion(sesion);
            if(result > 0)
            {
                sesiones.inscribirEstudiante(sesion.estudiante_id, result);
                return RedirectToAction("CreacionExitosa", "CrearSesion", new { result = true, tutoriaID = sesion.tutoria_id, contrasena = sesion.contrasena});
            }
            else
            {
                return RedirectToAction("CreacionExitosa", "CrearSesion", new { result = false, tutoriaID = sesion.tutoria_id, contrasena = sesion.contrasena });
            }
            
        }

        public ActionResult inscribirse( int sesionID, int estudianteID, string password)
        {
            Sesion sesion = sesiones.getSesion(sesionID);
            int estudiante_id = sesiones.getStudentID(User.Identity.GetUserId());
            var exito = "";
            if (estudiante_id == sesion.estudiante_id || sesiones.inscrito(estudianteID, sesionID))
            {
                TempData["exito"] = "inscrito";
                return RedirectToAction("Index", "CrearSesion", new { tutoriaID = sesion.tutoria_id, courseID = sesion.curso_id, courseName = sesion.nombre_curso});
            }
            if(sesion.privacidad == "Privada")
            {
                if (Equals(sesion.contrasena, password))
                {
                    sesiones.inscribirEstudiante(estudiante_id, sesion.id);
                    TempData["exito"] = "exito";
                    return RedirectToAction("Index", "CrearSesion", new { tutoriaID = sesion.tutoria_id, courseID = sesion.curso_id, courseName = sesion.nombre_curso });
                }
                else
                {
                    TempData["exito"] = "fallo";
                    return RedirectToAction("Index", "CrearSesion", new { tutoriaID = sesion.tutoria_id, courseID = sesion.curso_id, courseName = sesion.nombre_curso });
                }
            }
            else{
                sesiones.inscribirEstudiante(estudiante_id, sesion.id);
                TempData["exito"] = "exito";
            }
            TempData["exito"] = "fallo";
            return RedirectToAction("Index", "CrearSesion", new { tutoriaID = sesion.tutoria_id, courseID = sesion.curso_id, courseName = sesion.nombre_curso });

        }

        public ActionResult CreacionExitosa(bool result, int tutoriaID, string contrasena)
        {
            ViewBag.result = result;
            ViewBag.tutoriaID = tutoriaID;
            ViewBag.contrasena = contrasena;
            return View();
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
            string tutorid = tutoria.tutor_pid;
            var disponibilidadTutor = tutorHorario.Horarios.Where(tutor => tutor.person_id == tutoria.tutor_pid);



            var sesionesBloqueo = tutorHandler.obtainTutorSessions(tutorid);

            foreach (var sesion in sesionesBloqueo)
            {
                if(sesion.estado_sesion == "Pendiente")
                {
                    var day = Convert.ToDateTime(sesion.fecha_inicio).DayOfWeek;
                    var start = Convert.ToDateTime(sesion.fecha_inicio).Hour * 60 + Convert.ToDateTime(sesion.fecha_inicio).Minute;
                    var end = Convert.ToDateTime(sesion.fecha_fin).Hour * 60 + Convert.ToDateTime(sesion.fecha_fin).Minute;

                    scheduler.TimeSpans.Add(new DHXBlockTime()
                    {
                        Day = day, // marks each Saturday with the 'green_section' style
                        Zones = new List<Zone>() { new Zone { Start = start, End = end } }
                    });
                }

            }
            List<DayOfWeek> days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            foreach (var Horario in disponibilidadTutor)
            {
                var day = Horario.start_date.DayOfWeek;
                var start = Horario.start_date.Hour * 60 + Horario.start_date.Minute;
                var end = Horario.end_date.Hour * 60 + Horario.end_date.Minute;

                scheduler.TimeSpans.Add(new DHXMarkTime()
                {
                    Day = day, // marks each Saturday with the 'green_section' style
                    Zones = new List<Zone>() { new Zone { Start = start, End = end } },
                    CssClass = "green_section"
                }); ;
                scheduler.TimeSpans.Add(new DHXMarkTime()
                {
                    Day = day, // marks each Saturday with the 'green_section' style
                    Zones = new List<Zone>() { new Zone { Start = start, End = end } },
                    InvertZones = true,
                    SpanType = DHXMarkTime.Type.BlockEvents
                });

                if (days.Any(a => a == day))
                {
                    days.Remove(day);
                }
            }
            foreach (var day in days)
            {
                var start =7 *60;
                var end = 21*60;
                scheduler.TimeSpans.Add(new DHXMarkTime()
                {
                    Day = day, // marks each Saturday with the 'green_section' style
                    Zones = new List<Zone>() { new Zone { Start = start, End = end } },
                    SpanType = DHXMarkTime.Type.BlockEvents
                });
            }
        }

        public ContentResult Data()
        {
            //var sesiones = db.sesiones;
            return new SchedulerAjaxData();
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = DHXEventsHelper.Bind<SesionHorario>(actionValues);
                TempData["cal"] = changedEvent;
                
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




