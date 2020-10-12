using System;
using System.Collections;
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
    /// Логика взаимодействия для StrahSch.xaml
    /// </summary>
    public partial class StrahSch : Window
    {
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

                //var cost = from n in md.NumberAnalyze
                //           join b in md.BioMaterial on n.IDService equals b.IDSetService
                //           join ss in md.SetServicee on n.IDService equals ss.ID
                //           join ls in md.LabServices on ss.ID equals ls.IDSetService
                //           join p in md.Patients on n.IDPatient equals p.ID
                //           select new
                //           {
                //               NamePat = p.FIO,
                //               Cost = ls.Cost
                //           };

                var biocost = from setS in md.SetServicee
                              join LabS in md.LabServices on setS.ID equals LabS.IDSetService
                              select new
                              {

                              };

                foreach (var item in comp)
                {
                    ListComp.Items.Add(item.compName);
                }

                foreach (var item in cost)
                {
                    ListPatSum.Items.Add(item.NamePat + " - " + item.Cost);
                }


                foreach (var item in list)
                {
                    ListPat.Items.Add(item.ToString());
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
            CompName.Text = ListComp.SelectedItem.ToString();
        }

        private void CompName_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CombBoxCust.Visibility = Visibility.Hidden;
        }
    }
}
