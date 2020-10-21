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
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;

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
            public string DayAndMonth { get; set; }
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

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Collapsed;
            tab.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Visible;
            tab.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grid.Items.Clear();

            chart.Series.Clear();
            chart.ChartAreas.Clear();

            using (ModelBD md = new ModelBD())
            {
                var dat = md.Rendering.ToList();

                var SServ = from r in md.Rendering
                            join ls in md.LabServices on r.IdService equals ls.ID
                            select new
                            {
                                r.Period,
                                ls.Name
                            };

                var CountPat = from r in md.Rendering
                               join o in md.Orderr on r.IdService equals o.IDService
                               select new
                               {
                                   r.Period
                               };

                var dat_1 = from r in md.Rendering
                            select new
                            {
                                datt = r.Period
                            };

                int count = 0;
                int count_2 = 0;

                int countPat = 0;
                int countServ = 0;

                foreach (var item in dat_1.Distinct())
                {
                    if (DateTime.Parse(item.datt).Day >= DatFirst.SelectedDate.Value.Day && DateTime.Parse(item.datt).Day <= DatLast.SelectedDate.Value.Day)
                    {
                        foreach (var item1 in CountPat)
                        {
                            DateTime date = DateTime.Parse(item1.Period);

                            if (date.Day.Equals(DateTime.Parse(item.datt).Day))
                            {
                                count++;
                            }
                        }

                        foreach (var item1 in SServ)
                        {
                            DateTime date = DateTime.Parse(item1.Period);
                            if (date.Day.Equals(DateTime.Parse(item.datt).Day))
                            {
                                count_2++;
                            }
                        }

                        GetTAble get = new GetTAble
                        {
                            Kserv = count_2,
                            Sserv = count_2,
                            Kpat = count,
                            DayAndMonth = "Day: " + DateTime.Parse(item.datt).Day + $" - Month: {DateTime.Parse(item.datt).Month}"
                        };

                        grid.Items.Add(get);

                        countPat += count;
                        countServ += count_2;

                        count = 0;
                        count_2 = 0;
                    }
                }

                KPat.Text = countPat.ToString();
                KServ.Text = countServ.ToString();
                KSserv.Text = countServ.ToString();



                int con = int.Parse(KServ.Text) + int.Parse(KSserv.Text) + int.Parse(KPat.Text);

                string[] str = new string[3] { "Кол-во оказанных услуг", "Кол-во пациентов", "Перечень оказанных услуг" };

                chart.ChartAreas.Add(new ChartArea("Default"));

                GenerateGraph(str[1], dat_1, CountPat, count);
                GenerateGraph(str[0], dat_1, SServ, count);
                GenerateGraph(str[2], dat_1, SServ, count);

            }
        }

        public void GenerateGraph(string NameSeries, IQueryable<dynamic> dat_1, IQueryable<dynamic> CountOther, int count)
        {
            count = 0;

            ArrayList list = new ArrayList();

            chart.Series.Add(new Series(NameSeries));

            foreach (var item in dat_1)
            {

                if (DateTime.Parse(item.datt).Day >= DatFirst.SelectedDate.Value.Day && DateTime.Parse(item.datt).Day <= DatLast.SelectedDate.Value.Day)
                {
                    foreach (var item1 in CountOther)
                    {
                        DateTime date = DateTime.Parse(item1.Period);
                        if (date.Day.Equals(DateTime.Parse(item.datt).Day))
                        {
                            count++;
                        }
                    }

                    if (!list.Contains(DateTime.Parse(item.datt).Day))
                    {
                        chart.Series[NameSeries].ChartArea = "Default";
                        chart.Series[NameSeries].ChartType = SeriesChartType.Column;

                        chart.Series[NameSeries].Points.AddXY("Day: " + DateTime.Parse(item.datt).Day + $" - Month: {DateTime.Parse(item.datt).Month}", count);

                        list.Add(DateTime.Parse(item.datt).Day);
                    }                   
                    count = 0;
                }         
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
