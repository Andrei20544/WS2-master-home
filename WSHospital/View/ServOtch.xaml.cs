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
        public class GetTAble
        {
            public int Kserv { get; set; }
            public int Kpat { get; set; }
            public int AvgDay { get; set; }
            public int Sserv { get; set; }
        }

        public DateTime dat;
        public DateTime dat_1;

        public ServOtch()
        {
            InitializeComponent();

            GetTAble get = new GetTAble();

            grid.DataContext = get;

            DatFirst.SelectedDate = DateTime.Parse("10.09.2020");
            DatLast.SelectedDate = DateTime.Parse("25.09.2020");

        }

       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Collapsed;
            Canv.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Visible;
            Canv.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grid.Items.Clear();
            Canv.Children.Clear();

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


                var SServ2 = from r in md.Rendering
                             join ls in md.LabServices on r.IdService equals ls.ID
                             join o in md.Orderr on ls.ID equals o.IDService
                             join p in md.Patients on o.IDPatient equals p.ID
                             select new
                             {
                                 r.Period,
                                 p.FIO,
                                 ls.Name
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

                foreach (var item in SServ)
                {
                    if (DateTime.Parse(item.Period) > DatFirst.SelectedDate.Value && DateTime.Parse(item.Period) < DatLast.SelectedDate.Value)
                    {
                        count++;
                    }
                }
                KSserv.Text = count.ToString();
                count = 0;

                foreach (var item in CountPat)
                {
                    if (DateTime.Parse(item.Period) > DatFirst.SelectedDate.Value && DateTime.Parse(item.Period) < DatLast.SelectedDate.Value)
                    {
                        count++;
                    }
                }

                KPat.Text = count.ToString();

                GetTAble get = new GetTAble
                {
                    Kserv = int.Parse(KServ.Text),
                    Sserv = int.Parse(KSserv.Text),
                    Kpat = int.Parse(KPat.Text)
                };

                grid.Items.Add(get);


                int con = int.Parse(KServ.Text) + int.Parse(KSserv.Text) + int.Parse(KPat.Text);

                string[] str = new string[3] { "Кол-во оказанных услуг", "Кол-во пациентов", "Перечень оказанных услуг" };
                int[] con_1 = new int[3] { int.Parse(KServ.Text), int.Parse(KPat.Text), int.Parse(KSserv.Text) };

                //StackPanel stackPanel1 = new StackPanel();
                //stackPanel1.Width = 45;
                //stackPanel1.VerticalAlignment = VerticalAlignment.Bottom;
                //stackPanel1.Margin = new Thickness(0, 0, 0, 0);

                //for (int i = 0; i < con; i++)
                //{
                //    Rectangle rectangle = new Rectangle();
                //    rectangle.Fill = Brushes.Black;
                //    rectangle.Height = 1;
                //    rectangle.Width = 5;

                //    Label label = new Label();

                //    StackPanel stackPanel = new StackPanel();
                //    stackPanel.Orientation = Orientation.Horizontal;

                //    if (i % 10 == 0)
                //    {
                //        rectangle.Width = 15;
                //        rectangle.Margin = new Thickness(5, 0, 0, 0);
                //        label.Content = (((i - con) * -1) - 1).ToString();

                //        stackPanel.Children.Add(rectangle);
                //        stackPanel.Children.Add(label);
                //    }
                //    else stackPanel.Children.Add(rectangle);

                //    stackPanel1.Children.Add(stackPanel);
                //}

                //CanvChild.Children.Add(stackPanel1);


                //for (int i = 0; i < 3; i++)
                //{
                //    StackPanel stackPanel = new StackPanel();
                //    stackPanel.Orientation = Orientation.Vertical;
                //    stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                //    stackPanel.Margin = new Thickness(0, 0, 0, 0);

                //    Rectangle rectangle_1 = new Rectangle();
                //    rectangle_1.Width = 35;
                //    rectangle_1.Fill = Brushes.Black;

                //    TextBlock textBlock_1 = new TextBlock();

                //    rectangle_1.Height = con_1[i] + 20;
                //    textBlock_1.Text = str[i];
                //    textBlock_1.Width = 100;
                //    textBlock_1.HorizontalAlignment = HorizontalAlignment.Center;
                //    textBlock_1.TextWrapping = TextWrapping.Wrap;
                //    textBlock_1.Margin = new Thickness(5, 0, 0, 0);

                //    stackPanel.Children.Add(textBlock_1);
                //    stackPanel.Children.Add(rectangle_1);

                //    CanvChild.Children.Add(stackPanel);

                //}

                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 280;
                line.StrokeThickness = 3;
                line.Stroke = Brushes.Black;

                Canv.Children.Add(line);

                line = new Line();
                line.StrokeThickness = 3;
                line.Stroke = Brushes.Black;

                line.X1 = 0;
                line.X2 = 280;
                line.Y1 = 280;
                line.Y2 = 280;

                Canv.Children.Add(line);

                for (int i = 0; i < con; i++)
                {
                    Line line_1 = new Line();
                    line_1.StrokeThickness = 2;
                    line_1.Stroke = Brushes.Black;

                    if (i % 10 == 0)
                    {
                        line_1.X1 = 0;
                        line_1.X2 = 20;
                        line_1.Y1 = (280 - i * 2) / 2;
                        line_1.Y2 = (280 - i * 2) / 2;

                        Label label = new Label();
                        label.Content = /*(((i - con) * -1) - 1).ToString();*/ i.ToString();
                        label.Margin = new Thickness(20, ((280 - i * 2) / 2) - 13, 0, 0);

                        Canv.Children.Add(line_1);
                        Canv.Children.Add(label);
                    }
                    
                }

                for (int i = 0; i < 3; i++)
                {
                    Line line_1 = new Line();
                    line_1.StrokeThickness = 12;
                    line_1.Stroke = Brushes.Black;

                    Label label = new Label();
                    label.Content = con_1[i];
                    label.Margin = new Thickness(50 + i * 32, ((280 - con_1[i]) / 2) - 20, 0, 0);

                    line_1.X1 = 60 + (i * 32);
                    line_1.X2 = 60 + (i * 32);
                    line_1.Y1 = 280;
                    line_1.Y2 = (280 - con_1[i]) / 2;

                    Canv.Children.Add(label);
                    Canv.Children.Add(line_1);

                    label = new Label();
                    label.Content = str[i];
                    label.Margin = new Thickness(50 + i * 32, (285 + i * 10), 0, 0);

                    Canv.Children.Add(label);

                }               

            }
        }
    }
}
