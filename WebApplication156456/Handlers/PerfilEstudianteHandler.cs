using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication156456.Models;

namespace WebApplication156456.Handlers
{
    public class PerfilEstudianteHandler
    {
        private SqlConnection sqlConnection;
        private string connectionRoute;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;

        public PerfilEstudianteHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            sqlConnection = new SqlConnection(connectionRoute);
        }

        public List<Sesion> getStudentSessions(string studentId)
        {
            List<Sesion> sessionList = new List<Sesion>();
            _dataSet = new DataSet();

            try
            {
                SqlCommand getSesions = new SqlCommand("Student_Profile", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter();

                sqlConnection.Open();
                getSesions.CommandType = CommandType.StoredProcedure;
                getSesions.Parameters.AddWithValue("@student_id", studentId);

                getSesions.ExecuteNonQuery();
                _adapter = new SqlDataAdapter(getSesions);
                _adapter.Fill(_dataSet);
                sqlConnection.Close();

                if (_dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < _dataSet.Tables[0].Rows.Count; i++)
                    {
                        Sesion sesion = new Sesion();
                        sesion.id = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["id"]);
                        sesion.nombre_curso = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombreCurso"]);
                        sesion.nombre_tutor = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombreTutor"]);
                        sesion.apellido_tutor = Convert.ToString(_dataSet.Tables[0].Rows[i]["apellidoTutor"]);
                        sesion.modalidad = Convert.ToString(_dataSet.Tables[0].Rows[i]["modalidadSesion"]);
                        sesion.cantidad_estudiantes = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["cantidadEstudiantes"]);
                        sesion.direccion = Convert.ToString(_dataSet.Tables[0].Rows[i]["direccionSesion"]);
                        sesion.enlace = Convert.ToString(_dataSet.Tables[0].Rows[i]["enlaceSesion"]);
                        sesion.fecha_inicio = Convert.ToString(_dataSet.Tables[0].Rows[i]["fechaInicio"]);
                        sesion.fecha_fin = Convert.ToString(_dataSet.Tables[0].Rows[i]["fechaFin"]);
                        sesion.tarifa_individual = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["tarifaIndiv"]);
                        sesion.tarifa_grupal = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["tarifaGrup"]);
                        sesion.evaluacion_sesion = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["evaluacionSesion"]);
                        sesion.estado_sesion = Convert.ToString(_dataSet.Tables[0].Rows[i]["estadoSesion"]);
                        sesion.estudiante_id = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["estudianteID"]);
                        sesion.contrasena = Convert.ToString(_dataSet.Tables[0].Rows[i]["contrasena"]);
                        sesion.privacidad = Convert.ToString(_dataSet.Tables[0].Rows[i]["privacidad"]);
                        sessionList.Add(sesion);
                        System.Diagnostics.Debug.WriteLine(sesion.nombre_curso);
                    }
                }
                return sessionList;

            }
            catch (SqlException sqlException)
            {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }

            return sessionList;
        }
        public void evaluarSesion(int sesionID,  int studentID, int calificacion)
        {

            sqlConnection.Open();

            SqlCommand update = new SqlCommand("calificar_sesion", sqlConnection);
            update.CommandType = CommandType.StoredProcedure;
            update.Parameters.AddWithValue("@sesion_id", sesionID);
            update.Parameters.AddWithValue("@calificacion", calificacion);
            update.Parameters.AddWithValue("@student_id", studentID);
            update.ExecuteNonQuery();
            sqlConnection.Close();
            
        }
    }
}