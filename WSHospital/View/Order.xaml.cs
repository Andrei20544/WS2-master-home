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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        private string link;
        private string services;

        public Order(ListBox serv, double? cost, int idpat, long SHtr)
        {
            InitializeComponent();

            using (ModelBD md = new ModelBD())
            {               
                var Ord = from p in md.Patients
                          join o in md.Orderr on p.ID equals o.IDPatient
                          join s in md.LabServices on o.IDService equals s.ID
                          join set in md.SetServicee on s.IDSetService equals set.ID
                          join b in md.BioMaterial on set.ID equals b.IDSetService
                          select new
                          {
                              IDpat = p.ID,
                              IDOrd = o.ID,
                              IDBio = b.IDBio,
                              PolNum = p.InsurancePolicy,
                              FIO = p.FIO,
                              DateOF = p.DateOfBirth
                          };

                foreach (var item in Ord)
                {
                    if (item.IDpat.Equals(idpat))
                    {
                        DateTime dat = DateTime.Now;

                        OrderDateOne.Text = DateTime.Now.ToString();
                        OrderNum.Text = item.IDOrd.ToString();
                        NumProb.Text = SHtr.ToString();
                        PoliceNum.Text = item.PolNum.ToString();
                        FIO.Text = item.FIO;
                        DateOfBirthP.Text = item.DateOF.ToString();

                        ServCount.Items.Clear();
                        link = "";
                        foreach (var item1 in serv.SelectedItems)
                        {
                            ServCount.Items.Add(item1);
                            services += item1.ToString() + ", ";
                        }
                        CostServ.Text = cost.ToString();
                    }
                }
            }
          
            link = $"{OrderDateOne.Text.Split(' ')[0]}E{OrderDateOne.Text.Split(' ')[1]}&{OrderNum.Text}&{NumProb.Text}&{PoliceNum.Text}&{FIO.Text}&{DateOfBirthP.Text}&{services}&{cost}";

        }

        public string GetBase64(string str)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(plainTextBytes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.PrintVisual(otch, "");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var lnk = "https://wsrussia.ru/?data=" + GetBase64(link);

            Clipboard.SetText(lnk);

            MessageBox.Show("Ссылка скопирована в буфер обмена: \n" + lnk);
        }
    }
}
