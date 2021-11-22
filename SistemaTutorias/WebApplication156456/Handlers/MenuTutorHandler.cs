using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication156456.Models;

namespace WebApplication156456.Handlers
{
    public class MenuTutorHandler {
        private SqlConnection sqlConnection;
        private string connectionRoute;

        public MenuTutorHandler() {
            connectionRoute = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            sqlConnection = new SqlConnection(connectionRoute);
        }

        public List<Cursos> obtainAllCourses() {
            List<Cursos> coursesList = new List<Cursos>();
            string sqlQuery = "SELECT * FROM CURSO";
            DataTable resultingTable = obtainDataTableFromQuery(sqlQuery);

            foreach (DataRow row in resultingTable.Rows) {
                coursesList.Add(
                    new Cursos
                    {
                        nombre = Convert.ToString(row["nombre"]),
                        detalles = Convert.ToString(row["detalles"]),
                        area_especialidad = Convert.ToString(row["area_especialidad"]),
                        id = Convert.ToString(row["id"])
                    }
                );
            }
            return coursesList;
        }

        public List<Tutoria> obtainTutorTutorships(string personID) {
            List<Tutoria> tutorshipList = new List<Tutoria>();
            string sqlQuery = 
                "select * from tutoria " +
                "inner join tutor on tutoria.tutorID = tutor.id " +
                "where tutor.personID= " + "\'" + personID + "\'";
            DataTable resultingTable = obtainDataTableFromQuery(sqlQuery);

            foreach (DataRow row in resultingTable.Rows) {
                tutorshipList.Add(
                    new Tutoria { 
                        tutoria_id = Convert.ToInt32(row["id"]),
                        tutor_id = Convert.ToInt32(row["tutorID"]),
                        curso_id = Convert.ToString(row["cursoid"]),
                        tipo_sesion = Convert.ToString(row["tipo_sesion"]),
                        cantidad_estudiantes = Convert.ToInt32(row["cantidad_estudiantes"]),
                        tarifa_individual = Convert.ToInt32(row["tarifa_individual"]),
                        tarifa_grupal = Convert.ToInt32(row["tarifa_grupal"]),
                        calificacion_tutoria = (float)Convert.ToDouble(row["calificacion_tutoria"])
                    }
                );
            }
            return tutorshipList;
        }

        public List<Sesion> obtainTutorSessions(string personID) {
            List<Sesion> sessionList = new List<Sesion>();

            try {
                SqlCommand sqlQueryCommand = new SqlCommand("Sesion_CRUD", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);

                sqlConnection.Open();
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "RetrieveByPersonID";
                sqlQueryCommand.Parameters.Add("@person_ID", SqlDbType.NVarChar).Value = personID;

                using (SqlDataReader reader = sqlQueryCommand.ExecuteReader()) {
                    while (reader.Read()) {
                        sessionList.Add(
                            new Sesion {
                                id = Convert.ToInt32(reader["id"]),
                                estudiante_id = Convert.ToInt32(reader["estudiante_id"]),
                                tutor_id = Convert.ToInt32(reader["tutor_id"]),
                                curso_id = Convert.ToString(reader["curso_id"]),
                                modalidad = Convert.ToString(reader["modalidad"]),
                                evaluacion_sesion = Convert.ToInt32(reader["evaluacion_sesion"]),
                                direccion = Convert.ToString(reader["direccion"]),
                                enlace = Convert.ToString(reader["enlace"]),
                                estado_sesion = Convert.ToString(reader["estado_sesion"]),
                                fecha_inicio = Convert.ToString(reader["start_date"]),
                                fecha_fin = Convert.ToString(reader["end_date"])
                            }

                        );
                    }
                }

            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }

            return sessionList;
        }

        private DataTable obtainDataTableFromQuery(string sqlQuery) {
            SqlCommand sqlQueryCommand = new SqlCommand(sqlQuery, sqlConnection);
            SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);
            DataTable resultDataTable = new DataTable();
            sqlConnection.Open();
            dataTableAdapter.Fill(resultDataTable);
            sqlConnection.Close();

            return resultDataTable;
        }

        public void setTutorRegionInfo(
                string province,
                string city,
                string district,
                string details,
                string userId) {

            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Tutor_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "updateRegion";
                sqlQueryCommand.Parameters.Add("@regionProvince", SqlDbType.NVarChar).Value = province;
                sqlQueryCommand.Parameters.Add("@regionCity", SqlDbType.NVarChar).Value = city;
                sqlQueryCommand.Parameters.Add("@regionDistrict", SqlDbType.NVarChar).Value = district;
                sqlQueryCommand.Parameters.Add("@regionDetails", SqlDbType.NVarChar).Value = details;
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = userId;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void getTutorRegionInfo(string userId, MenuTutorModel tutorMenuModel) {
            DataTable resultDataTable;

            if ((resultDataTable = getTutorRegionInfoTable(userId)) != null) {
                try
                {
                    tutorMenuModel.regionProvince = Convert.ToString(resultDataTable.Rows[0]["region_provinc"]);
                    tutorMenuModel.regionCity = Convert.ToString(resultDataTable.Rows[0]["region_canton"]);
                    tutorMenuModel.regionDistrict = Convert.ToString(resultDataTable.Rows[0]["region_distr"]);
                    tutorMenuModel.regionDetails = Convert.ToString(resultDataTable.Rows[0]["region_detalles"]);
                }
                catch 
                {
                    tutorMenuModel.regionProvince ="";
                    tutorMenuModel.regionCity = "";
                    tutorMenuModel.regionDistrict = "";
                    tutorMenuModel.regionDetails = "";
                }
            }
        }

        private DataTable getTutorRegionInfoTable(string userId) {
            DataTable resultDataTable = null;

            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Tutor_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "getRegion";
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = userId;
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(sqlQueryCommand);
                resultDataTable = new DataTable();
                dataTableAdapter.Fill(resultDataTable);
                sqlConnection.Close();
            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }

            return resultDataTable;
        }

        public void setTutorshipInfo(
                int tutorshipID,
                string tutorshipModality,
                int inputMaxStudents,
                int inputIndividualFare,
                int inputGroupFare) {

            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Tutoria_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "updateTutorshipFromUI";
                sqlQueryCommand.Parameters.Add("@id", SqlDbType.Int).Value = tutorshipID;
                sqlQueryCommand.Parameters.Add("@tipo_sesion", SqlDbType.VarChar).Value = tutorshipModality;
                sqlQueryCommand.Parameters.Add("@cantidad_estudiantes", SqlDbType.Int).Value = inputMaxStudents;
                sqlQueryCommand.Parameters.Add("@tarifa_individual", SqlDbType.Int).Value = inputIndividualFare;
                sqlQueryCommand.Parameters.Add("@tarifa_grupal", SqlDbType.Int).Value = inputGroupFare;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void deleteTutorship(int tutorshipID) {
            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Tutoria_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "deleteTutorshipFromUI";
                sqlQueryCommand.Parameters.Add("@id", SqlDbType.Int).Value = tutorshipID;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            } catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void addTutorship(
                string userID,
                string tutorshipCourse,
                string tutorshipModality,
                int inputMaxStudents,
                int inputIndividualFare,
                int inputGroupFare) {

            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Tutoria_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "addTutorshipFromUI";
                sqlQueryCommand.Parameters.Add("@personID", SqlDbType.NVarChar).Value = userID;
                sqlQueryCommand.Parameters.Add("@cursoid", SqlDbType.NVarChar).Value = tutorshipCourse;
                sqlQueryCommand.Parameters.Add("@tipo_sesion", SqlDbType.NVarChar).Value = tutorshipModality;
                sqlQueryCommand.Parameters.Add("@cantidad_estudiantes", SqlDbType.Int).Value = inputMaxStudents;
                sqlQueryCommand.Parameters.Add("@tarifa_individual", SqlDbType.Int).Value = inputIndividualFare;
                sqlQueryCommand.Parameters.Add("@tarifa_grupal", SqlDbType.Int).Value = inputGroupFare;
                sqlQueryCommand.Parameters.Add("@calificacion_tutoria", SqlDbType.Float).Value = 0;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void modifyVirtualSession(string sessionAdress, string sessionResponse, int currentSessionID) {
            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Sesion_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UpdateVirtual";
                sqlQueryCommand.Parameters.Add("@link", SqlDbType.VarChar).Value = sessionAdress;
                sqlQueryCommand.Parameters.Add("@state", SqlDbType.VarChar).Value = sessionResponse;
                sqlQueryCommand.Parameters.Add("@session_ID", SqlDbType.Int).Value = currentSessionID;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }

        public void modifyNonVirtualSession(string sessionAdress, string sessionResponse, int currentSessionID) {
            try {
                sqlConnection.Open();
                SqlCommand sqlQueryCommand = new SqlCommand("Sesion_CRUD", sqlConnection);
                sqlQueryCommand.CommandType = CommandType.StoredProcedure;
                sqlQueryCommand.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UpdateNonVirtual";
                sqlQueryCommand.Parameters.Add("@direction", SqlDbType.VarChar).Value = sessionAdress;
                sqlQueryCommand.Parameters.Add("@state", SqlDbType.VarChar).Value = sessionResponse;
                sqlQueryCommand.Parameters.Add("@session_ID", SqlDbType.Int).Value = currentSessionID;
                sqlQueryCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (SqlException sqlException) {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
        }
    }
}