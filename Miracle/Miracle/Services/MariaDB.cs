using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Miracle.Services
{
    public class MariaDB
    {
        private static readonly MariaDB instance = new MariaDB();

        public static MariaDB Instance
        {
            get
            {
                return instance;
            }
        }

        #region 생성자
        public MariaDB()
        {

        }
        #endregion


        #region GetDT_SP...Parameter None
        public DataTable GetDT_SP(string sp_name, string p_name1, object p_val1)
        {
            DataSet DS = new DataSet();

            try
            {
                using (var connection = new MySqlConnection(G.DBConString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(sp_name, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter(p_name1, p_val1));

                    MySqlDataAdapter adpt = new MySqlDataAdapter
                    {
                        SelectCommand = command
                    };

                    adpt.Fill(DS, "Result");
                }
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return DS.Tables["Result"];
        }
        #endregion

        #region GetDT_SP...Parameter None
        public DataTable GetDT_SP(string sp_name, List<MySqlParameter> paraList)
        {
            DataSet DS = new DataSet();

            try
            {
                using (var connection = new MySqlConnection(G.DBConString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(sp_name, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var item in paraList)
                    {
                        command.Parameters.Add(item);
                    }

                    MySqlDataAdapter adpt = new MySqlDataAdapter
                    {
                        SelectCommand = command
                    };

                    adpt.Fill(DS, "Result");
                }
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return DS.Tables["Result"];
        }
        #endregion

        public DataTable GetDT_SP(string sp_name)
        {
            DataSet DS = new DataSet();

            try
            {
                using (var connection = new MySqlConnection(G.DBConString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(sp_name, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    MySqlDataAdapter adpt = new MySqlDataAdapter
                    {
                        SelectCommand = command
                    };

                    adpt.Fill(DS, "Result");
                }
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return DS.Tables["Result"];
        }

    }
}
