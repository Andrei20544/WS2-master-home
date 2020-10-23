using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        public int sum;

        public Analyzer()
        {
            sum = 0;
            InitializeComponent();         

            Services services = new Services();

            grid.DataContext = services;

            using (ModelBD md = new ModelBD())
            {
                var serv = from s in md.LabServices
                           join o in md.Orderr on s.ID equals o.IDService
                           where o.Status == "OK" || o.Status == "IN PROGRESS"
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
                    sum++;
                    GetServices.Add(services);
                    grid.Items.Add(services);
                }
            }
        }

        public string GetNamPat(ModelBD md, int i)
        {
            int OID = GetServices[i].id;
            var idOrd = md.Orderr.Where(p => p.ID.Equals(OID)).FirstOrDefault();
            int OID1 = int.Parse(idOrd.IDPatient.ToString());
            var NamPat = md.Patients.Where(p => p.ID.Equals(OID1)).FirstOrDefault();

            return NamPat.FIO;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JsonRequests));

            for(int i = 0; i < sum; i++)
            {
                if (grid.SelectedItem == GetServices[i])
                {
                    if (GetServices[i].status == "IN PROGRESS")
                    {
                        using (ModelBD md = new ModelBD())
                        {                         
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://localhost:44323/api/orders/{GetNamPat(md,i)}");
                            WebResponse response = request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        string[] masJson = line.Split(',');

                                        if (masJson.Length >= 3)
                                        {
                                            GetId(masJson, 2, 1);
                                        }
                                        else
                                        {
                                            GetId(masJson, 1, 2);
                                        }

                                     
                                    }
                                }
                                response.Close();
                            }
                            //RequestAsync();
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            using (ModelBD md = new ModelBD())
                            {


                                MessageBox.Show($"{GetServices[i].id}" + " " + $"{GetServices[i].name}");

                                string URLSite = $"https://localhost:44323/api/orders/{GetNamPat(md,i)}";
                                StreamWriter sw = null;

                                string PostData = "Posted=" + HttpUtility.UrlEncode("True") + "&X=" + HttpUtility.UrlEncode("Value");

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLSite);
                                request.Method = "POST";
                                request.ContentLength = PostData.Length;
                                request.ContentType = "application/x-www-form-urlencoded";
                                sw = new StreamWriter(request.GetRequestStream());

                                byte[] sendBuffer = Encoding.ASCII.GetBytes(PostData);

                                sw.Write(PostData);
                                sw.Close();

                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                StreamReader strData = new StreamReader(response.GetResponseStream(), Encoding.ASCII);

                                string outHtml = strData.ReadToEnd();
                                MessageBox.Show(outHtml);

                                strData.Dispose();
                                strData.Close();
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //using (ModelBD md = new ModelBD())
                        //{
                        //    var serv = from ls in md.LabServices
                        //               join o in md.Orderr on ls.ID equals o.IDService
                        //               select new
                        //               {
                        //                   IdPat = o.IDPatient,
                        //                   Servcode = ls.ServiceCode,
                        //                   Servid = ls.ID,

                        //                   idord = o.ID
                        //               };

                        //    foreach (var item in serv)
                        //    {
                        //        if (item.idord == GetServices[i].id)
                        //        {
                        //            JsonRequests json = new JsonRequests()
                        //            {
                        //                IDPat = item.IdPat,
                        //                ServCode = item.Servcode,
                        //                ServId = item.Servid
                        //            };

                        //            try
                        //            {
                        //                XmlWriter writer = new XmlTextWriter(@"C:\\Users\\su.KBK\\Documents\\JsonSerialize.xml", null);
                        //                serializer.WriteObject(writer, json);
                        //                writer.Close();

                        //                MessageBox.Show("200");
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                MessageBox.Show("400: " + ex.Message);
                        //            }

                        //        }

                        //    }
                        //    StatAnalizy();
                        //}
                        break;
                    }                
                }
            }           
        }

        public void GetId(string[] masJson, int num, int num_2) { 
            using (ModelBD md = new ModelBD())
            {             
                int msPatId = int.Parse(masJson[0].Split(':')[1]);
                int msLsServId = int.Parse(masJson[1].Split(':')[2].Substring(0, 1));
                int msSetServId = int.Parse(masJson[num].Split(':')[num_2].Substring(0, 1));

                var labserv = md.LabServices.Where(p => p.ID.Equals(msLsServId)).FirstOrDefault();
                var patient = md.Patients.Where(p => p.ID.Equals(msPatId)).FirstOrDefault();
                var setserv = md.SetServicee.Where(p => p.ID.Equals(msSetServId)).FirstOrDefault();

                if (num == 2)
                {
                    MessageBox.Show($"FIO: {patient.FIO}, IDSetServ: {setserv.ID}, IDLabServ: {labserv.ID}");
                }
                else
                {
                    MessageBox.Show($"FIO: {patient.FIO}, IDSetServ: {setserv.ID}");
                }

                try
                {
                    int? idpat = msPatId;
                    var order = md.Orderr.Where(p => p.IDPatient.Equals(idpat)).FirstOrDefault();

                    Orderr orderr = new Orderr
                    {
                        IDPatient = order.IDPatient,
                        IDService = order.IDService,
                        Status = "IN PROGRESS"
                    };

                    md.Entry(orderr).State = System.Data.Entity.EntityState.Modified;
                    md.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Clipboard.SetText(ex.Message);
                }

                

            }
        }

        private async void StatAnalizy()
        {
            Prog.Value = 0;
            Coml.Content = "";

            for(int i = 0; i < 30; i++)
            {
                await Task.Delay(1000);
                Coml.Content = "in progress";   
                Prog.Value += 1;
            }

            //Получение результата

            Coml.Content = "End";
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
