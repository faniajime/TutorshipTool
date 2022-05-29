using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Handlers;
using WebApplication156456.Models;

namespace WebApplication156456.Controllers
{
    [Authorize(Roles = Roles.Estudiante)]
    public class TutoriaController : Controller
    {
        HorarioContext tutorHorario = new HorarioContext();
        TutorHandler tutorHandler = new TutorHandler();
        // GET: Tutoria
        public ActionResult Index(string courseID, string courseName)
        {
            var tutoriahandler = new TutoriaHandler();
            List<Tutoria> tutorias = new List<Tutoria>();
            tutorias = (List<Tutoria>)tutoriahandler.GetTutorshipsList(courseID);
            List<Tutoria> filteredTutorias = new List<Tutoria>();
            foreach (var tutoria in tutorias)
            {
                string tutorid = tutorHandler.getTutorIdFromPID(tutoria.tutorID);
                var disponibilidad = tutorHorario.Horarios.Where(tutor => tutor.person_id == tutorid);
                if (disponibilidad.Count() > 0)
                {
                    filteredTutorias.Add(tutoria);
                }
            }
            if (filteredTutorias.Count >0)
            {
                ViewBag.tutorshipsAvailable = true;
            }
            else
            {
                ViewBag.tutorshipsAvailable = false;
            }
            ViewBag.courseName = courseName;
            ViewBag.courseID = courseID;
            ViewBag.tutorias = filteredTutorias;
            return View();
        }
    }
}