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
    /// Логика взаимодействия для ReceptionBioMaterialWindow.xaml
    /// </summary>
    public partial class ReceptionBioMaterialWindow : Window
    {

        public CheckBox check;
        public ReceptionBioMaterialWindow(Users u, int age, BitmapImage ph)
        {
            InitializeComponent();               
        }

        public ReceptionBioMaterialWindow() {}

        public Random rnd;

        public string GetControlEan(string str)
        {
            int ch = 0;
            int nch = 0;
            for(int i = 1; i<6; i++)
            {
                ch += int.Parse(str.Substring(2 * i, 1));
                nch += int.Parse(str.Substring(2 * i - 1, 1));
            }
            ch += 3;
            int cntr = 10 * (ch + nch) % 10;

            return cntr == 10 ? "0" : cntr.ToString();
        }

        public string BarCodeGenerate()
        {
            rnd = new Random();
            long CodeZakaza = rnd.Next();
            long day = DateTime.Now.Millisecond;
            string prefix = CodeZakaza.ToString().Substring(0, 2);
            string ShCode = prefix.Length == 0 ? "40":prefix + ("0000000000" + CodeZakaza).Substring(("0000000000" + CodeZakaza).Length - 11);
            string StrShk = ShCode + GetControlEan(ShCode);

            return StrShk;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string barcode = BarCodeGenerate();

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            stackPanel.Margin = new Thickness(80, 0, 0, 0);
            stackPanel.Width = 520;
            stackPanel.Height = 140;

            for (int i = 0; i < barcode.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                Label label = new Label();
                label.VerticalAlignment = VerticalAlignment.Bottom;

                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Orientation = Orientation.Horizontal;
                stackPanel1.VerticalAlignment = VerticalAlignment.Bottom;

                rectangle.Fill = Brushes.Black;
                rectangle.Width = int.Parse(barcode[i].ToString());
                rectangle.Height = 100;

                label.Content = barcode[i];
                label.Margin = new Thickness(0, 20, 0, 0);

                stackPanel1.Children.Add(label);
                stackPanel1.Children.Add(rectangle);

                stackPanel.Children.Add(stackPanel1);
            }

            Shtr.Text = barcode;
            canv.Children.Add(stackPanel);     

        }

        public Orderr orderr;
        public NumberAnalyze numAn;
        public BioMaterial bioMaterial;

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (ModelBD md = new ModelBD())
            {

                try
                {
                    var IdPat = md.Patients.Where(p => p.FIO.Equals(FIO.Text)).FirstOrDefault();

                    var ServNam = md.SetServicee.Where(p => p.Name.Equals(NameServ.Text)).FirstOrDefault();

                    double? sum = 0;

                    var dop = DopServ.SelectedItems;
                    var cost = md.LabServices.ToList();

                    foreach (var item in dop)
                    {
                        foreach (var item1 in cost)
                        {
                            if (item.ToString() == item1.Name)
                            {
                                sum += item1.Cost;
                            }
                        }
                    }

                    orderr = new Orderr
                    {
                        IDPatient = IdPat.ID,
                        IDService = ServNam.ID,
                        Status = "IN PROGRESS"
                    };

                    md.Orderr.Add(orderr);
                    md.SaveChanges();

                    if (DopServ.Items != null)
                    {
                        var items = DopServ.SelectedItems;

                        foreach(var item in items)
                        {
                            bioMaterial = new BioMaterial
                            {
                                IDSetService = ServNam.ID,
                                BioCode = int.Parse(BioCodeA.Text),
                                BioName = item.ToString()
                            };

                            md.BioMaterial.Add(bioMaterial);
                            md.SaveChanges();
                        }                
                    }
                  
                    numAn = new NumberAnalyze
                    {
                        IDPatient = IdPat.ID,
                        IDService = ServNam.ID,
                    };

                    md.NumberAnalyze.Add(numAn);
                    md.SaveChanges();

                    MessageBox.Show("Данные успешно созранены в БД");

                    DateTime dat = DateTime.Now;

                    Order order = new Order(DopServ, sum, IdPat.ID);
                    order.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(ModelBD md = new ModelBD())
            {
                var pat = md.Patients.Where(p => p.FIO.Equals(FIO.Text)).FirstOrDefault();
                AddPatient addPatient = new AddPatient(pat.ID, Shtr.Text);
                addPatient.Show();
            }
        }

        private void Shtr_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Shtr.Text.Length != 0)
            {
                gen.IsEnabled = false;
            }
            else if (Shtr.Text.Length == 0)
            {
                gen.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient(Shtr.Text);
            addPatient.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
            printDialog.PrintVisual(canv, "");
        }     

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                return;
            }
            e.Handled = true;
        }

        public void GetSelectionChange(object selector, TextBox textInput)
        {
            string code = "";

            if (selector == null)
            {
                code = "";
            }
            else if (selector != null && textInput.Text != "")
            {
                code = selector.ToString().Split('-')[0].Trim();
                textInput.Text = code;
            }
        }

        public void GetVisibilityListBox(ListBox list, Visibility vis, TextBox text, Thickness thickness)
        {
            list.Visibility = vis;
            text.BorderThickness = thickness;
        }

        //BioCode
        private void BioCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (ModelBD md = new ModelBD())
            {
                var BioCodeB = from b in md.BioMaterial
                               where b.BioCode.ToString().Contains(BioCodeA.Text)
                               select new
                               {
                                   BioName = b.BioName,
                                   BioCode = b.BioCode
                               };

                spic.Items.Clear();

                if (BioCodeA.Text == "")
                {
                    GetVisibilityListBox(spic, Visibility.Collapsed, BioCodeA, new Thickness(1, 1, 1, 1));
                }
                else if (BioCodeA.Text != "")
                {
                    GetVisibilityListBox(spic, Visibility.Visible, BioCodeA, new Thickness(1, 1, 1, 0));

                    foreach (var item in BioCodeB)
                    {
                        if (BioCodeA.Text.Equals(item.BioCode.ToString()))
                        {
                            GetVisibilityListBox(spic, Visibility.Collapsed, BioCodeA, new Thickness(1, 1, 1, 1));
                        }
                    }



                    foreach (var item in BioCodeB)
                    {
                        spic.Items.Add(" " + item.BioCode + " - " + item.BioName);
                    }
                }
            }
        }

        private void spic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            GetSelectionChange(spic.SelectedItem, BioCodeA);
        }
        //

        //LevenshteinDistance
        static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;

        private static int LevenshteinDistance(string item, string nam)
        {
            var n = item.Length;
            var m = nam.Length;
            var matrix = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (int i = 0; i < n; i++)
            {
                matrix[i, 0] = i;
            }
            for (int j = 0; j < m; j++)
            {
                matrix[0, j] = j;
            }

            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    var substitutionCost = item[i - 1] == nam[j - 1] ? 0 : 1;

                    matrix[i, j] = Minimum(matrix[i - 1, j] + deletionCost,          // удаление
                                            matrix[i, j - 1] + insertionCost,         // вставка
                                            matrix[i - 1, j - 1] + substitutionCost); // замена
                }
            }

            return matrix[n - 1, m - 1];
        }
        //

        //FIO
        private void FIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (ModelBD md = new ModelBD())
                {
                    var fioPat = from p in md.Patients select p;

                    FioSpic.Items.Clear();

                    if (FIO.Text == "") GetVisibilityListBox(FioSpic, Visibility.Collapsed, FIO, new Thickness(1, 1, 1, 1));

                    else if (FIO.Text != "") 
                    {
                        GetVisibilityListBox(FioSpic, Visibility.Visible, FIO, new Thickness(1, 1, 1, 0));

                        foreach (var item in fioPat)
                        {
                            if (FIO.Text.Equals(item.FIO.ToString())) GetVisibilityListBox(FioSpic, Visibility.Collapsed, FIO, new Thickness(1, 1, 1, 1));
                        }
                    }


                    foreach (var item in fioPat)
                    {
                        if (FIO.Text == "")
                        {
                            break;
                        }
                        else
                        {
                            if (LevenshteinDistance(item.FIO.Split(' ')[0], FIO.Text) <= 3)
                            {
                                FioSpic.Items.Add(item.FIO);
                            }
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FioSpic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSelectionChange(FioSpic.SelectedItem, FIO);
        }
        //

        //
        private void NameServ_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (ModelBD md = new ModelBD())
                {
                    var NamServ = from s in md.SetServicee
                                  select new
                                  {
                                      ID = s.ID,
                                      ServName = s.Name
                                  };

                    DopServ.Items.Clear();
                    foreach (var item in NamServ)
                    {
                        if (NameServ.Text.Equals(item.ServName))
                        {
                            var ServNam = from s in md.LabServices
                                          where s.IDSetService == item.ID
                                          select new
                                          {
                                              NameServ = s.Name,
                                              ID = s.IDSetService
                                          };

                            foreach (var item1 in ServNam)
                            {
                                DopServ.Items.Add(item1.NameServ);
                            }
                        }
                    }

                    NamServSpic.Items.Clear();

                    if (NameServ.Text == "") GetVisibilityListBox(NamServSpic, Visibility.Collapsed, NameServ, new Thickness(1, 1, 1, 1));

                    else if (NameServ.Text != "")
                    {
                        GetVisibilityListBox(NamServSpic, Visibility.Visible, NameServ, new Thickness(1, 1, 1, 0));

                        foreach (var item in NamServ)
                        {
                            if (NameServ.Text.Equals(item.ServName.ToString())) GetVisibilityListBox(NamServSpic, Visibility.Collapsed, NameServ, new Thickness(1, 1, 1, 1));
                        }
                    }


                    foreach (var item in NamServ)
                    {
                        if (NameServ.Text == "")
                        {
                            break;
                        }
                        else
                        {
                            if (LevenshteinDistance(item.ServName, NameServ.Text) <= 3)
                            {
                                NamServSpic.Items.Add(item.ServName);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NamServSpic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSelectionChange(NamServSpic.SelectedItem, NameServ);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            using (ModelBD md = new ModelBD())
            {
                CheckBox check = new CheckBox();

                var dop = DopServ.SelectedItems;
                var cost = md.LabServices.ToList();

                double? sum = 0;

                foreach (var item in dop)
                {
                    foreach (var item1 in cost)
                    {
                        if (item.ToString() == item1.Name)
                        {
                            sum += item1.Cost;
                        }
                    }
                }

                summ.Text = sum.ToString();
            }
        }
        //


    }
}
