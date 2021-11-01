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
            insert.Parameters.AddWithValue("@nombre", persona.nombre);
            insert.Parameters.AddWithValue("@apellido", persona.apellido);
            insert.Parameters.AddWithValue("@email", persona.email);
            insert.Parameters.AddWithValue("@contrasena", persona.contrasena);
            insert.Parameters.AddWithValue("@descripcion", persona.descripcion);
            insert.Parameters.AddWithValue("@persona_id", persona.persona_id);
            insert.ExecuteNonQuery();
            connection.Close();
        }
        public void crearEstudiante(Estudiante estudiante)
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("crear_estudiant", connection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@personID", estudiante.id);
            insert.ExecuteNonQuery();
            connection.Close();
        }
        public void crearTutor(Tutor tutor)
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("crear_persona", connection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@personID", tutor.id);
            insert.Parameters.AddWithValue("@region_provinc", tutor.region_provinc);
            insert.Parameters.AddWithValue("@region_canton", tutor.region_canton);
            insert.Parameters.AddWithValue("@region_distr", tutor.region_distr);
            insert.Parameters.AddWithValue("@region_detalles", tutor.region_detalles);
            insert.ExecuteNonQuery();
            connection.Close();
        }

    }
}


