using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        POEEntities poe = new POEEntities();
        user use = new user();

        public RegisterPage()
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

        //Register Button
        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            use.Username = tb1.Text;
            use.hashPassword = Encrypt(tb2.Password);

            if (tb1.Text == "" && tb2.Password == "" && tb3.Password == "")
            {
                MessageBox.Show("Username and Password are empty", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (tb2.Password == tb3.Password)
            {
                tb2.Password = Encrypt(tb2.Password);
                // use the db context to push the data
               
                poe.users.Add(use);
                int a = poe.SaveChanges();
                if (a > 0)
                {
                    MessageBox.Show("Your Account has been Successfully Created", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Your Account has been Unsuccessfully Created", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("Once Successfully Created Double-Click on Back into Login", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Password does not Match, Please Re-Enter", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                tb2.Password = " ";
                tb3.Password = " ";
                tb2.Focus();
            }
        }

        //Clear all textboxes on the page
        private void bt2_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Password = "";
            tb3.Password = "";
            tb1.Focus();
        }

        // When the user clicks on the label, it will take them to the login page once they have an account
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoginPage login = new LoginPage();
            login.Show();
            this.Close();
        }
    }
}
