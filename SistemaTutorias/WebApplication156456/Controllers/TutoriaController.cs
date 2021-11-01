using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Handlers;

namespace WebApplication156456.Controllers
{
    [AllowAnonymous]
    public class TutoriaController : Controller
    {
        
        // GET: Tutoria
        public ActionResult Index(string courseID, string courseName)
        {
            var tutoriahandler = new TutoriaHandler();
            List<Tutoria> tutorias = new List<Tutoria>();
            tutorias = (List<Tutoria>)tutoriahandler.GetTutorshipsList(courseID);
            if (tutorias.Count >0)
            {
                ViewBag.tutorshipsAvailable = true;
            }
            else
            {
                ViewBag.tutorshipsAvailable = false;
            }
            ViewBag.courseName = courseName;
            ViewBag.courseID = courseID;
            ViewBag.tutorias = tutorias;
            return View();
        }
    }
}