using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.PurchasingPOCOs
{
    public class InventoryInfo
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Reorder { get; set; }
        public int InStock { get; set; }
        public int OnOrder { get; set; }
        public string UnitType { get; set; }
        public int UnitSize { get; set; }
    }
}
