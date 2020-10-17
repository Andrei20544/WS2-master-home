using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WSHospital.View
{
    /// <summary>
    /// Логика взаимодействия для AddStuf.xaml
    /// </summary>
    public partial class AddStuf : Window
    {
        public AddStuf()
        {
            InitializeComponent();

            using (ModelBD md = new ModelBD())
            {
                var Gend = from g in md.Users
                           select new
                           {
                               gender = g.Gender
                           };

                var Rol = from r in md.Rolee
                           select new
                           {
                               role = r.RoleName
                           };

                foreach (var item in Gend)
                {
                    GendR.Items.Add(item.gender);
                }

                foreach (var item in Rol)
                {
                    Rolee.Items.Add(item.role);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ModelBD md = new ModelBD())
                {
                    var rol = md.Rolee.Where(p => p.RoleName.Equals(Rolee.SelectedItem)).FirstOrDefault();

                    Users users = new Users
                    {
                        FirstName = FirstNam.Text,
                        LastName = LastNam.Text,
                        Login = Log.Text,
                        Password = Pass.Password,
                        Services = double.Parse(Serv.Text),
                        RoleID = rol.RoleID,
                        Photo = "",
                        Gender = GendR.SelectedItem.ToString(),
                        DateOfBirth = DatOf.SelectedDate
                    };

                    md.Users.Add(users);
                    md.SaveChanges();

                    MessageBox.Show("!!");

                    MainWindow main = new MainWindow();
                    main.LOG.Text = Log.Text;
                    main.PASS.Text = Pass.Password;

                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
