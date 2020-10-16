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
    public partial class ServOtch : Window
    {
        public ServOtch()
        {
            InitializeComponent();

            DatFirst.SelectedDate = DateTime.Parse("10.09.2020");
            DatLast.SelectedDate = DateTime.Parse("25.09.2020");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ModelBD md = new ModelBD())
            {
                var dat = md.Rendering.ToList();

                var SServ = from ss in md.SetServicee
                            join ls in md.LabServices on ss.ID equals ls.IDSetService
                            join r in md.Rendering on ls.IDSetService equals r.IdService
                            select new
                            {
                                ss.Name,
                                r.Period
                            };

                var CountPat = from r in md.Rendering
                               join o in md.Orderr on r.IdService equals o.IDService
                               select new
                               {
                                   r.Period
                               };

                int count = 0;

                foreach (var item in dat)
                {
                    if (DateTime.Parse(item.Period) > DatFirst.SelectedDate.Value && DateTime.Parse(item.Period) < DatLast.SelectedDate.Value)
                    {
                        count++;
                    }
                }
                KServ.Text = count.ToString();
                count = 0;

                foreach (var item in SServ.Distinct())
                {
                    if (DateTime.Parse(item.Period) > DatFirst.SelectedDate.Value && DateTime.Parse(item.Period) < DatLast.SelectedDate.Value)
                    {
                        count++;
                    }
                }
                KSserv.Text = count.ToString();
                count = 0;

                foreach (var item in CountPat.Distinct())
                {
                    if (DateTime.Parse(item.Period) > DatFirst.SelectedDate.Value && DateTime.Parse(item.Period) < DatLast.SelectedDate.Value)
                    {
                        count++;
                    }
                }

                KPat.Text = count.ToString();

                

                
            }
        }

        //public void GetGraph()
        //{
        //    int sum = int.Parse(KServ.Text) + int.Parse(KPat.Text) + int.Parse(AvgKPatDay.Text) + ServP.Items.Count;

        //    string[] otchName = new string[4] { one.Content.ToString(), two.Content.ToString(), thr.Content.ToString(), fr.Content.ToString() };
        //    string[] otchName_2 = new string[4] { KServ.Text, KPat.Text, AvgKPatDay.Text, ServP.Items.Count.ToString() };

        //    StackPanel stack = new StackPanel();
        //    stack.Orientation = Orientation.Horizontal;
        //    stack.VerticalAlignment = VerticalAlignment.Center;
        //    stack.HorizontalAlignment = HorizontalAlignment.Center;

        //    for (int i = 0; i < 4; i++)
        //    {
        //        Rectangle rectangle = new Rectangle();
        //        Label label = new Label();

        //        label.Content = otchName[i];
        //        label.Margin = new Thickness(5, 0, 0, 0);
        //        label.VerticalAlignment = VerticalAlignment.Bottom;
        //        label.RenderTransformOrigin = new Point(-90, 0);
        //        rectangle.Width = 5;
        //        rectangle.Height = int.Parse(otchName_2[i]) * 3;
        //        rectangle.Fill = Brushes.Black;

        //        stack.Children.Add(label);
        //        stack.Children.Add(rectangle);

        //    }

        //    canv.Children.Add(stack);
        //}
    }
}
