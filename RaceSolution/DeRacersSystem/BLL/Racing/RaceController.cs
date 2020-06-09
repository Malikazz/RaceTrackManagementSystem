using DeRacersSystem.DAL;
using DeRacersSystem.Data.RacingPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.BLL.Racing
{
    [DataObject]
    public class RaceController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ScheduleView> Get_ScheduleView(DateTime date)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Races
                              where DbFunctions.TruncateTime(x.RaceDate) == date.Date
                              orderby x.RaceDate
                              select new ScheduleView
                              {
                                  RaceID = x.RaceID,
                                  Time = x.RaceDate,
                                  Competition = x.Certification.Description + " - " + x.Comment,
                                  Run = x.Run,
                                  Drivers = x.NumberOfCars
                              };
                return results.ToList();
            }
        }
    }
}
