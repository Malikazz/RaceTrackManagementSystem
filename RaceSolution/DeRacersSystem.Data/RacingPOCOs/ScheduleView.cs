using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.RacingPOCOs
{
    public class ScheduleView
    {
        public int RaceID { get; set; }
        public DateTime Time { get; set; }
        public string Competition { get; set; }
        public string Run { get; set; }
        public int Drivers {get; set;}
    }
}
