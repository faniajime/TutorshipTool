using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using WebApplication156456;
using Microsoft.AspNet.Identity;
using WebApplication156456.Handlers;

namespace AppointmentCalendar.Controllers
{
    public class HorarioController : Controller
    {
        private HorarioContext db = new HorarioContext();
        private TutorHandler tutorHandler = new TutorHandler();

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Flat;

            scheduler.InitialDate = new DateTime(2021, 11, 20);// the initial data of Scheduler

            scheduler.Config.first_hour = 7;//the minimum value of the hour scale
            scheduler.Config.last_hour = 21;//the maximum value of the hour scale
            scheduler.Config.time_step = 30;//the scale interval for the time selector in the lightbox
            scheduler.Config.limit_time_select = true;//sets max and min values for the time selector in the lightbox to the values of the last_hour and first_hour options

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            var usid = User.Identity.GetUserId();
            var apps = db.Horarios.Where( evento => evento.person_id == usid);
            return new SchedulerAjaxData(apps);
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var tutid = User.Identity.GetUserId();

            try
            {
                var changedEvent = DHXEventsHelper.Bind<HorarioModel>(actionValues);
                changedEvent.person_id = tutid;
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        db.Horarios.Add(changedEvent);
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