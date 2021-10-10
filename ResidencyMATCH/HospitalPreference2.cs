using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidencyMATCH
{
    public partial class HospitalPreference
    {
        //public List<int> PreferredDoctors { get; set; }
        //public List<int> ResidentsMatched { get; set; }

        public HospitalPreference()
        {
            //PreferredDoctors.Add(ChoiceDoctor1);
            //if (ChoiceDoctor2 != null) PreferredDoctors.Add((int)ChoiceDoctor2);
            //if (ChoiceDoctor3 != null) PreferredDoctors.Add((int)ChoiceDoctor3);
            //if (ChoiceDoctor4 != null) PreferredDoctors.Add((int)ChoiceDoctor4);
            //if (ChoiceDoctor5 != null) PreferredDoctors.Add((int)ChoiceDoctor5);

            Openings = ResidentCapacity;
        }
    }
}
