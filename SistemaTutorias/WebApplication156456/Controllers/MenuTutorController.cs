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
        private MenuTutorHandler menuTutorHandler { get; }
        private NotificacionHandler notificationHandler { get; }
        private List<Tutoria> tutorshipList { set; get; }
        private List<Cursos> coursesList { set; get; }
        private List<Sesion> sessionList { set; get; }
        private List<Sesion> specificTutorshipSessions { set; get; }
        private List<int> tutorshipIDs { set; get; }
        private int pendingSessions { set; get; }
        private Sesion currentSession { set; get; }




        public MenuTutorController() {
            tutorMenuInstance = new MenuTutorModel();
            menuTutorHandler = new MenuTutorHandler();
            notificationHandler = new NotificacionHandler();

            tutorshipList = new List<Tutoria>();
            coursesList = new List<Cursos>();
            sessionList = new List<Sesion>();
            tutorshipIDs = new List<int>();
            specificTutorshipSessions = new List<Sesion>();
        }

        private List<Tutoria> obtainTutorships() {
            return menuTutorHandler.obtainTutorTutorships(User.Identity.GetUserId());
        }

        private List<Sesion> obtainSessions() {
            return menuTutorHandler.obtainTutorSessions(User.Identity.GetUserId());
        }

        private List<Cursos> obtainCourses() {
            return menuTutorHandler.obtainAllCourses();
        }

        private List<int> obtainTutorshipIDs() {
            return menuTutorHandler.getTutorshipIDs(User.Identity.GetUserId());
        }

        private void updateViewBag() {
            ViewBag.province = tutorMenuInstance.regionProvince;
            ViewBag.city = tutorMenuInstance.regionCity;
            ViewBag.district = tutorMenuInstance.regionDistrict;
            ViewBag.details = tutorMenuInstance.regionDetails;

            tutorshipIDs.Clear();
            tutorshipList.Clear();
            coursesList.Clear();
            sessionList.Clear();

            tutorshipIDs = obtainTutorshipIDs();
            tutorshipList = obtainTutorships();
            coursesList = obtainCourses();
            sessionList = obtainSessions();

            pendingSessions = 0;

            foreach (int id in tutorshipIDs) {
                menuTutorHandler.updateTutorshipRating(id);
            }

            foreach (Sesion sesion in sessionList) {
                sesion.lista_asistentes.Clear();
                sesion.lista_asistentes = menuTutorHandler.getSessionAssistants(sesion.id, User.Identity.GetUserId());

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
            menuTutorHandler.getTutorRegionInfo(
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

            menuTutorHandler.setTutorRegionInfo(
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


            menuTutorHandler.setTutorshipInfo(
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
            menuTutorHandler.deleteTutorship(tutorshipID);
            return RedirectToAction("MenuTutor", "MenuTutor");
        }

        public ActionResult SelectCourse(int tutorshipID) {
            updateViewBag();
            specificTutorshipSessions.Clear();
            double potentialIncome = 0.0;
            Tutoria currentTutorship = this.tutorshipList.Find(x => x.tutoria_id == tutorshipID);

            foreach (Sesion sesion in sessionList) {
                if (sesion.tutoria_id == tutorshipID && (sesion.estado_sesion == "Finalizada" || sesion.estado_sesion == "Cancelada")) {
                    specificTutorshipSessions.Add(sesion);

                    if (sesion.estado_sesion == "Finalizada") {
                        if (sesion.lista_asistentes.Count == 1) {
                            potentialIncome += currentTutorship.tarifa_individual;
                        } else if (sesion.lista_asistentes.Count > 1) {
                            potentialIncome += currentTutorship.tarifa_grupal * sesion.lista_asistentes.Count;
                        }
                    }
                }
            }
            ViewBag.income = ("₡" + potentialIncome.ToString());


            List<Tutoria> tutorshipList = ViewBag.tutorshipList;
            ViewBag.currentTutorship = tutorshipList.Find(x => x.tutoria_id == tutorshipID);
            ViewBag.specificSessions = specificTutorshipSessions;
            ViewBag.potentialIncome = potentialIncome;
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

            menuTutorHandler.addTutorship(
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
                int currentSessionID, 
                string cancelReason) {

            updateViewBag();
            string message = "";
            currentSession = menuTutorHandler.obtainSpecificSession(currentSessionID);
            string courseName = obtainCourseName(currentSession.curso_id);
            Tutor tutorInfo = menuTutorHandler.getTutorInformation(currentSession.tutor_id);

            switch (responseOperation) {
                case "VirtualNew":
                    menuTutorHandler.modifyVirtualSession(sessionAdress, sessionResponse, currentSessionID);

                    if (sessionResponse == "Pendiente") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha aceptado su solicitud de sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Fecha de sesión: {currentSession.fecha_inicio} - {currentSession.fecha_fin}\n" +
                            $"Modalidad: {currentSession.modalidad}\n" +
                            $"Cantidad de estudiantes: {currentSession.cantidad_estudiantes}\n" +
                            $"Enlace sesión: {currentSession.enlace}\n" +
                            $"Tarifas: \n -Individual {currentSession.tarifa_individual} \n -Grupal {currentSession.tarifa_grupal}\n";
                    } else {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha cancelado su solicitud de sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Motivo: {cancelReason}\n";

                    }

                    break;
                case "VirtualPending":
                    menuTutorHandler.modifyVirtualSession(sessionAdress, sessionResponse, currentSessionID);

                    if (sessionResponse == "Finalizada") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha dado por finalizada la sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Fecha de sesión: {currentSession.fecha_inicio} - {currentSession.fecha_fin}\n\n" +
                            $"Recuerde brindar su calificación para esta sesión.\n"; 
                    } else if (sessionResponse == "Cancelada") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha cancelado su sesión (ID: {currentSession.id}) agendada para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Motivo: {cancelReason}\n";
                    }

                    break;
                case "NonVirtualNew":
                    menuTutorHandler.modifyNonVirtualSession(sessionAdress, sessionResponse, currentSessionID);

                    if (sessionResponse == "Pendiente") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha aceptado su solicitud de sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Fecha de sesión: {currentSession.fecha_inicio} - {currentSession.fecha_fin}\n" +
                            $"Modalidad: {currentSession.modalidad}\n" +
                            $"Cantidad de estudiantes: {currentSession.cantidad_estudiantes}\n" +
                            $"Dirección sesión: {currentSession.enlace}\n" +
                            $"Tarifas: \n -Individual {currentSession.tarifa_individual} \n -Grupal {currentSession.tarifa_grupal}\n";
                    } else {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha cancelado su solicitud de sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Motivo: {cancelReason}\n";

                    }

                    break;
                case "NonVirtualPending":
                    menuTutorHandler.modifyNonVirtualSession(sessionAdress, sessionResponse, currentSessionID);

                    if (sessionResponse == "Finalizada") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha dado por finalizada la sesión (ID: {currentSession.id}) para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Fecha de sesión: {currentSession.fecha_inicio} - {currentSession.fecha_fin}\n\n" +
                            $"Recuerde brindar su calificación para esta sesión.\n";
                    } else if (sessionResponse == "Cancelada") {
                        message = $"El tutor {tutorInfo.nombre} {tutorInfo.apellido} ha cancelado su sesión (ID: {currentSession.id}) agendada para el curso {currentSession.curso_id}: {courseName}.\n" +
                            $"Motivo: {cancelReason}\n";
                    }

                    break;

                default:
                    break;
            }
            notificationHandler.createNewNotification_Student(currentSession.estudiante_id, message);

            return RedirectToAction("ViewSessions", "MenuTutor");
        }
    }
}