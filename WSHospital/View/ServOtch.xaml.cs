﻿using System;
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

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid.Items.Clear();

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

                Rectangle rectangle_1 = new Rectangle();
                Label label_1 = new Label();


                for (int i = 0; i <= con; i++)
                {

                }

            }
        }
    }
}
