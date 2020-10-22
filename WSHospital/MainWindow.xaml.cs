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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSHospital.View;
using System.IO;
using System.Threading;

namespace WSHospital
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sign();
        }

        public void Sign()
        {
            if (LOG.Text.Length == 0 && PASS.Text.Length == 0)
            {
                MessageBox.Show("Введите логин и пароль");
            }
            else if (LOG.Text.Length == 0 && PASS.Text.Length != 0)
            {
                MessageBox.Show("Введите логин");
            }
            else if (LOG.Text.Length != 0 && PASS.Text.Length == 0)
            {
                MessageBox.Show("Введите пароль");
            }
            else
            {
                using (ModelBD md = new ModelBD())
                {
                    var login = md.Users.FirstOrDefault(p => p.Login.Equals(LOG.Text));
                    var password = md.Users.FirstOrDefault(p => p.Password.Equals(PASS.Text));

                    if (login == null && password == null)
                    {
                        MessageBox.Show("Такого логина и пароля не существует");
                    }
                    else if (login == null && password != null)
                    {
                        MessageBox.Show("Неправильный логин");
                    }
                    else if (password == null && login != null)
                    {
                        MessageBox.Show("Неправильный пароль");
                    }
                    else
                    {
                        Users user = md.Users.Where(p => p.Login.Equals(LOG.Text) && p.Password.Equals(PASS.Text)).FirstOrDefault();
                        UserWindow userWindow = UserWindow.getInst(user);
                        userWindow.Show();
                    }
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddStuf addStuf = new AddStuf();
            addStuf.Show();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
