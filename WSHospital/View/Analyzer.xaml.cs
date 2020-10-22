using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
using System.Xml;

namespace WSHospital.View
{
    public class Services
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public double? cost { get; set; }
    }

    public partial class Analyzer : Window
    {
        public List<Services> GetServices;

        public Analyzer()
        {
            InitializeComponent();

            Services services = new Services();

            grid.DataContext = services;

            using (ModelBD md = new ModelBD())
            {
                var serv = from s in md.LabServices
                           join o in md.Orderr on s.ID equals o.IDService
                           where o.Status == "OK"
                           select new
                           {
                               ID = o.ID,
                               NameServ = s.Name,
                               Stat = o.Status,
                               cost = s.Cost
                           };

                GetServices = new List<Services>();

                foreach (var item in serv)
                {
                    services = new Services
                    {
                        id = item.ID,
                        name = item.NameServ,
                        status = item.Stat,
                        cost = item.cost,
                    };
                    GetServices.Add(services);
                    grid.Items.Add(services);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Структура запроса: JSON {“patient”: “{id}”, “services”: [{“serviceCode”:000}, {“serviceCode”:000}… ]}

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JsonRequests));

            for(int i = 0; i < grid.Columns.Count + 5; i++)
            {
                if (grid.SelectedItem == GetServices[i])
                {
                    //MessageBox.Show($"{GetServices[i].id}, {GetServices[i].name}");

                    using (ModelBD md = new ModelBD())
                    {
                        var serv = from ls in md.LabServices
                                   join o in md.Orderr on ls.ID equals o.IDService
                                   select new
                                   {
                                       IdPat = o.IDPatient,
                                       Servcode = ls.ServiceCode,
                                       Servid = ls.ID,

                                       idord = o.ID
                                   };

                        foreach (var item in serv)
                        {
                            if (item.idord == GetServices[i].id)
                            {
                                JsonRequests json = new JsonRequests()
                                {
                                    IDPat = item.IdPat,
                                    ServCode = item.Servcode,
                                    ServId = item.Servid
                                };

                                try
                                {
                                    XmlWriter writer = new XmlTextWriter(@"C:\\Users\\su.KBK\\Documents\\JsonSerialize.xml", null);
                                    serializer.WriteObject(writer, json);
                                    writer.Close();

                                    MessageBox.Show("200");
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show("400: " + ex.Message);
                                }

                            }
                            
                        }
                    }

                    break;
                }
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    [DataContract]
    public class JsonRequests
    {
        [DataMember]
        public int? IDPat { get; set; }
        [DataMember]
        public int ServId { get; set; }
        [DataMember]
        public double? ServCode { get; set; }
    }

}
