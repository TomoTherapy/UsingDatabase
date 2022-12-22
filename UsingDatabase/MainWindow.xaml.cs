using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

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
            mysql = new MySQL();
        }

        #region SQLite
        private void OpenDatabase_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog() { OverwritePrompt = false, CheckFileExists = false };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lite.ConnectDB(dialog.FileName);
            }

        }

        private void QueryExecute_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!lite.Connection)
            {
                MessageBox.Show("DB 연결안됨");
                return;
            }

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
            if (!lite.Connection)
            {
                MessageBox.Show("DB 연결안됨");
                return;
            }

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

        private void csv_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
