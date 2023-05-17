using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Galary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSet dataSet = null;
        private SqlDataAdapter dataAdapter = null;
        private int countWin = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(zapros.Text, con);
                DataTable table = new DataTable();
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());

                countWin++;
                TabItem itm = new TabItem();
                itm.Header = $"{countWin}";

                grid.ItemsSource = table.AsDataView();

                itm.Content = chernovik.Content;
                Controller.Items.Add(itm);
                con.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            zapros.Text = "select * from Picture_t";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            zapros.Clear();
            //gridTabl.ItemsSource = null;
        }
    }
}
