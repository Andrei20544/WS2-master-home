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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WSHospital.View;

namespace WSHospital
{
    public partial class UserWindow : Window
    {
        private static UserWindow instance;
        public static UserWindow getInst(Users U)
        {
            if (instance == null)
                instance = new UserWindow(U);
            return instance;
        }

        public static void NullInst()
        {
            instance = null;
        }


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
                bordWork.Visibility = Visibility.Collapsed;

                LookOtch.Visibility = Visibility.Collapsed;
                bordLookOtch.Visibility = Visibility.Collapsed;

                CreateSch.Visibility = Visibility.Collapsed;
                bordCreateScg.Visibility = Visibility.Collapsed;
            }
            else if (user.RoleID == 2)
            {
                LookOtch.Visibility = Visibility.Visible;
                bordLookOtch.Visibility = Visibility.Visible;

                CreateSch.Visibility = Visibility.Visible;
                bordCreateScg.Visibility = Visibility.Visible;
            }
            else
            {
                bio.Visibility = Visibility.Collapsed;
                bordBio.Visibility = Visibility.Collapsed;

                otch.Visibility = Visibility.Collapsed;
                bordOtch.Visibility = Visibility.Collapsed;

                work.Visibility = Visibility.Collapsed;
                bordWork.Visibility = Visibility.Collapsed;
            }
        }

        private void bio_Click(object sender, RoutedEventArgs e)
        {
            ReceptionBioMaterialWindow RBMW = ReceptionBioMaterialWindow.GetInst();
            RBMW.Show();
        }

        public BitmapImage GetBitmap()
        {
            var Phot = user.Photo.Split('.');
            Uri uri = new Uri($@"{AppDomain.CurrentDomain.BaseDirectory}\Assets\{Phot[0]}.png");
            BitmapImage bitmap = new BitmapImage(uri);
            return bitmap;
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

                var SetLabServ = from n in md.NumberAnalyze
                                 join ss in md.LabServices on n.IDService equals ss.ID
                                 join p in md.Patients on n.IDPatient equals p.ID
                                 where ss.ID == n.IDService
                                 select new
                                 {
                                     NamePat = p.FIO,
                                     NameServ = ss.Name
                                 };

                var patName = md.Patients.ToList();

                ArrayList list = new ArrayList();

                string nam = "";
                foreach(var item in patName)
                {
                    foreach(var item1 in SetLabServ)
                    {
                        if (item1.NamePat == item.FIO)
                        {
                            nam += item1.NameServ + ", ";
                        }                      
                    }
                    list.Add(item.FIO + " - " + nam);
                    nam = "";
                }

                StrahSch strah = new StrahSch(list);
                strah.Show();
            }
        }

        private void work_Click(object sender, RoutedEventArgs e)
        {
            Analyzer analyzer = new Analyzer();
            analyzer.Show();
        }

        private void LookOtch_Click(object sender, RoutedEventArgs e)
        {
            ServOtch serv = new ServOtch();
            serv.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NullInst();
        }

        private void ex_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
