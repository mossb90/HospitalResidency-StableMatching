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
        StableMatchEntities1 Context = new StableMatchEntities1();
        List<DoctorPreference> doctorPool = new List<DoctorPreference>();
        List<HospitalPreference> hospitalPool = new List<HospitalPreference>();
        MatchMaker match = new MatchMaker();
        //List<int> PreferredHospitals;

        public MainWindow()
        {
            InitializeComponent();
            ComboBoxLoad();
            
       

            foreach (DoctorPreference docEntry in Context.DoctorPreferences)
            {
                Console.WriteLine("Look there is DoctorID data: " + docEntry.DoctorID);
                doctorPool.Add(docEntry);              
            }

            foreach (HospitalPreference hospitalEntry in Context.HospitalPreferences)
            {
                hospitalPool.Add(hospitalEntry);
            }

        }

        //Combo Box Loads Names of Doctors to Select matches from
        public void ComboBoxLoad()
        {
            List<Doctor> docNames = Context.Doctors.ToList();
            cbxDoctorName.ItemsSource = docNames;
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //match.MakeMatches(doctorPool, hospitalPool);


        }


        public void DisplayResults()
        {
            var matchResults = Context.Docg
            text_results.Text = 
        }
        #region Unused controls 

        private void btn_radioByDoctor_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_radioByHospital_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void text_results_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        #endregion

  
    }
}
