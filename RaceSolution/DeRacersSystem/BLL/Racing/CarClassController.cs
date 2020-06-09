using DeRacersSystem.DAL;
using DeRacersSystem.Data.Entities;
using DeRacersSystem.Data.RacingPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.BLL.Racing
{
    [DataObject]
    public class CarClassController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CarClassList> Get_CarClassList(int raceid)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.CarClasses
                              where x.CertificationLevel == (from y in context.Races
                                                             where y.RaceID == raceid
                                                             select y.CertificationLevel).FirstOrDefault()
                              select new CarClassList
                              {
                                  CarClassID = x.CarClassID,
                                  CarClassName = x.CarClassName
                              };
                return results.ToList();
            }
        }
    }
}
