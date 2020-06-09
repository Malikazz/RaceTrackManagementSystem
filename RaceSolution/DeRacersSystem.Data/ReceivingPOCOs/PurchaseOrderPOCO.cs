using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.ReceivingPOCOs
{
    public class PurchaseOrderPOCO
    {
        public int OrderID { get; set; }
        public string Item { get; set; }
        public int QuantityOrdered { get; set; }
        public string OrderedUnits { get; set; }
        public int QuantityOutstanding { get; set; }
        public int ReceivedUnitSize { get; set; }
        //public string RejectedUnitsReason { get; set; }
        //public int SalvagedItems { get; set; }
    }
}
