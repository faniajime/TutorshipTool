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
    public class RegistrationHandler
    {
        public string connection_path;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;

        public RegistrationHandler()
        {
            connection_path = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            connection = new SqlConnection(connection_path);
        }
        public void crearPersona(Persona persona)
        {
            connection.Open();
            
            SqlCommand insert = new SqlCommand("crear_persona", connection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.Add("@nombre", SqlDbType.VarChar).Value =  persona.nombre;
            insert.Parameters.Add("@apellido", SqlDbType.VarChar).Value = persona.apellido;
            insert.Parameters.Add("@email", SqlDbType.VarChar).Value = persona.email;
            insert.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = persona.contrasena;
            insert.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = persona.descripcion;
            insert.Parameters.Add("@persona_id", SqlDbType.VarChar).Value = persona.persona_id;
            insert.ExecuteNonQuery();
            connection.Close();
        }
        public void crearEstudiante(Estudiante estudiante)
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("crear_estudiante", connection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.Add("@personID", SqlDbType.VarChar).Value = estudiante.id;
            insert.ExecuteNonQuery();
            connection.Close();
        }
        public void crearTutor(Tutor tutor)
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("crear_tutor", connection);
            System.Diagnostics.Debug.WriteLine(tutor.id);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.Add("@personID", SqlDbType.VarChar).Value = tutor.id;
            insert.Parameters.Add("@region_provinc", SqlDbType.VarChar).Value = tutor.region_provinc;
            insert.Parameters.Add("@region_canton", SqlDbType.VarChar).Value = tutor.region_canton;
            insert.Parameters.Add("@region_distr", SqlDbType.VarChar).Value = tutor.region_distr;
            insert.Parameters.Add("@region_detalles", SqlDbType.VarChar).Value = tutor.region_detalles;
            insert.ExecuteNonQuery();
            connection.Close();
        }

    }
}


