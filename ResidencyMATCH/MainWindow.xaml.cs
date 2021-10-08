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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ResidencyMATCH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StableMatchEntities Context = new StableMatchEntities();
        List<DoctorPreference> doctorPool = new List<DoctorPreference>();
        List<HospitalPreference> hospitalPool = new List<HospitalPreference>();
        List<int> PreferredHospitals;

        public MainWindow()
        {
            InitializeComponent();
            
            foreach (DoctorPreference docEntry in Context.DoctorPreferences)
            {
                Console.WriteLine("Look there is DoctorID data: " + docEntry.DoctorID);
                doctorPool.Add(docEntry);

                //docEntry.PreferredHospitals.Add(docEntry.ChoiceHospital1);
                //if (docEntry.ChoiceHospital2 != null) docEntry.PreferredHospitals.Add((int)docEntry.ChoiceHospital2);
                //if (docEntry.ChoiceHospital3 != null) docEntry.PreferredHospitals.Add((int)docEntry.ChoiceHospital3);
                //if (docEntry.ChoiceHospital4 != null) docEntry.PreferredHospitals.Add((int)docEntry.ChoiceHospital4);
                //if (docEntry.ChoiceHospital5 != null) docEntry.PreferredHospitals.Add((int)docEntry.ChoiceHospital5);


            }

            foreach (HospitalPreference hospitalEntry in Context.HospitalPreferences)
            {

                hospitalPool.Add(hospitalEntry);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MatchMaker.MakeMatches(doctorPool, hospitalPool);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_radioByDoctor_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_radioByHospital_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
