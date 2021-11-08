using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Handlers;
using WebApplication156456.Models;
using System.Text;
using System.Web.UI.WebControls;

namespace WebApplication156456.Controllers
{
    [Authorize(Roles = Roles.Estudiante)]
    public class CursosController : Controller
    {
        private CursosHandler _cursosHandler;
        string valor = "";

        public ActionResult Index(string search)
        {
            
            _cursosHandler = new CursosHandler();
           
            //var model = _cursosHandler.GetCoursesList();
            //-------------------------------
            
            List<Cursos> cursos = new List<Cursos>();
            cursos = (List<Cursos>)_cursosHandler.GetCoursesList();
            bool is_empty = false;
            ViewBag.empty = is_empty;
            
            if (search != null)
            {
                
                cursos = cursos.FindAll(x => x.nombre.Contains(search) || x.area_especialidad.Contains(search) || x.id.Contains(search));
                ViewBag.cursos = cursos;

                if (cursos.Any())
                {
                    return View();
                }
                else
                {
                    is_empty = true;
                    ViewBag.empty = is_empty;
                    return View();
                }
            }
            else
            {
                ViewBag.cursos = cursos;
                return View();
            }
            //---------------------------------
            //return View(model);
        }

        public ActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCourse(Cursos model)
        {
            try
            {
                _cursosHandler = new CursosHandler();
                _cursosHandler.addCourse(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            _cursosHandler = new CursosHandler();
            var model = _cursosHandler.editById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Cursos model)
        {
            try
            {
                _cursosHandler = new CursosHandler();
                _cursosHandler.updateCourse(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }       

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _cursosHandler = new CursosHandler();
                _cursosHandler.DeleteCourse(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public string search(string selectedValue)
        {
            
            valor = selectedValue;
            System.Diagnostics.Debug.WriteLine("bhdhsdbsahkdhbsahdbahdbsahdbsahd     " + valor);
            Cursos obj = new Cursos();
            obj.Filter = selectedValue;
            return valor;
        }
    }
}
