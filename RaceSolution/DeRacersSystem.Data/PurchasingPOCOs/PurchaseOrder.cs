using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.PurchasingPOCOs
{
    public class PurchaseOrder
    {
        public int ProductID { get; set; }
        public string Product { get; set; }
        public int OrderQty { get; set; }
        public int UnitSize { get; set; }
        public string UnitType { get; set; }
        public decimal UnitCost { get; set; }
        public string Warning { get; set; }
        public decimal PerItemCost { get; set; }
        public decimal ExtendedCost { get; set; }
    }
}
