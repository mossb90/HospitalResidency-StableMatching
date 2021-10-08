using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidencyMATCH
{
    
    public static class MatchMaker
    {
        

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

        public static void MakeMatches( List<DoctorPreference> doctorPool,  List<HospitalPreference> hospitalPool)
        {
            // think about if ref's required here
            bool matchMade = false;
            foreach (DoctorPreference doctor in doctorPool)
            {
                if (doctor.isMatched == false)
                {
                    

                    foreach (int hospital in doctor.PreferredHospitals)
                    {
                        HospitalPreference currentHospital = hospitalPool.Find(x => x.HospitalID == hospital);
                        if ((currentHospital.PreferredDoctors.Contains(doctor.DoctorID)) && (currentHospital.Openings >= 1))
                        {
                            doctor.isMatched = true;
                            doctor.HospitalMatched = currentHospital.HospitalID;
                            currentHospital.ResidentsMatched.Add(doctor.DoctorID);
                            currentHospital.Openings -= 1;
                            matchMade = true;
                            break;
                        }
                        else if ((currentHospital.PreferredDoctors.Contains(doctor.DoctorID)) && (currentHospital.PreferredDoctors[0] == doctor.DoctorID))
                        {
                            doctor.isMatched = true;
                            doctor.HospitalMatched = currentHospital.HospitalID;
                            int lowestRankDoctorIDIndex = -1;    // Index of lowest ranked doctor already matched that will get bumped
                            foreach (int alreadyMatched in currentHospital.ResidentsMatched)
                            {
                                if (currentHospital.PreferredDoctors.IndexOf(alreadyMatched) > lowestRankDoctorIDIndex)
                                {
                                    lowestRankDoctorIDIndex = currentHospital.PreferredDoctors.IndexOf(alreadyMatched);
                                }
                            }
                            currentHospital.ResidentsMatched.Remove(currentHospital.PreferredDoctors[lowestRankDoctorIDIndex]);
                            currentHospital.ResidentsMatched.Add(doctor.DoctorID);
                            DoctorPreference bumpedDoctor = doctorPool.Find(x => x.DoctorID == currentHospital.PreferredDoctors[lowestRankDoctorIDIndex]);
                            bumpedDoctor.isMatched = false;
                            bumpedDoctor.HospitalMatched = null;
                            matchMade = true;
                        }

                    }
                }
            }
            if((matchMade = true) && doctorPool.Any(m => !(bool)m.isMatched)){
                MakeMatches(doctorPool, hospitalPool);
            }
        }
    }
}