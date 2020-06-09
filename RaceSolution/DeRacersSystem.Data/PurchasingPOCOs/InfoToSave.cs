using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.PurchasingPOCOs
{
    public class InfoToSave
    {
        public int EmployeeID { get; set; }
        public int VendorID { get; set; }
        public string Comments { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }

    }
}
