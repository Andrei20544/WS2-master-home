using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

                foreach (var item in comp)
                {
                    ListComp.Items.Add(item.compName);
                }
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
            bordD.Content = ListComp.SelectedItem.ToString();

            ListPat.Items.Clear();
            ListPatSum.Items.Clear();
            It.Content = null;

            using(ModelBD md = new ModelBD())
            {
                var otch = from o in md.Orderr
                           join p in md.Patients on o.IDPatient equals p.ID
                           join ls in md.LabServices on o.IDService equals ls.ID
                           join c in md.Company on p.IDCompany equals c.ID
                           where c.ID == p.IDCompany
                           select new
                           {
                               NamePat = p.FIO,
                               Serv = ls.Name,
                               Cost = ls.Cost,
                               Date = ls.Period
                           };

                var PatNam = md.Patients.ToList();
                

                string serv = "";
                foreach (var item in PatNam)
                {
                    foreach(var item1 in otch)
                    {
                        if (item.FIO == item1.NamePat)
                        {
                            serv += item1.Serv + $", \n {new string(' ', item1.NamePat.Length + 12)}";
                        }                       
                    }
                    var comp = md.Company.Where(p => p.Name.Equals(CompName.Content.ToString())).FirstOrDefault();
                    if (item.IDCompany == comp.ID)
                    {
                        ListPat.Items.Add(item.FIO + " - " + serv);
                    }                 
                    serv = "";
                }


                double? summ1 = 0;
                double? itog = 0;
                foreach (var item in PatNam)
                {
                    foreach (var item1 in otch)
                    {
                        if (item.FIO == item1.NamePat)
                        {
                            summ1 += item1.Cost;
                        }
                    }
                    var comp = md.Company.Where(p => p.Name.Equals(CompName.Content.ToString())).FirstOrDefault();

                    if(item.IDCompany == comp.ID)
                    {
                        ListPatSum.Items.Add(item.FIO + " - " + summ1);
                        list1.Add(item.FIO + " - " + summ1);
                        itog += summ1;
                    }                 
                    summ1 = 0;
                }

                It.Content = itog;

            }         
        }

        private PrintDialog print = new PrintDialog();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            print.PrintVisual(otch, "");
        }
        public SaveFileDialog saveFile = new SaveFileDialog();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            saveFile.Filter = "Text files(*.csv)|*.csv|All files(*.*)|*.*";

            if (saveFile.ShowDialog() == true)
            {
                string pathCsvFile = saveFile.FileName;

                string allItemsPat = String.Join(";\n", ListPat.Items.OfType<string>());
                string allItemsSum = String.Join("\n;", ListPatSum.Items.OfType<string>());


                using (StreamWriter sw = File.AppendText(pathCsvFile))
                {
                    sw.WriteLine($"Компания; {CompName.Content}\n" +
                        $"ФИО пациентов с оказанными услугами; {allItemsPat}\n" +
                        $"Стоимость услуг по каждому пациенту; {allItemsSum}\n" +
                        $"Итоговая стоимость; {It.Content}\n" +
                        $"Период оплаты; {Period.Content}");
                    sw.Close();
                }

                MessageBox.Show($"Файл CSV сохранен в папке: {pathCsvFile}");
            }
        }

        private void CompName_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            CombBoxCust.Visibility = Visibility.Hidden;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Period.Content = dat.SelectedDate;
        }
    }
}
