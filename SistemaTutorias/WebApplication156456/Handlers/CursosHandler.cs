using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using WebApplication156456.Models;
using System.Text;
using System.Diagnostics;

namespace WebApplication156456.Handlers
{

    public class CursosHandler
    {
        public string connection_path;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;
        public CursosHandler()
        {
            connection_path = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            connection = new SqlConnection(connection_path);
        }

        public IList<Cursos> GetCoursesList()
        {
            IList<Cursos> coursesList = new List<Cursos>();
            _dataSet = new DataSet();

            try
            {
                connection.Open();
                SqlCommand show = new SqlCommand("Courses_View_Edit", connection);
                show.CommandType = CommandType.StoredProcedure;
                show.Parameters.AddWithValue("@mode", "getCoursesList");
                show.ExecuteNonQuery();
                _adapter = new SqlDataAdapter(show);
                _adapter.Fill(_dataSet);
                connection.Close();

                if (_dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < _dataSet.Tables[0].Rows.Count; i++)
                    {
                        Cursos obj = new Cursos();
                        obj.id = Convert.ToString(_dataSet.Tables[0].Rows[i]["id"]);
                        obj.nombre = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombre"]);
                        obj.area_especialidad = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombre_area"]);
                        obj.detalles = Convert.ToString(_dataSet.Tables[0].Rows[i]["detalles"]);
                        //obj.tutores = Convert.ToString(_dataSet.Tables[0].Rows[i]["mobile"]);
                        coursesList.Add(obj);
                    }
                }
                return coursesList;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                connection.Close();
            }
            return coursesList;
        }

        public void addCourse (Cursos model)
        {
            
            connection.Open();
            SqlCommand insert = new SqlCommand("Course_View_Edit", connection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@mode", "addCourse");
            insert.Parameters.AddWithValue("@nombre", model.nombre);
            insert.Parameters.AddWithValue("@detalles", model.detalles);
            insert.Parameters.AddWithValue("@area_especialidad", model.area_especialidad);
            insert.ExecuteNonQuery();
            connection.Close();
        }

        public Cursos editById(int Id)
        {
            var model = new Cursos();             
            connection.Open();
            SqlCommand edit = new SqlCommand("Course_View_Edit", connection);
            edit.CommandType = CommandType.StoredProcedure;
            edit.Parameters.AddWithValue("@mode", "GetCourseById");
            edit.Parameters.AddWithValue("@id", Id);
            _adapter = new SqlDataAdapter(edit);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            connection.Close();
            if (_dataSet.Tables.Count > 0 && _dataSet.Tables[0].Rows.Count > 0)
            {
                model.id = Convert.ToString(_dataSet.Tables[0].Rows[0]["id"]);
                model.nombre = Convert.ToString(_dataSet.Tables[0].Rows[0]["nombre"]);
                model.detalles = Convert.ToString(_dataSet.Tables[0].Rows[0]["detalles"]);
                model.area_especialidad = Convert.ToString(_dataSet.Tables[0].Rows[0]["area_especialidad"]);
            }            
            return model;
        }

        public void updateCourse(Cursos model)
        {          
            connection.Open();
            SqlCommand update = new SqlCommand("Course_View_Edit", connection);
            update.CommandType = CommandType.StoredProcedure;
            update.Parameters.AddWithValue("@mode", "UpdateCourse");
            update.Parameters.AddWithValue("@nombre", model.nombre);
            update.Parameters.AddWithValue("@detalles", model.detalles);
            update.Parameters.AddWithValue("@area_especialidad", model.area_especialidad);
            update.Parameters.AddWithValue("@id", model.id);
            update.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteCourse(int id)
        {           
            connection.Open();
            SqlCommand delete = new SqlCommand("Course_View_Edit", connection);
            delete.CommandType = CommandType.StoredProcedure;
            delete.Parameters.AddWithValue("@mode", "DeleteCourse");
            delete.Parameters.AddWithValue("@id", id);
            delete.ExecuteNonQuery();
            connection.Close();
        }
    }
}