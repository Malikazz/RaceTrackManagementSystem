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
    public class MemberController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DriverList> Get_DriverList()
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Members
                              select new DriverList
                              {
                                  MemberID = x.MemberID,
                                  Name = x.FirstName + " " + x.LastName
                              };
                return results.ToList();
            }
        }
    }
}
