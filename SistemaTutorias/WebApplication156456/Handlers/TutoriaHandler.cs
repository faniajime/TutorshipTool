using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApplication156456.Handlers
{
    public class TutoriaHandler
    {
        public string connection_path;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;
        public TutoriaHandler()
        {
            connection_path = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            connection = new SqlConnection(connection_path);
        }

        public IList<Tutoria> GetTutorshipsList(string cursoID)
        {
            IList<Tutoria> tutoriasList = new List<Tutoria>();
            _dataSet = new DataSet();

            try
            {
                connection.Open();
                SqlCommand show = new SqlCommand("get_tutorias_por_curso", connection);
                show.CommandType = CommandType.StoredProcedure;
                show.Parameters.AddWithValue("@cursoID", cursoID);
                show.ExecuteNonQuery();
                _adapter = new SqlDataAdapter(show);
                _adapter.Fill(_dataSet);
                connection.Close();

                if (_dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < _dataSet.Tables[0].Rows.Count; i++)
                    {
                        Tutoria obj = new Tutoria();
                        obj.id = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["id"]);
                        obj.tutorID = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["tutorID"]);
                        obj.cursoid = Convert.ToString(_dataSet.Tables[0].Rows[i]["cursoid"]);
                        obj.tipo_sesion = Convert.ToString(_dataSet.Tables[0].Rows[i]["tipo_sesion"]);
                        obj.cantidad_estudiantes = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["cantidad_estudiantes"]);
                        obj.tarifa_individual = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["tarifa_individual"]);
                        obj.tarifa_grupal = Convert.ToInt32(_dataSet.Tables[0].Rows[i]["tarifa_grupal"]);
                        obj.calificacion_tutoria = Convert.ToDouble(_dataSet.Tables[0].Rows[i]["calificacion_tutoria"]);
                        obj.tutor_nombre = Convert.ToString(_dataSet.Tables[0].Rows[i]["tutor_nombre"]);
                        obj.tutor_apellidos = Convert.ToString(_dataSet.Tables[0].Rows[i]["tutor_apellidos"]);
                        obj.disponibilidad = Convert.ToString(_dataSet.Tables[0].Rows[i]["disponibilidad"]).Replace("'", "");
                        obj.nombre = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombre"]);
                        obj.area = Convert.ToString(_dataSet.Tables[0].Rows[i]["nombre_area"]);
                        obj.stars = Convert.ToInt32(Convert.ToInt32(obj.calificacion_tutoria) / 2);
                        tutoriasList.Add(obj);
                    }
                }
                return tutoriasList;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                connection.Close();
            }
            return tutoriasList;
        }
    }
}