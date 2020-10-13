using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для StrahSch.xaml
    /// </summary>
    public partial class StrahSch : Window
    {
        private ArrayList list1 = new ArrayList();

        public StrahSch(ArrayList list)
        {
            InitializeComponent();

            using (ModelBD md = new ModelBD())
            {
                var comp = from c in md.Company
                           select new
                           {
                               compName = c.Name
                           };

                var cost = from n in md.NumberAnalyze
                           join b in md.BioMaterial on n.IDService equals b.IDSetService
                           join ss in md.SetServicee on n.IDService equals ss.ID
                           join ls in md.LabServices on b.BioName equals ls.Name
                           join p in md.Patients on n.IDPatient equals p.ID
                           select new
                           {
                               NamePat = p.FIO,
                               Cost = ls.Cost
                           };


                //var step = from b in md.BioMaterial
                //           join ss in md.SetServicee on b.IDSetService equals ss.ID
                //           join ls in md.LabServices on b.BioName equals ls.Name
                //           select new
                //           {
                //               BioCode = b.BioCode,
                //               NameServ = ls.Name,
                //               CostServ = ls.Cost
                //           };

               

                var PatNam = md.Patients.ToList();

                double? summ1 = 0;
                foreach(var item in PatNam)
                {
                    foreach (var item1 in cost)
                    {
                        if (item.FIO == item1.NamePat)
                        {
                            summ1 += item1.Cost;
                        }
                    }
                    ListPatSum.Items.Add(item.FIO + " - " + summ1);
                    list1.Add(item.FIO + " - " + summ1);
                    summ1 = 0;
                }
                



                foreach (var item in comp)
                {
                    ListComp.Items.Add(item.compName);
                }

                foreach (var item in list)
                {
                    ListPat.Items.Add(item.ToString());
                }

                double? summ = 0;
                foreach(var item in cost)
                {
                    summ += item.Cost;
                }
                It.Content = summ.ToString();
            }
        }

        private void bord_Click(object sender, RoutedEventArgs e)
        {
            if (CombBoxCust.Visibility == Visibility.Visible) CombBoxCust.Visibility = Visibility.Hidden;
            else CombBoxCust.Visibility = Visibility.Visible;
        }

        private void ListComp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompName.Content = ListComp.SelectedItem.ToString();
        }

        private PrintDialog print = new PrintDialog();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            print.PrintVisual(otch, "");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pathCsvFile = $"C:\\Users\\{Environment.UserName}.KBK\\Documents";

            using (var w = new StreamWriter(pathCsvFile))
            {
                for (int i = 0; i <= list1.Count; i++)
                {
                    var first = list1[i];
                    var line = string.Format($"{first}");
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }

        private void CompName_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            CombBoxCust.Visibility = Visibility.Hidden;
        }
    }
}
