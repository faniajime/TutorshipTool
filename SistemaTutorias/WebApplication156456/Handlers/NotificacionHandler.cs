using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication156456.Models;
using Microsoft.AspNet.Identity;
using PruebaAzure.Models;

namespace WebApplication156456.Handlers
{
    public class NotificacionHandler {
        private SqlConnection sqlConnection;
        private string connectionRoute;

        public NotificacionHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            sqlConnection = new SqlConnection(connectionRoute);
        }

        public List<Notificacion> obtainNotificationsSelf(string personID) {
            List<Notificacion> notificationList = new List<Notificacion>();

            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Notificaciones_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GetAllNotifications_Self";
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = personID;

                using (SqlDataReader reader = sqlQueryCommand.ExecuteReader()) {
                    while (reader.Read()) {
                        notificationList.Add(
                            new Notificacion {
                                idNotificacion = Convert.ToInt32(reader["id"]),
                                mensajeNotificacion = Convert.ToString(reader["mensaje"]),
                                fechaNotificacion = Convert.ToString(reader["fecha"])
                            }    
                        );
                    }
                }
                sqlConnection.Close();

            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }

            return notificationList;
        }

        public void deleteSpecificNotification(int notificationID, string personID) {
            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Notificaciones_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "DeleteNotification_Self_Specific";
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = personID;
                sqlQueryCommand.Parameters.Add("@notificationID", SqlDbType.Int).Value = notificationID;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }

        }

        public void deleteAllNotifications(string personID) {
            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Notificaciones_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "DeleteNotification_Self_All";
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = personID;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void createNewNotification_Student(int studentID, string message) {
            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Notificaciones_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "InsertNotification_Estudent";
                sqlQueryCommand.Parameters.Add("@userID", SqlDbType.Int).Value = studentID;
                sqlQueryCommand.Parameters.Add("@mensaje", SqlDbType.NVarChar).Value = message;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void createNewNotification_Tutor(int tutorID, string message) {
            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Notificaciones_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "InsertNotification_Tutor";
                sqlQueryCommand.Parameters.Add("@userID", SqlDbType.Int).Value = tutorID;
                sqlQueryCommand.Parameters.Add("@mensaje", SqlDbType.NVarChar).Value = message;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }
    }
}