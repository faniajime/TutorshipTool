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
        public ActionResult getTutorias(string buscar)
        {
            TutoriaHandler accessoDatos = new TutoriaHandler();
            List<Tutoria> tutorias = new List<Tutoria>();
            tutorias = accessoDatos.obtenerTutorias();
            bool empty = false;
            ViewBag.empty = empty;
            if (buscar != null)
            {
                System.Diagnostics.Debug.WriteLine(buscar);
                tutorias = tutorias.FindAll(x => x.curso.Contains(buscar) || x.tutor.Contains(buscar) || x.tipo_sesion.Contains(buscar));
                ViewBag.tutorias = tutorias;

                if (tutorias.Any())
                {
                    return View();
                }
                else
                {
                    empty = true;
                    ViewBag.empty = empty;
                    return View();
                }
            }
            else
            {
                ViewBag.tutorias = tutorias;
                return View();
            }
            
        }
    }
}