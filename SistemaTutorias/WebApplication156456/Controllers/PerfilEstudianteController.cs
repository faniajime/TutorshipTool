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
        private PerfilEstudianteHandler perfilEstudianteHandler;
        private SesionHandler sesionHandler;
        
        public ActionResult Perfil()
        {
            perfilEstudianteHandler = new PerfilEstudianteHandler();
            sesionHandler = new SesionHandler();
            int student_id = sesionHandler.getStudentID(User.Identity.GetUserId());
            //var model = perfilEstudianteHandler.getStudentSessions(User.Identity.GetUserId());
            //int my_id = Convert.ToInt32(User.Identity.GetUserId());
            List <Sesion> sesions = perfilEstudianteHandler.getStudentSessions(User.Identity.GetUserId());
            ViewBag.sesions = sesions;
            ViewBag.studentID = student_id;
            return View();
        }
    }
}