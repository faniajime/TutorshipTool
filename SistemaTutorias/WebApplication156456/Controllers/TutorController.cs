using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Models;

namespace WebApplication156456.Controllers
{
    [Authorize(Roles = Roles.Tutor)]
    public class TutorController : Controller
    {
        // GET: Tutor
        
        public ActionResult Index()
        {

            return View();
        }
    }
}