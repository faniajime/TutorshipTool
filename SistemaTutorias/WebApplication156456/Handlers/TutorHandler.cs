using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication156456.Handlers
{
    public class TutorHandler
    {
        private SqlConnection sqlConnection;
        private string connectionRoute;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;
        private SqlConnection connection;

        public TutorHandler()
        {
            connectionRoute = ConfigurationManager.ConnectionStrings["PICONNECTION"].ToString();
            sqlConnection = new SqlConnection(connectionRoute);
        }

        public int getTutorId(string personaID)
        {

            sqlConnection.Open();
            SqlCommand insert = new SqlCommand("get_id_tutor", sqlConnection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@id_persona", personaID);
            _adapter = new SqlDataAdapter(insert);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            sqlConnection.Close();
            
            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToInt32(_dataSet.Tables[0].Rows[0]["id"]);
            }
            return -1;


        }
        public string getTutorIdFromPID(int tutorid)
        {

            sqlConnection.Open();
            SqlCommand insert = new SqlCommand("get_tutor_from_pid", sqlConnection);
            insert.CommandType = CommandType.StoredProcedure;
            insert.Parameters.AddWithValue("@id_tutor", tutorid);
            _adapter = new SqlDataAdapter(insert);
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet);
            sqlConnection.Close();

            if (_dataSet.Tables.Count > 0)
            {
                return Convert.ToString(_dataSet.Tables[0].Rows[0]["pid"]);
            }
            return "Not found";


        }
    }
}
