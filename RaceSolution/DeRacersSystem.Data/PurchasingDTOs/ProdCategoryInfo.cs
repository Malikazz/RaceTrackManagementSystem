using DeRacersSystem.Data.PurchasingPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.PurchasingDTOs
{
    public class ProdCategoryInfo
    {
        public string Category { get; set; }
        public IEnumerable<InventoryInfo> ProductList { get; set; }
    }
}
