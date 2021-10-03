using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaTutorias.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SistemaTutorias.Handlers
{
    public class TutoresHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public TutoresHandler()
        {
            rutaConexion = ConfigurationManager.ConnectionStrings["TutoresConnection"].ToString();
            conexion = new SqlConnection(rutaConexion);
        }

        private DataTable crearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

        public List<TutorModel> obtenerTodoslosPlanetas()
        {
            List<TutorModel> planetas = new List<TutorModel>();
            string consulta = "SELECT * FROM Planeta ";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                planetas.Add(
                new TutorModel
                {
                    nombre = Convert.ToString(columna["nombreTutor"]),
                    region = Convert.ToString(columna["region"]),
                    id = Convert.ToInt32(columna["tutorId"])
                });
            }
            return planetas;
        }
    }
}