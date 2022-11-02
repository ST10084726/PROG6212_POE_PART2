using System;
using System.Collections.Generic;
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

namespace PROG6212_POE_pt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global instantiation
        SqlConnection con = new SqlConnection("Data source = (localdb)\\MSSQLLocalDB; Initial Catalog = POE; Integrated Security = true ");
        public string username;
        public int userID;
        module mod = new module();
        public MainWindow()
        {
            InitializeComponent();
            bindGridView();
            //when the form loads the db should pull 
        }

        public void bindGridView()
        {
            // use of Linq
            POEEntities poe = new POEEntities();
            var userResults = from u in poe.modules
                              //where u.UserId == userID
                              select new { u.moduleID, u.moduleCode, u.moduleName, u.credits, u.hrs, u.startDate, u.Weeks, u.studyHrs };
            Datagrid1.ItemsSource = userResults.ToList();
        }

        // Go back to login screen
        private void bt9_Click(object sender, RoutedEventArgs e)
        {
            LoginPage login = new LoginPage();
            login.Show();
            this.Hide();
        }

        private void bt10_Click(object sender, RoutedEventArgs e)
        {
            //Close application
            this.Close();
        }

        private void Datagrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        //Insert into the datagrid
        private void bt5_Click(object sender, RoutedEventArgs e)
        {
            POEEntities poe = new POEEntities();
            //validation 
            if (tb6.Text.Length == 0 || tb7.Text.Length == 0 || tb8.Text.Length == 0 || tb9.Text.Length == 0)
            {
                MessageBox.Show("Fields cannot be Left Blank >>>>");
            }
            else
            {
                // confirm if entry is correct before sending to the Datagrid
                if (MessageBox.Show("Is the entry correct?", "Cofirm Entry", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SqlCommand sqlCommand = new SqlCommand("Select UserId FROM users WHERE Username = @Username", con);
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Username", username);
                    con.Open();
                    var userId = sqlCommand.ExecuteScalar();
                    mod.UserId = poe.users.Where(u => u.Username == username).Select(u => u.UserId).First();
                    con.Close();

                    mod.moduleName = tb7.Text;
                    mod.moduleCode = tb6.Text;
                    mod.credits = Convert.ToInt32(this.tb8.Text);
                    mod.hrs = Convert.ToInt32(this.tb9.Text);
                    mod.startDate = date1.DisplayDate;
                    mod.Weeks = Convert.ToInt32(tb10.Text);
                    mod.studyHrs = CalculateMod.Class1.SelfStudies(Convert.ToInt32(tb8.Text), Convert.ToInt32(tb10.Text), Convert.ToInt32(tb9.Text));


                    mod.UserId = poe.users.Where(u => u.Username == username).Select(u => u.UserId).First();
                    
                    poe.modules.Add(mod);
                    int a = poe.SaveChanges();
                    if (a > 0)
                    {
                        MessageBox.Show("data added");
                        bindGridView();//refresh
                    }
                    else
                    {
                        MessageBox.Show("Failed ");
                    }

                     bindGridView();



                }



            }
        }

        //To calculate the remaining hours of studying
        private void bt8_Click(object sender, RoutedEventArgs e)
        {
            
            mod.studyHrs = CalculateMod.Class1.SelfStudies(Convert.ToInt32(tb8.Text), Convert.ToInt32(tb10.Text), Convert.ToInt32(tb9.Text));
            mod.remain = CalculateMod.Class1.remainingHours(mod.studyHrs, Convert.ToInt32(tb11.Text));

            tb12.Text = Convert.ToString(mod.remain);
        }
    }
}
