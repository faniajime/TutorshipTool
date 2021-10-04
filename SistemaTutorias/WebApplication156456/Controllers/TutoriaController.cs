using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PruebaAzure.Models;
using PruebaAzure.Handlers;


namespace PruebaAzure.Controllers
{
    public class TutoriasController : Controller
    {
        public ActionResult getTutorias()
        {
            TutoriaHandler accessoDatos = new TutoriaHandler();
            ViewBag.tutorias = accessoDatos.obtenerTutorias();
            return View();
        }

        public ActionResult filtrarTutorias(string filtro)
        {
            System.Diagnostics.Debug.WriteLine(filtro);
            List<Tutoria> tutorias = new List<Tutoria>();
            tutorias = ViewBag.tutorias;
            tutorias = tutorias.FindAll(x => x.curso.Contains(filtro) || x.tutor.Contains(filtro) || x.tipo_sesion.Contains(filtro));
            ViewBag.tutorias = tutorias;
            return View();
        }
    }
}