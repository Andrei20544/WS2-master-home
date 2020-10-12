using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
using WSHospital.View;

namespace WSHospital
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private Users user;
        private int Age;
        public UserWindow()
        {
            InitializeComponent();
        }
        public UserWindow(Users u)
        {
            InitializeComponent();
            user = u;
            NAME.Text = user.FirstName;
            LNAME.Text = user.LastName;          

            DateTime dateNow = DateTime.Now;
            DateTime birth = user.DateOfBirth.Value;

            Age = (int)dateNow.Subtract(birth).TotalDays/365;

            AGE.Text = Age.ToString();                   

            if (GetBitmap() == null)
            {

            }
            else
            {
                Img.Source = GetBitmap();
            }
            

            if(user.RoleID == 1)
            {
                work.Visibility = Visibility.Collapsed;
                LookOtch.Visibility = Visibility.Collapsed;
                CreateSch.Visibility = Visibility.Collapsed;
            }
            else if (user.RoleID == 2)
            {
                LookOtch.Visibility = Visibility.Collapsed;
                CreateSch.Visibility = Visibility.Visible;
            }
            else
            {
                bio.Visibility = Visibility.Collapsed;
                otch.Visibility = Visibility.Collapsed;
                work.Visibility = Visibility.Collapsed;
            }
        }

        private void bio_Click(object sender, RoutedEventArgs e)
        {
            ReceptionBioMaterialWindow RBMW = new ReceptionBioMaterialWindow(user, Age, GetBitmap());
            RBMW.Show();
        }

        public BitmapImage GetBitmap()
        {
            var Phot = user.Photo.Split('.');
            Uri uri = new Uri($@"{AppDomain.CurrentDomain.BaseDirectory}\Assets\{Phot[0]}.png");
            BitmapImage bitmap = new BitmapImage(uri);
            return bitmap;
        }

        private void otch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateSch_Click(object sender, RoutedEventArgs e)
        {
            using(ModelBD md = new ModelBD())
            {
                var fioPatserv = from p in md.Patients
                                 join n in md.NumberAnalyze on p.ID equals n.IDPatient
                                 join s in md.SetServicee on n.IDService equals s.ID
                                 select new
                                 {
                                     NamePat = p.FIO,
                                     NameServ = s.Name
                                 };

                ArrayList list = new ArrayList();
                
                foreach(var item in fioPatserv)
                {
                    list.Add(item.NamePat + " - " + item.NameServ);
                }

                StrahSch strah = new StrahSch(list);
                strah.Show();
            }
        }
    }
}
