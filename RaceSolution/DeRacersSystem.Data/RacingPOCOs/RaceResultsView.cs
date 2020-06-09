using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.RacingPOCOs
{
    public class RaceResultsView
    {
        public int RaceDetailID { get; set; }
        public string Name { get; set; }
        public TimeSpan? Time { get; set; }
        public int Penalties { get; set; }
    }
}
