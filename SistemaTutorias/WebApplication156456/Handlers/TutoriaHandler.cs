using PruebaAzure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace PruebaAzure.Handlers
{
    public class TutoriaHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public TutoriaHandler()
        {
            rutaConexion = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
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

        public List<Tutoria> obtenerTutorias()
        {
            List<Tutoria> tutorias = new List<Tutoria>();
            string consulta = "SELECT * FROM TUTORIA";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                tutorias.Add(
                new Tutoria
                {
                    curso = Convert.ToString(columna["curso"]),
                    tutor = Convert.ToString(columna["tutor"]),
                    tipo_sesion = Convert.ToString(columna["tipo_sesion"]),
                    cantidad_estudiantes = Convert.ToInt16(columna["cantidad_estudiantes"]),
                    tarifa_individual = Convert.ToInt16(columna["tarifa_individual"]),
                    tarifa_grupal = Convert.ToInt16(columna["tarifa_grupal"]),
                    calificacion_tutoria = Convert.ToInt16(columna["calificacion_tutoria"])
                });
            }
            return tutorias;
        }

    }
}