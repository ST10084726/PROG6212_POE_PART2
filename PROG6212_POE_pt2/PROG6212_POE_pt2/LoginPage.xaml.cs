using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROG6212_POE_pt2
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        //global instantiation
        SqlConnection con = new SqlConnection("Data source = (localdb)\\MSSQLLocalDB; Initial Catalog = POE; Integrated Security = true ");
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        POEEntities poe = new POEEntities();

        public LoginPage()
        {
            InitializeComponent();
        }

        // Method to Hash encrypt the password the user as entered
        static string Encrypt(String Value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                //hash data
                byte[] data = md5.ComputeHash(utf8.GetBytes(Value));
                return Convert.ToBase64String(data);
            }
        }

        //Login Button
        private void bt3_Click(object sender, RoutedEventArgs e)
        {
            tb5.Password = Encrypt(tb5.Password);
            con.Open();
            string login = "SELECT * FROM dbo.users WHERE Username= '" + tb4.Text + "' and hashPassword= '" + tb5.Password + "'";
            com = new SqlCommand(login, con);
            SqlDataReader dr = com.ExecuteReader();

            //if the username and password match
            if (dr.Read() == true)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                mw.username = tb4.Text;
                this.Hide();
            }
            else
            {
                // if the username and password does not match
                MessageBox.Show("Invalid Username or Password, Please Try Again", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);

                //Reset textboxes
                tb4.Text = "";
                tb5.Password = "";
                tb4.Focus();
            }
        }

        // Clear the textboxes on the page
        private void bt4_Click(object sender, RoutedEventArgs e)
        {
            tb4.Text = "";
            tb5.Password = "";
            tb4.Focus();
        }

        // When the user clicks on the label, it will take them to the register page if they don't have an account
        private void Create_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RegisterPage register = new RegisterPage();
            register.Show();
            this.Hide();
        }

       
    }
}
