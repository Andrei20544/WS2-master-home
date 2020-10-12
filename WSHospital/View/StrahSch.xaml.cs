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
    /// Логика взаимодействия для StrahSch.xaml
    /// </summary>
    public partial class StrahSch : Window
    {
        public StrahSch()
        {
            InitializeComponent();

            using (ModelBD md = new ModelBD())
            {
                var comp = from c in md.Company
                           select new
                           {
                               compName = c.Name
                           };

                foreach(var item in comp)
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
            
        }
    }
}
