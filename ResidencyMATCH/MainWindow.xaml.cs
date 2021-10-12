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
            Dictionary<int, ArrayList> hospitalResidentsMatchDict = match.MakeMatches(ref doctorPool, ref hospitalPool);
            UpdateDatabase();
            DisplayResults();


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
          

            var selectedDoctor = cbxDoctorName.SelectedItem as Doctor;

            MessageBox.Show(selectedDoctor.DoctorID.ToString());
        }

        //Match button click to load database with changes, calling the UpdateDatabase Method 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //DisplayResults();
            
            btn_match.IsEnabled = false;

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
           var dataResults = Context.Hospitals.Join(Context.HospitalPreferences, h => h.HospitalID, hp => hp.HospitalID, (h, hp) => new
            {
                HospitalName = h.Name,
                HospitalCity = h.City,
                HospitalState = h.State,
                HospitalID = h.HospitalID,

            }
            )
            .Join(Context.DoctorPreferences, hc => hc.HospitalID, m => m.HospitalMatched, (hc, m) => new
            {
                SelectedDoctor = m.HospitalMatched,
                DoctorID = m.DoctorID,
            }
            )
            .Join(Context.Doctors, d => d.DoctorID, sd => sd.DoctorID, (d, sd) => new
            {
                FirstName = sd.FirstName,
                LastName = sd.LastName,
                AlmaMater = sd.AlmaMater,
             
            }
            )
            .ToList();

            foreach (var item in dataResults)
            {

                //TODO >>>> Cant print out individual item lists 
                text_results.Text = text_results.Text + item.FirstName;
            }
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
