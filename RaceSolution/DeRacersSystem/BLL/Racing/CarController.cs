using DeRacersSystem.DAL;
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
    public class CarController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<VINList> Get_VINList(int carclassid)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Cars
                              where x.CarClassID == carclassid
                              select new VINList
                              {
                                  CarID = x.CarID,
                                  SerialNumber = x.SerialNumber
                              };
                return results.ToList();
            }
        }
    }
}
