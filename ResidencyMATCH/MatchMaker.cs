using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidencyMATCH
{
    
    public class MatchMaker
    {

        public Dictionary<int, List<int>> preferredHospitalsDict = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> preferredDoctorsDict = new Dictionary<int, List<int>>();
        public Dictionary<int, ArrayList> hospitalResidentsMatchedDict = new Dictionary<int, ArrayList>();
        

        /*
        - populate preference lists for resident and hospitals inside 
          partial classes of DoctorPreferences and Hospital Preferences
                List<int> preferredHospitals = new List<int>();
                List<int> preferredDoctors = new List<int>();

        MakeMatches Pseudocode
        - set bool matchMade = false;  // flag to identify if match was made during iteration of the method
        - loop through each doctor in DoctorPreference   
            - if doctor.IsMatched == false
                -- loop through each hospital in residentPool.preferredHospitals list
                    -- if hospitalPool.preferredDoctors includes this doctor && hospitalPool.CurrentOpenings>=1
                            doctor.IsMatched = true;
                            doctor.HospitalMatch = hospital.HospitalID;
                            add doctor to hospital.ResidentsMatched
                            hospital.CurrentOpenings -= 1;
                            matchMade = true;
                            break
                    -- else if hospitalPool.preferredDoctors includes this doctor && this doctor is preferredDoctors[0]
                            doctor.IsMatched = true;
                            doctor.HospitalMatch = hospital.HospitalID;
                            remove lowest ranked match for this hospital from hospital.ResidentsMatched 
                                    change that_doctor.IsMatched = false and that_doctor.HospitalMatch = null;
                            matchMade = true
         - recursively run through the program again matchMade = true && if not all the doctorPool.IsMatched are true (if someone had their match taken away)
         */

        public void MakeMatches( List<DoctorPreference> doctorPool, List<HospitalPreference> hospitalPool)
        {
            
            // Populates preferredHospitalsDict with doctor's choices of hospitals for residencies 
            // key = DoctorID value = list of ChoiceHospital1-5
            foreach (DoctorPreference doctor in doctorPool)
            {
                List<int> hospitalChoices = new List<int>();
                hospitalChoices.Clear();
                
                hospitalChoices.Add(doctor.ChoiceHospital1);
                if (doctor.ChoiceHospital2 != null) hospitalChoices.Add((int)doctor.ChoiceHospital2);
                if (doctor.ChoiceHospital3 != null) hospitalChoices.Add((int)doctor.ChoiceHospital3);
                if (doctor.ChoiceHospital4 != null) hospitalChoices.Add((int)doctor.ChoiceHospital4);
                if (doctor.ChoiceHospital5 != null) hospitalChoices.Add((int)doctor.ChoiceHospital5);

                preferredHospitalsDict.Add(doctor.DoctorID, hospitalChoices);
            }

            // Populates preferredDoctorsDict with hospital's choices of doctors for residencies 
            // key = HospitalID value = list of ChoiceDoctor1-5
            foreach (HospitalPreference hospital in hospitalPool)
            {
                List<int> doctorChoices = new List<int>();
                doctorChoices.Clear();

                doctorChoices.Add(hospital.ChoiceDoctor1);
                if (hospital.ChoiceDoctor2 != null) doctorChoices.Add((int)hospital.ChoiceDoctor2);
                if (hospital.ChoiceDoctor3 != null) doctorChoices.Add((int)hospital.ChoiceDoctor3);
                if (hospital.ChoiceDoctor4 != null) doctorChoices.Add((int)hospital.ChoiceDoctor4);
                if (hospital.ChoiceDoctor5 != null) doctorChoices.Add((int)hospital.ChoiceDoctor5);

                preferredDoctorsDict.Add(hospital.HospitalID, doctorChoices);
            }

            bool matchMade = false;
            foreach (DoctorPreference doctor in doctorPool)
            {
                if (doctor.isMatched == false)
                {
                    List<int> preferredHospitals = preferredHospitalsDict[doctor.DoctorID];
                    foreach (int hospitalChoice in preferredHospitals)
                    {
                        List<int> preferredDoctors = preferredDoctorsDict[hospitalChoice];
                        HospitalPreference currentHospital = hospitalPool.Find(x => x.HospitalID == hospitalChoice);
                        if (preferredDoctors.Contains(doctor.DoctorID) && (currentHospital.Openings >= 1))
                        {
                            doctor.isMatched = true;
                            doctor.HospitalMatched = currentHospital.HospitalID;
                         
                            if (hospitalResidentsMatchedDict.ContainsKey(currentHospital.HospitalID))
                            {
                                hospitalResidentsMatchedDict[currentHospital.HospitalID].Add(doctor.DoctorID);
                            }
                            else hospitalResidentsMatchedDict.Add(currentHospital.HospitalID, new ArrayList() { doctor.DoctorID });
                            currentHospital.Openings -= 1;
                            matchMade = true;
                            break;
                        }
                        else if (preferredDoctors.Contains(doctor.DoctorID) && (preferredDoctors[0] == doctor.DoctorID))
                        {
                            doctor.isMatched = true;
                            doctor.HospitalMatched = currentHospital.HospitalID;
                            int lowestRankDoctorIDIndex = -1;    // Index of lowest ranked doctor already matched that will get bumped
                            foreach (int alreadyMatched in hospitalResidentsMatchedDict[currentHospital.HospitalID])
                            {
                                if (preferredDoctors.IndexOf(alreadyMatched) > lowestRankDoctorIDIndex)
                                {
                                    lowestRankDoctorIDIndex = preferredDoctors.IndexOf(alreadyMatched);
                                }
                            }
                            hospitalResidentsMatchedDict[currentHospital.HospitalID].Remove(preferredDoctors[lowestRankDoctorIDIndex]);
                            if (hospitalResidentsMatchedDict.ContainsKey(currentHospital.HospitalID))
                            {
                                hospitalResidentsMatchedDict[currentHospital.HospitalID].Add(doctor.DoctorID);
                            }
                            else hospitalResidentsMatchedDict.Add(currentHospital.HospitalID, new ArrayList() { doctor.DoctorID });
                            DoctorPreference bumpedDoctor = doctorPool.Find(x => x.DoctorID == preferredDoctors[lowestRankDoctorIDIndex]);
                            bumpedDoctor.isMatched = false;
                            bumpedDoctor.HospitalMatched = null;
                            matchMade = true;
                        }

                    }
                }
            }
            Console.WriteLine("finished first run through");
            if((matchMade = true) && doctorPool.Any(m => !(bool)m.isMatched)){
                MakeMatches(doctorPool, hospitalPool);
            }
            
            SaveChanges
            Console.WriteLine("Matching: ");
            foreach (KeyValuePair<int, ArrayList> pair in hospitalResidentsMatchedDict)
            {
                Console.WriteLine($"Matches for  {pair.Key}: ");
                foreach (var match in hospitalResidentsMatchedDict[pair.Key])
                {
                    Console.WriteLine(match);
                }
        
                Console.WriteLine();
            }
        }
        
    }
}