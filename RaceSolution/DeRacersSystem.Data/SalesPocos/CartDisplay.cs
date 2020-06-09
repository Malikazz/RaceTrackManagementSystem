using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.SalesPocos
{
  public class CartDisplay
  {
    // emp id that will work 20, 37, 49, 56, 57
    public int EmployeeID { get { return 20; } }
    public int ProductID { get; set; }
    public int CategoryID { get; set; }
    public string ProductName { get; set; }
    public int ProductQuantity { get; set; }
    public double ProductPrice { get; set; }
    public double ExtendedAmount { get { return ProductPrice * ProductQuantity; } }
  }
}
