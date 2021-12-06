using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Handlers;
using WebApplication156456.Models;

namespace WebApplication156456.Controllers
{
    public class NotificacionController : Controller {
        private NotificacionHandler notificationHandler { get; }
        private List<Notificacion> notificationList { set; get; }

        public NotificacionController() {
            notificationHandler = new NotificacionHandler();
        }

        public void updateViewbag() {
            notificationList = notificationHandler.obtainNotificationsSelf(User.Identity.GetUserId());
            ViewBag.notifications = notificationList;
        }

        public ActionResult NotificationPanel() {
            updateViewbag();
            return View();
        }

        public ActionResult DeleteNotification(int notID) {
            notificationHandler.deleteSpecificNotification(notID, User.Identity.GetUserId());
            return RedirectToAction("NotificationPanel");
        }

        public ActionResult DeleteAllNotifications() {
            notificationHandler.deleteAllNotifications(User.Identity.GetUserId());
            return RedirectToAction("NotificationPanel");
        }

    }
}