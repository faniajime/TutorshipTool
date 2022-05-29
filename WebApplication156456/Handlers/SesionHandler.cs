using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebApplication156456.Models;

namespace WebApplication156456.Handlers
{
    public class SesionHandler
    {
        private SqlConnection sqlConnection;
        private string connectionRoute;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;

        public SesionHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            sqlConnection = new SqlConnection(connectionRoute);
            connection = new SqlConnection(connectionRoute);
        }

        public int getStudentID(string personID)
        {

            sqlConnection.Open();
            SqlCommand get = new SqlCommand("get_student_id", sqlConnection);
            get.CommandType = CommandType.StoredProcedure;
            get.Parameters.AddWithValue("@person_id", personID);
            _adapter = new SqlDataAdapter(get);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            sqlConnection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToInt32(_dataSet.Tables[0].Rows[0]["id"]);
            }
            return -1;
        }
        public int crearSesion(Sesion sesion)
        {

            sqlConnection.Open();
            SqlCommand insert = new SqlCommand("Sesion_CRUD", sqlConnection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "CreateSession";
            insert.Parameters.Add("@course_id", SqlDbType.NVarChar).Value = sesion.curso_id;
            insert.Parameters.Add("@student_ID", SqlDbType.Int).Value =  sesion.estudiante_id;
            insert.Parameters.Add("@tutor_ID", SqlDbType.Int).Value =  sesion.tutor_id;
            insert.Parameters.Add("@tutorship_ID", SqlDbType.Int).Value = sesion.tutoria_id;
            insert.Parameters.Add("@modality", SqlDbType.VarChar).Value =  sesion.modalidad;
            insert.Parameters.Add("@direction", SqlDbType.VarChar).Value =  sesion.direccion;
            insert.Parameters.Add("@link", SqlDbType.VarChar).Value =  sesion.enlace;
            insert.Parameters.Add("@state", SqlDbType.VarChar).Value = sesion.estado_sesion;
            insert.Parameters.Add("@start_Date", SqlDbType.DateTime).Value =  sesion.fecha_inicio;
            insert.Parameters.Add("@end_Date", SqlDbType.DateTime).Value = sesion.fecha_fin;
            insert.Parameters.Add("@privacidad", SqlDbType.VarChar).Value = sesion.privacidad;
            insert.Parameters.Add("@text", SqlDbType.VarChar).Value = sesion.descripcion;
            insert.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = sesion.contrasena;
            _adapter = new SqlDataAdapter(insert);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            var adapter = _adapter;
            sqlConnection.Close();
       
            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToInt32(_dataSet.Tables[0].Rows[0]["id"]);
            }
            return -1;
        }

        public int getCantidadSesiones(int idTutoria)
        {
            sqlConnection.Open();
            SqlCommand get = new SqlCommand("Sesion_CRUD", sqlConnection);
            get.CommandType = CommandType.StoredProcedure;
            get.Parameters.AddWithValue("@mode", "RetrieveNumberOpenSessions");
            get.Parameters.AddWithValue("@tutorship_ID", idTutoria);
            _adapter = new SqlDataAdapter(get);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            sqlConnection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToInt32(_dataSet.Tables[0].Rows[0]["cantidad"]);
            }
            return -1;
        }
        public List<Sesion> getOpenSesions(int tutoriaId, string personID)
        {
            List<Sesion> sesionList = new List<Sesion>();
            _dataSet = new DataSet();

            try
            {

                MenuTutorHandler handler = new MenuTutorHandler();
                SqlCommand show = new SqlCommand("get_open_sessions", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(show);
                sqlConnection.Open();
                show.CommandType = CommandType.StoredProcedure;
                show.Parameters.Add("@tutorship_ID", SqlDbType.Int).Value = tutoriaId;
                var session_handler = new SesionHandler();

                using (SqlDataReader reader = show.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sesionList.Add(
                        new Sesion
                        {
                            id = Convert.ToInt32(reader["id"]),
                            estudiante_id = Convert.ToInt32(reader["estudiante_id"]),
                            tutoria_id = Convert.ToInt32(reader["id_tutoria"]),
                            privacidad = Convert.ToString(reader["privacidad"]),
                            modalidad = Convert.ToString(reader["modalidad"]),
                            direccion = Convert.ToString(reader["region_provinc"]) + ", " + Convert.ToString(reader["region_canton"]) + ", " + Convert.ToString(reader["region_distr"]) + ", " + Convert.ToString(reader["region_detalles"]) + ".",
                            enlace = Convert.ToString(reader["enlace"]),
                            estado_sesion = Convert.ToString(reader["estado_sesion"]),
                            fecha_inicio = Convert.ToString(reader["start_date"]),
                            fecha_fin = Convert.ToString(reader["end_date"]),
                            descripcion = Convert.ToString(reader["text"]),
                            nombre_tutor = Convert.ToString(reader["nombre"]),
                            apellido_tutor = Convert.ToString(reader["apellido"]),
                            nombre_curso = Convert.ToString(reader["nombre_curso"]),
                            sigla_curso = Convert.ToString(reader["sigla_curso"]),
                            contrasena = Convert.ToString(reader["contrasena"]),
                            tarifa_individual = Convert.ToInt32(reader["tarifa_individual"]),
                            tarifa_grupal = Convert.ToInt32(reader["tarifa_grupal"]),
                            cantidad_estudiantes = getEstudiantesInscritos(Convert.ToInt32(reader["id"]))
                        });
                    }

                    foreach (Sesion sesion in sesionList) {
                        sesion.lista_asistentes.Clear();
                        sesion.lista_asistentes = handler.getSessionAssistants(sesion.id, personID);
                    }
                }
                
            }
            catch (SqlException sqlException)
            {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
            sqlConnection.Close();
            return sesionList;
        }
        public int getEstudiantesInscritos(int idSesion)
        {
            
            SqlCommand get = new SqlCommand("get_subscribed_students", connection);
            connection.Open();
            get.CommandType = CommandType.StoredProcedure;
            get.Parameters.AddWithValue("@sesion_id", idSesion);
            _adapter = new SqlDataAdapter(get);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            connection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToInt32(_dataSet.Tables[0].Rows[0]["cantidad"]);
            }
            return -1;
        }

        public int inscribirEstudiante(int idEstudiante, int idSesion)
        {
            sqlConnection.Open();
            SqlCommand insert = new SqlCommand("insert_asistant", sqlConnection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@id_student", idEstudiante);
            insert.Parameters.AddWithValue("@id_session", idSesion);
            insert.ExecuteNonQuery();
            _adapter = new SqlDataAdapter(insert);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            sqlConnection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return 1;
            }
            return -1;
        }

        public string getContrasena(int idSesion)
        {
            SqlCommand get = new SqlCommand("get_password", connection);
            connection.Open();
            get.CommandType = CommandType.StoredProcedure;
            get.Parameters.AddWithValue("@session_ID", idSesion);
            _adapter = new SqlDataAdapter(get);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            connection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToString(_dataSet.Tables[0].Rows[0]["contrasena"]);
            }
            return null;
        }

        public Sesion getSesion(int sesionID, string personID)
        {
            _dataSet = new DataSet();
            Sesion sesion = new Sesion();
            try
            {
                MenuTutorHandler handler = new MenuTutorHandler();
                SqlCommand show = new SqlCommand("get_sesion_by_id", sqlConnection);
                SqlDataAdapter dataTableAdapter = new SqlDataAdapter(show);
                sqlConnection.Open();
                show.CommandType = CommandType.StoredProcedure;
                show.Parameters.Add("@session_id", SqlDbType.Int).Value = sesionID;
                var session_handler = new SesionHandler();

                using (SqlDataReader reader = show.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sesion = new Sesion 
                        {
                            id = Convert.ToInt32(reader["id"]),
                            estudiante_id = Convert.ToInt32(reader["estudiante_id"]),
                            tutoria_id = Convert.ToInt32(reader["id_tutoria"]),
                            privacidad = Convert.ToString(reader["privacidad"]),
                            modalidad = Convert.ToString(reader["modalidad"]),
                            direccion = Convert.ToString(reader["region_provinc"]) + ", " + Convert.ToString(reader["region_canton"]) + ", " + Convert.ToString(reader["region_distr"]) + ", " + Convert.ToString(reader["region_detalles"]) + ".",
                            enlace = Convert.ToString(reader["enlace"]),
                            estado_sesion = Convert.ToString(reader["estado_sesion"]),
                            fecha_inicio = Convert.ToString(reader["start_date"]),
                            fecha_fin = Convert.ToString(reader["end_date"]),
                            descripcion = Convert.ToString(reader["text"]),
                            nombre_tutor = Convert.ToString(reader["nombre"]),
                            apellido_tutor = Convert.ToString(reader["apellido"]),
                            nombre_curso = Convert.ToString(reader["nombre_curso"]),
                            sigla_curso = Convert.ToString(reader["sigla_curso"]),
                            contrasena = Convert.ToString(reader["contrasena"]),
                            tarifa_individual = Convert.ToInt32(reader["tarifa_individual"]),
                            tarifa_grupal = Convert.ToInt32(reader["tarifa_grupal"]),
                            cantidad_estudiantes = getEstudiantesInscritos(Convert.ToInt32(reader["id"]))
                        };
                    }
                    sesion.lista_asistentes = handler.getSessionAssistants(sesion.id, personID);
                }

            }
            catch (SqlException sqlException)
            {
                System.Diagnostics.Debug.WriteLine(sqlException.ToString());
            }
            sqlConnection.Close();
            return sesion;
        }

        public bool inscrito(int studentID, int sesionID)
        {
            SqlCommand get = new SqlCommand("es_asistente", connection);
            connection.Open();
            get.CommandType = CommandType.StoredProcedure;
            get.Parameters.AddWithValue("@estudiante_id", studentID);
            get.Parameters.AddWithValue("@sesion_id", sesionID);
            _adapter = new SqlDataAdapter(get);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            connection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                var asistentes = Convert.ToInt32(_dataSet.Tables[0].Rows[0]["asistentes"]);
                if (asistentes > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
