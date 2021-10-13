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
        private int runCount = 0;
        private bool matchMade;

        /*
        MakeMatches Pseudocode for Implimenting Stable Match Algorithm
        - populate preferredHospitalsDict and preferredDoctorsDict in order to track assignments
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

        public Dictionary<int, ArrayList> MakeMatches( ref List<DoctorPreference> doctorPool, ref List<HospitalPreference> hospitalPool)
        {
            // Tracks how many times this method runs with each match request
            runCount++;
            matchMade = false;
            //Console.WriteLine("runCount = " + runCount);

            // Ensures dictionaries are only initialized on the first run through MatchMaker and not on recursive runs through
            if (runCount == 1)
            {
                if (hospitalResidentsMatchedDict.Count > 0) return hospitalResidentsMatchedDict;

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
            }
            

            // Loops through attempt match for each doctor
            foreach (DoctorPreference doctor in doctorPool)
            {
                if (doctor.isMatched == false)
                {
                    // Inspects each Doctor's residency hospital choices in order of preference and tries to make a match if 
                    // Doctor is on the hospital's list of preferred residency hires
                    foreach (int hospitalChoice in preferredHospitalsDict[doctor.DoctorID])
                    {
                        HospitalPreference currentHospital = hospitalPool.Find(x => x.HospitalID == hospitalChoice);
                        // Check for Doctor on hospital's preferred hire list AND that there is still an opening
                        if (preferredDoctorsDict[hospitalChoice].Contains(doctor.DoctorID) && (currentHospital.Openings >= 1))
                        {
                            doctor.isMatched = true;
                            doctor.HospitalMatched = currentHospital.HospitalID;

                            // Adds match to hospitalResidentsMatchedDict & reduces hospital openings by 1
                            if (hospitalResidentsMatchedDict.ContainsKey(currentHospital.HospitalID))
                            {
                                hospitalResidentsMatchedDict[currentHospital.HospitalID].Add(doctor.DoctorID);
                            }
                            else hospitalResidentsMatchedDict.Add(currentHospital.HospitalID, new ArrayList() { doctor.DoctorID });
                            currentHospital.Openings -= 1;
                            if (currentHospital.Openings == 0) currentHospital.isFullyStaffed = true;
                            matchMade = true;
                            break;
                        }
                        // Manages a match if there are no openings left but Doctor is ranked higher than another Doctor tentatively matched.
                        // to that hospital.  In that case the lowest ranked Doctor already matched to that hospital will get bumped and will
                        // be put pack int the pool of doctors that have to be matched
                        else if (preferredDoctorsDict[hospitalChoice].Contains(doctor.DoctorID) )
                        {                            
                            int lowestRankDoctorIDIndex = -1;    // Index of lowest ranked doctor already matched that will get bumped

                            // Determines lowest ranked Doctor of those already matched at for this hospital and determines if the current
                            // doctor has a higher rank
                            foreach (int alreadyMatched in hospitalResidentsMatchedDict[currentHospital.HospitalID])
                            {
                                if (preferredDoctorsDict[hospitalChoice].IndexOf(alreadyMatched) > lowestRankDoctorIDIndex)
                                {
                                    lowestRankDoctorIDIndex = preferredDoctorsDict[hospitalChoice].IndexOf(alreadyMatched);
                                }
                            }
                            if (lowestRankDoctorIDIndex > preferredDoctorsDict[hospitalChoice].IndexOf(doctor.DoctorID))
                            {
                                // Removes lowest ranking Doctor from the hospitalResidentsMatchedDict
                                hospitalResidentsMatchedDict[currentHospital.HospitalID].Remove(preferredDoctorsDict[hospitalChoice][lowestRankDoctorIDIndex]);

                                // Adds new match to hospitalResidentsMatchedDict 
                                if (hospitalResidentsMatchedDict.ContainsKey(currentHospital.HospitalID))
                                {
                                    hospitalResidentsMatchedDict[currentHospital.HospitalID].Add(doctor.DoctorID);
                                }
                                else hospitalResidentsMatchedDict.Add(currentHospital.HospitalID, new ArrayList() { doctor.DoctorID });

                                // Update's current Doctor's attributes with the new match 
                                doctor.isMatched = true;
                                doctor.HospitalMatched = currentHospital.HospitalID;

                                // Updates bumped Doctor's information so they are considered for a new match during the next run through
                                DoctorPreference bumpedDoctor = doctorPool.Find(x => x.DoctorID == preferredDoctorsDict[hospitalChoice][lowestRankDoctorIDIndex]);
                                bumpedDoctor.isMatched = false;
                                bumpedDoctor.HospitalMatched = null;

                                matchMade = true;
                                break;
                            }
                            
                        }
                        else matchMade = false;

                    }
                }
            }
            // Recurses through method to match any Doctors that got bumped and need to be considered for rematch
            if((matchMade == true) && doctorPool.Any(m => !(bool)m.isMatched)){
                MakeMatches(ref doctorPool, ref hospitalPool);
            }
            
            //SaveChanges
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
            runCount = 0;
            return hospitalResidentsMatchedDict;
        }
        
    }
}