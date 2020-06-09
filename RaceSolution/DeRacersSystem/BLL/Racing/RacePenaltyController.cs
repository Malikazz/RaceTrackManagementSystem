using DeRacersSystem.DAL;
using DeRacersSystem.Data.RacingPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.BLL.Racing
{
    [DataObject]
    public class RacePenaltyController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<PenaltyList> Get_PenaltyList()
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.RacePenalties
                              select new PenaltyList
                              {
                                  PenaltyID = x.PenaltyID,
                                  Description = x.Description
                              };
                return results.ToList();
            }
        }
    }
}
