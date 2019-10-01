using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace UsingDatabase
{
    class MySQL
    {
        private MySqlConnection conn;

        public DataTable Dt { get; set; }
        public DataSet Ds { get; set; }
        public bool Connection { get; set; }
        public string Status { get; set; }

        public MySQL()
        {
            conn = null;
            Status = "";
            Connection = false;
            Dt = new DataTable();
            Ds = new DataSet();
        }

        public void ConnectMySQL(string server, string port, string userId, string password, string schema)
        {
            string connectionString = String.Format("server={0};port={1};user id={2};password={3};database={4};", server, port, userId, password, schema);

            conn = new MySqlConnection(connectionString);
            ConnectionTest();
        }

        public void ConnectionTest()
        {
            try
            {
                conn.Open();
                Connection = true;
                Status = "Successful Connection";
            }
            catch (Exception ex)
            {
                Connection = false;
                Status = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);

                Status = command.ExecuteNonQuery() + " rows affected";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public void ExecuteQueryDataTable(string sql)
        {
            try
            {
                conn.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                Dt = new DataTable();
                adapter.Fill(Dt);

                Status = "Query Executed";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public void ExecuteQueryDataSet(string sql)
        {
            try
            {
                conn.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                Ds = new DataSet();
                adapter.Fill(Ds, "DataBinding");

                Status = "Query Executed";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ExecuteToGetCount(string sql)
        {
            try
            {
                conn.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                Dt = new DataTable();
                adapter.Fill(Dt);

                int count = (int)Dt.Rows[0].Field<Int64>(0);

                Status = "Query Executed";

                return count;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
