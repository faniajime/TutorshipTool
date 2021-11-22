using Microsoft.AspNet.Identity;
using PruebaAzure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System;
using WebApplication156456.Controllers;
using WebApplication156456.Handlers;
using WebApplication156456.Models;
using System.Data;

namespace WebApplication156456.Controllers
{
    [Authorize(Roles = Roles.Tutor)]
    public class MenuTutorController : Controller {
        public MenuTutorModel tutorMenuInstance { get; }
        private MenuTutorHandler databaseHandler { get; }
        private List<Tutoria> tutorshipList { set; get; }
        private List<Cursos> coursesList { set; get; }
        private List<Sesion> sessionList { set; get; }
        private int pendingSessions { set; get; }
        private Sesion currentSession { set; get; }


        public MenuTutorController() {
            tutorMenuInstance = new MenuTutorModel();
            databaseHandler = new MenuTutorHandler();
        }

        private List<Tutoria> obtainTutorships() {
            return databaseHandler.obtainTutorTutorships(User.Identity.GetUserId());
        }

        private List<Sesion> obtainSessions() {
            return databaseHandler.obtainTutorSessions(User.Identity.GetUserId());
        }

        private List<Cursos> obtainCourses() {
            return databaseHandler.obtainAllCourses();
        }

        private void updateViewBag() {
            ViewBag.province = tutorMenuInstance.regionProvince;
            ViewBag.city = tutorMenuInstance.regionCity;
            ViewBag.district = tutorMenuInstance.regionDistrict;
            ViewBag.details = tutorMenuInstance.regionDetails;


            tutorshipList = obtainTutorships();
            coursesList = obtainCourses();
            sessionList = obtainSessions();
            pendingSessions = 0;
            foreach (Sesion sesion in sessionList) {
                if (sesion.estado_sesion == ("Esperando Respuesta")) {
                    pendingSessions++;
                }
            }

            foreach (Tutoria tutorship in tutorshipList) {
                tutorship.nombre_curso = obtainCourseName(tutorship.curso_id);
            }

            ViewBag.tutorshipList = tutorshipList;
            ViewBag.coursesList = coursesList;
            ViewBag.sessionList = sessionList;
            ViewBag.pendingSessions = pendingSessions;
        }

        public ActionResult MenuTutor() {
            databaseHandler.getTutorRegionInfo(
                User.Identity.GetUserId(),
                tutorMenuInstance
            );

            updateViewBag();
            return View();
        }


        public ActionResult SubmitRegion(
                string inputProvincia,
                string inputCanton,
                string inputDistrito,
                string inputDetalles) {

            databaseHandler.setTutorRegionInfo(
                inputProvincia,
                inputCanton,
                inputDistrito,
                inputDetalles,
                User.Identity.GetUserId()
            );

            return RedirectToAction("MenuTutor", "MenuTutor");
        }

        public ActionResult SubmitTutorship(
                int inputTutorshipID,
                string tutorshipModality,
                int inputMaxStudents,
                int inputIndividualFare,
                int inputGroupFare) {


            databaseHandler.setTutorshipInfo(
                inputTutorshipID,
                tutorshipModality,
                inputMaxStudents,
                inputIndividualFare,
                inputGroupFare
            );

            ///TODO
            return RedirectToAction("MenuTutor", "MenuTutor");
        }

        public ActionResult DeleteTutorship(int tutorshipID) {
            databaseHandler.deleteTutorship(tutorshipID);
            return RedirectToAction("MenuTutor", "MenuTutor");
        }

        public ActionResult SelectCourse(int tutorshipID) {
            updateViewBag();
            List<Tutoria> tutorshipList = ViewBag.tutorshipList;
            ViewBag.currentTutorship = tutorshipList.Find(x => x.tutoria_id == tutorshipID);
            return View("SelectCourse");
        }

        private string obtainCourseName(string courseID) {
            string courseName = coursesList.Find(x => x.id == courseID).nombre;
            return courseName;
        }

        public ActionResult AddTutorship() {
            updateViewBag();
            return View("AddTutorship");
        }

        public ActionResult AddThisTutorship(
                string tutorshipCourse,
                string tutorshipModality,
                int inputMaxStudents,
                int inputIndividualFare,
                int inputGroupFare) {

            databaseHandler.addTutorship(
                User.Identity.GetUserId(),
                tutorshipCourse,
                tutorshipModality,
                inputMaxStudents,
                inputIndividualFare,
                inputGroupFare
            );

            return RedirectToAction("MenuTutor", "MenuTutor");
        }

        public ActionResult setHorario() 
        {
            return View();
        }

        public ActionResult ViewSessions() {
            updateViewBag();
            return View("ViewSessions");
        }

        public ActionResult AdministerSessionRequest(int sessionID) {
            updateViewBag();

            if (sessionList.Exists(x => x.id == sessionID)) {
                currentSession = sessionList.Find(x => x.id == sessionID);
                ViewBag.currentSession = currentSession;
                ViewBag.currentSessionCourse = obtainCourseName(currentSession.curso_id);
                return View("AdministerSessionRequest");
            } else {
                return new HttpNotFoundResult();
            }
        }

        public ActionResult SubmitSessionResponse(
                string sessionAdress,
                string sessionResponse,
                string responseOperation,
                int currentSessionID) {

            switch (responseOperation) {
                case "VirtualNew":
                    databaseHandler.modifyVirtualSession(sessionAdress, sessionResponse, currentSessionID);
                    break;
                case "VirtualPending":
                    databaseHandler.modifyVirtualSession(sessionAdress, sessionResponse, currentSessionID);
                    break;
                case "NonVirtualNew":
                    databaseHandler.modifyNonVirtualSession(sessionAdress, sessionResponse, currentSessionID);
                    break;
                case "NonVirtualPending":
                    databaseHandler.modifyNonVirtualSession(sessionAdress, sessionResponse, currentSessionID);
                    break;
                default:
                    break;
            }
            return RedirectToAction("ViewSessions", "MenuTutor");
        }
    }
}