using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.Data.SalesPocos
{
  public class RefundItem
  {
    public int EmployeeID { get { return 20; } }
    public int ProductID { get; set; }
    public int CategoryID { get; set; }
    public int InvoiceID { get; set; }
    public int RefundID { get; set; }
    public string Reason { get; set; }
    public string ProductName { get; set; }
    public int ProductQuantity { get; set; }
    public double ProductPrice { get; set; }
    public double ExtenedAmount { get { return ProductQuantity * ProductPrice; } }
    public double RestockCharge { get; set; }
    public bool HasRestockCharge
    {
      get
      {
        if(RestockCharge != 0.0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }
    public bool ItemToBeRefunded {
      get
      {
        if (string.IsNullOrEmpty(Reason))
        {
          return false;
        }
        else
        {
          return true;
        }
      }
    }

  }
}
