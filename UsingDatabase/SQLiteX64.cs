using System;
using System.Data.SQLite;
using System.Data;
using System.Collections;
using System.IO;

namespace UsingDatabase
{
    public class SQLiteX64
    {
        private SQLiteConnection conn;
        public string Status { get; set; }
        public bool Connection { get; set; }
        public DataTable Dt { get; set; }
        public DataSet Ds { get; set; }

        public SQLiteX64()
        {
            conn = null;
            Status = "";
            Connection = false;
            Dt = new DataTable();
            Ds = new DataSet();
        }

        public void ConnectDB(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    conn = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                }
                else
                {
                    SQLiteConnection.CreateFile(path);
                    conn = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                }

                Connection = true;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
                Connection = false;
            }

            ConnectionTest();
        }

        public void ConnectionTest()
        {
            try
            {
                conn.Open();
                Connection = true;
                Status = "Connection Successful";
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

                SQLiteCommand command = new SQLiteCommand(sql, conn);
                int result = command.ExecuteNonQuery();

                Status = "Affected rows : " + result.ToString();
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

        public void ExecuteDataTable(string sql)
        {
            try
            {
                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
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

        public void ExecuteDataSet(string sql)
        {
            try
            {
                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                Ds = new DataSet();
                adapter.Fill(Ds, "LoadDataBinding");
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

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
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
