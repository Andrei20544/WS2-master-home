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
    public class Services
    {
        public string name { get; set; }
        public string status { get; set; }
        public double? cost { get; set; }
    }

    public partial class Analyzer : Window
    {
        public Analyzer()
        {
            InitializeComponent();

            Services services = new Services();
            List<Services> list = new List<Services>();

            grid.DataContext = list;

            using (ModelBD md = new ModelBD())
            {
                var serv = from s in md.LabServices
                           join o in md.Orderr on s.ID equals o.IDService
                           where o.Status == "OK"
                           select new
                           {
                               NameServ = s.Name,
                               Stat = o.Status,
                               cost = s.Cost
                           };          

                foreach (var item in serv)
                {
                    services = new Services
                    {
                        name = item.NameServ,
                        status = item.Stat,
                        cost = item.cost
                    };
                    list.Add(services);
                }
            }
        }
    }
}
