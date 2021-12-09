using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Handlers;
using WebApplication156456.Models;
using Microsoft.AspNet.Identity;




namespace WebApplication156456.Controllers
{
    [Authorize(Roles = Roles.Estudiante)]
    public class PerfilEstudianteController : Controller
    {
        private PerfilEstudianteHandler perfilEstudianteHandler = new PerfilEstudianteHandler();
        private SesionHandler sesionHandler = new SesionHandler();
        private MenuTutorHandler menuTutorHandler = new MenuTutorHandler();
        public ActionResult Perfil(string thiserror = "false")
        {
            int student_id = sesionHandler.getStudentID(User.Identity.GetUserId());
            //var model = perfilEstudianteHandler.getStudentSessions(User.Identity.GetUserId());
            //int my_id = Convert.ToInt32(User.Identity.GetUserId());
            List <Sesion> sesions = perfilEstudianteHandler.getStudentSessions(User.Identity.GetUserId());
            ViewBag.thiserror = thiserror;
            ViewBag.sesions = sesions;
            ViewBag.studentID = student_id;
            return View();
        }


        public ActionResult Evaluar(int idSesion, string thiserror = "false")
        {
            Sesion sesion = sesionHandler.getSesion(idSesion, User.Identity.GetUserId());
            ViewBag.estudianteID = sesionHandler.getStudentID(User.Identity.GetUserId());
            ViewBag.sesion = sesion;
            ViewBag.error = thiserror;
            return View();
        }

        public ActionResult enviarEvaluacion(int sesionID, int estudianteID, int evaluacion)
        {
            //var yaevaluo = perfilEstudianteHandler.yaEvaluo(estudianteID, sesionID);
           
            perfilEstudianteHandler.evaluarSesion(sesionID, estudianteID, evaluacion);
            menuTutorHandler.updateSessionRating(sesionID);
            menuTutorHandler.updateTutorshipRatingFromSID(sesionID);
            return RedirectToAction("Perfil", "PerfilEstudiante", new { thiserror = "true"});

            
        }
    }
}