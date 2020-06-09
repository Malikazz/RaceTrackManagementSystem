using DeRacersSystem.DAL;
using DeRacersSystem.Data.CommonPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.BLL.Common
{
    [DataObject]
    public class EmployeeController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<EmployeeList> Get_Employees()
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Employees
                              select new EmployeeList
                              {
                                  ID = x.EmployeeID,
                                  Name = x.FirstName + " " + x.LastName
                              };
                return results.ToList();
            }
        }
    }
}
