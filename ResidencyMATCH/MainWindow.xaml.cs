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
        MatchMaker match = new MatchMaker();
        //List<int> PreferredHospitals;

        public MainWindow()
        {
            InitializeComponent();
            ComboBoxLoad();
            ResetDatabase();

            //Fill list with data
            foreach (DoctorPreference docEntry in Context.DoctorPreferences)
            {
                doctorPool.Add(docEntry);              
            }

            foreach (HospitalPreference hospitalEntry in Context.HospitalPreferences)
            {
                hospitalPool.Add(hospitalEntry);
            }

            //Loads dictionaries and calls the MakeMatches method on window load 
            Dictionary<int, ArrayList> hospitalResidentsMatchDict = match.MakeMatches( doctorPool,  hospitalPool);
            UpdateDatabase();
            //DisplayResults();


        }

        //Combo Box Loads Names of Doctors to Select matches from
        public void ComboBoxLoad()
        {
            List<Doctor> docNames = Context.Doctors.ToList();
            cbxDoctorName.ItemsSource = docNames;
        }

        //combo box selection change to link chosen doctor to hospital/doctor information to the output screen 
        private void cbxDoctorName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            DisplayResults();

        }

        //Match button click to load database with changes, calling the UpdateDatabase Method 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            btn_match.IsEnabled = true;
            //DisplayResults();
            
        }

        //Update database on doctor matches
        public void UpdateDatabase()
        {

            
            var matched = Context.DoctorPreferences.First(d => d.isMatched == false);

            foreach (DoctorPreference doctor in doctorPool)
            {
                matched.isMatched = doctor.isMatched;
                matched.HospitalMatched = doctor.HospitalMatched;
            }
            
            var openings = Context.HospitalPreferences.FirstOrDefault();
            foreach (HospitalPreference hospital in hospitalPool)
            {
                
                openings.Openings = hospital.Openings;
                openings.isFullyStaffed = hospital.isFullyStaffed;
                
            }
           
            Context.SaveChanges();
        }

        public void ResetDatabase()
        {
            //var matched = Context.DoctorPreferences.First();
            foreach (DoctorPreference doctor in Context.DoctorPreferences)
            {
                doctor.isMatched = false;
                doctor.HospitalMatched = null;
            }
            //var resetOpenings = Context.HospitalPreferences.First();
            foreach (HospitalPreference hospital in Context.HospitalPreferences)
            {
                hospital.Openings = hospital.ResidentCapacity;
                hospital.isFullyStaffed = false;
            }
            Context.SaveChanges();
        }
        //Query to combine all doctor/hospital match results... Hospitals + Hospital preferences + Doctor Preferences + Doctor 
        public void DisplayResults()
        {
          

            var selectedDoctor = cbxDoctorName.SelectedItem as Doctor;
        
            var result = from dp in Context.DoctorPreferences
                          join h in Context.Hospitals on dp.HospitalMatched equals h.HospitalID
                          join d in Context.Doctors on dp.DoctorID equals d.DoctorID
                          where d.DoctorID == selectedDoctor.DoctorID
                         select new { d.FirstName, d.LastName, h.HospitalID, h.Name, h.City, h.State };

          
            result.ToList();

            foreach( var item in result)
            {
                txtDocFirstName.Text = item.FirstName;
                txtDocLastName.Text = item.LastName;
                txtHospitalName.Text = item.Name;
                txtHospitalCity.Text = item.City;
                txtHospitalState.Text = item.State;
            }

            var allResult = from dp in Context.DoctorPreferences
                         join h in Context.Hospitals on dp.HospitalMatched equals h.HospitalID
                         join d in Context.Doctors on dp.DoctorID equals d.DoctorID
                         select new { FirstName = d.FirstName, LastName = d.LastName, Hospital = h.Name, City = h.City, State = h.State };

            var results = allResult.ToList();
            grdAllResults.ItemsSource = results;
            





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
