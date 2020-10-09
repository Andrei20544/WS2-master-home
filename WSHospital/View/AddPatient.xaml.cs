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
    /// Логика взаимодействия для AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        public AddPatient(string shtrih)
        {
            InitializeComponent();

            Shtrih.Text = shtrih;

            using (ModelBD md = new ModelBD())
            {
                var comp = from c in md.Company
                           select new
                           {
                               NameComp = c.Name,
                               IdComp = c.ID
                           };

                foreach(var item in comp)
                {
                    CompName.Items.Add(item.IdComp + ". "+ item.NameComp);
                }


                var pol = (from p in md.Patients select p.TypeOfPolicy).Distinct();

                foreach(var item in pol)
                {
                    PolName.Items.Add(item.ToString());
                }

            }

        }

        public AddPatient(int id, string shtr)
        {
            InitializeComponent();

            using (ModelBD md = new ModelBD())
            {
                var patient = md.Patients.Where(p => p.ID.Equals(id)).FirstOrDefault();

                pFIO.Text = patient.FIO;
                DaT.SelectedDate = patient.DateOfBirth;
                pEmail.Text = patient.Email;
                pPassportData.Text = patient.PassportData;
                pPhone.Text = patient.Phone.ToString();
                pInsPolicy.Text = patient.InsurancePolicy.ToString();
                PolName.SelectedItem = patient.TypeOfPolicy;
                CompName.SelectedItem = patient.Company;

                Shtrih.Text = shtr;
            }
        }

        public Patients pat;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(ModelBD md = new ModelBD())
            {
                try
                {
                    pat = new Patients
                    {
                        FIO = pFIO.Text,
                        DateOfBirth = DaT.DisplayDate,
                        Email = pEmail.Text,
                        PassportData = pPassportData.Text,
                        Phone = int.Parse(pPhone.Text),
                        InsurancePolicy = int.Parse(pInsPolicy.Text),
                        TypeOfPolicy = PolName.SelectedItem.ToString(),
                        IDCompany = int.Parse(CompName.SelectedItem.ToString().Split('.')[0])
                    };

                    md.Patients.Add(pat);
                    md.SaveChanges();

                    ReceptionBioMaterialWindow rbmw = new ReceptionBioMaterialWindow();
                    rbmw.FIO.Text = pat.FIO;


                    MessageBox.Show("Данные успешно созранены в БД");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
