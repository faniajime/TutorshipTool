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
    public class HorarioHandler
    {
  
        public string connection_path;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;

        public HorarioHandler()
        {
            connection_path = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            connection = new SqlConnection(connection_path);
        }

        public bool insertHorario(Tutor tutor)
        {
            try
            {
                connection.Open();
                foreach (DiaHorarioModel dia in tutor.horario)
                {
                    using (SqlCommand insert = new SqlCommand("insertar_horario_tutor", connection))
                    {

                        insert.CommandType = CommandType.StoredProcedure;
                        insert.Parameters.Add("@tutor_id", SqlDbType.Int).Value = tutor.id;
                        insert.Parameters.Add("@dia", SqlDbType.TinyInt).Value = dia.dia;
                        insert.Parameters.Add("@hora_inicio", SqlDbType.Time).Value = dia.hora_inicio;
                        insert.Parameters.Add("@hora_fin", SqlDbType.Time).Value = dia.hora_fin;
                        insert.ExecuteNonQuery();
                    }
                }
                connection.Close();
                return true;
            }
            catch (SqlException e)
            {
                Console.Write(e);
                connection.Close();
                return false;

            }
        }

        public List<DiaHorarioModel> getHorario(int tutorid)
        {
            List<DiaHorarioModel> horario = new List<DiaHorarioModel>();
            _dataSet = new DataSet();
            try
            {
                connection.Open();
                SqlCommand get = new SqlCommand("get_horario_tutor", connection);
                get.CommandType = CommandType.StoredProcedure;
                get.Parameters.Add("@id_tutor", SqlDbType.Int).Value = tutorid;
                get.ExecuteNonQuery();
                _adapter = new SqlDataAdapter(get);
                _adapter.Fill(_dataSet);
                connection.Close();

                if (_dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < _dataSet.Tables[0].Rows.Count; i++)
                    {
                        DiaHorarioModel obj = new DiaHorarioModel();
                        obj.dia = Convert.ToInt16(_dataSet.Tables[0].Rows[i]["dia"]);
                        obj.hora_inicio = Convert.ToString(_dataSet.Tables[0].Rows[i]["hora_inicio"]);
                        obj.hora_fin = Convert.ToString(_dataSet.Tables[0].Rows[i]["hora_fin"]);
                        obj.disponible = true;
                        horario.Add(obj);
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                connection.Close();
            }
            return horario;
        }

        public bool updateHorarioTutor(Tutor tutor)
        {
            try
            {
                connection.Open();
                SqlCommand get = new SqlCommand("borrar_horario", connection);
                get.CommandType = CommandType.StoredProcedure;
                get.Parameters.Add("@id_tutor", SqlDbType.Int).Value = tutor.id;
                get.ExecuteNonQuery();
                connection.Close();
                insertHorario(tutor);
                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                connection.Close();
                return false;
            }

        }
    }
}
