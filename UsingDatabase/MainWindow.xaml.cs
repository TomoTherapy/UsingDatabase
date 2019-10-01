using System;
using System.Threading;
using System.Windows;

namespace UsingDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteX64 lite;
        MySQL mysql;

        public MainWindow()
        {
            InitializeComponent();

            lite = new SQLiteX64();
            lite.ConnectDB(AppDomain.CurrentDomain.BaseDirectory + @"\test.db");

            mysql = new MySQL();
        }

        #region SQLite
        private void QueryExecute_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SQLiteExecuteQuery(Query_textBox.Text.Trim());
                Status_textBlock.Text = lite.Status;
            }
            catch (Exception ex)
            {
                Status_textBlock.Text = ex.Message;
            }
        }

        private void SQLiteExecuteQuery(string sql)
        {
            if (sql.ToUpper().Contains("SELECT"))
            {
                lite.ExecuteDataTable(sql);
                SQLite_dataGrid.ItemsSource = lite.Dt.DefaultView;
            }
            else
            {
                lite.ExecuteNonQuery(sql);
                Status_textBlock.Text = lite.Status;
            }
        }

        private void CreateTable_btn_Click(object sender, RoutedEventArgs e)
        {
            string sql = "CREATE TABLE EMPLOYEE( " + Environment.NewLine;
            sql += "NUM INTEGER PRIMARY KEY AUTOINCREMENT " + Environment.NewLine;
            sql += ", NAME VARCHAR(100)" + Environment.NewLine;
            sql += ", ADDRESS VARCHAR(200)" + Environment.NewLine;
            sql += ", DESCRIPTION VARCHAR(200)" + Environment.NewLine;
            sql += ", DEPARTMENT VARCHAR(50)" + Environment.NewLine;
            sql += ", SALARY INTEGER" + Environment.NewLine;
            sql += ")" + Environment.NewLine;

            try
            {
                SQLiteExecuteQuery(sql);
            }
            catch(Exception ex)
            {
                Status_textBlock.Text = ex.Message;
            }
        }

        private void Insert_btn_Click(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO EMPLOYEE(NAME, ADDRESS, DESCRIPTION, DEPARTMENT,SALARY) VALUES (" + Environment.NewLine;
            sql += "'" + Name_textBox.Text + "'" + Environment.NewLine;
            sql += ",'" + Address_textBox.Text + "'" + Environment.NewLine;
            sql += ",'" + Description_textBox.Text + "'" + Environment.NewLine;
            sql += ",'" + Department_textBox.Text + "'" + Environment.NewLine;
            sql += "," + Salary_textBox.Text + Environment.NewLine;
            sql += ")" + Environment.NewLine;

            try
            {
                SQLiteExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Status_textBlock.Text = ex.Message;
            }
        }
        #endregion

        #region MySQL
        private void MySQLCon_btn_Click(object sender, RoutedEventArgs e)
        {
            mysql.ConnectMySQL(Server_textBox.Text, Port_textBox.Text, User_textBox.Text, Password_passwordBox.Password, Schema_textBox.Text);

            MySQLStatus_textBlock.Text = mysql.Status;
            Password_passwordBox.Password = "";
        }

        private void MySQLExecute_btn_Click(object sender, RoutedEventArgs e)
        {
            if (mysql.Connection == false) return;

            string sql = MySQLQuery_textBox.Text.Trim().ToUpper();

            if (sql.Contains("SELECT"))
            {
                mysql.ExecuteQueryDataTable(sql);
                MySQL_dataGrid.ItemsSource = mysql.Dt.DefaultView;
            }
            else
            {
                mysql.ExecuteNonQuery(sql);
            }

            MySQLStatus_textBlock.Text = mysql.Status;
        }

        private void Password_passwordBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                MySQLCon_btn_Click(sender, e);
            }
        }
        #endregion

    }
}
