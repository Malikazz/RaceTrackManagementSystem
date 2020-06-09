using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.ReceivingPOCOs
{
    public class RejectedItemPOCO
    {
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public string Reason { get; set; }
    }
}
