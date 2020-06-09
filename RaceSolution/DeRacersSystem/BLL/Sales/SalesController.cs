using DeRacersSystem.DAL;
using DeRacersSystem.Data.Entities;
using DeRacersSystem.Data.SalesPocos;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRacersSystem.BLL.Sales
{
  [DataObject]
  public class SalesController
  {
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<CategoryDDL> Get_CategoryDDL()
    {
      using (var context = new RaceContext())
      {
        return context.Categories.Select(x => new CategoryDDL { CategoryID = x.CategoryID, CategoryName = x.Description }).ToList();
      }
    }
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<ProductDDL> Get_ProductDDL(int categoryID)
    {
      using (var context = new RaceContext())
      {
        return context.Products.Where(x => x.CategoryID == categoryID)
          .Select(x => new ProductDDL
          {
            ProductID = x.ProductID,
            ProductName = x.ItemName,
            CategoryID = x.CategoryID
          }).ToList();
      }
    }
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public CartDisplay Get_CartDisplayItem(int productID, int categoryID)
    {
      using (var context = new RaceContext())
      {
        return context.Products.Where(x => x.ProductID == productID && x.CategoryID == categoryID)
          .Select(x => new CartDisplay
          {
            CategoryID = x.CategoryID,
            ProductID = x.ProductID,
            ProductName = x.ItemName,
            ProductPrice = (double)x.ItemPrice,
            ProductQuantity = 0,
          }).Single();
      }
    }
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<RefundItem> Get_RefundItemsByInvoiceNumber(int invoiceID)
    {
      using (var context = new RaceContext())
      {
        List<RefundItem> refundList = context.InvoiceDetails.Where(x => x.InvoiceID == invoiceID)
          .Select(x => new RefundItem
          {
            InvoiceID = x.InvoiceID,
            CategoryID = x.Product.CategoryID,
            ProductID = x.ProductID,
            ProductName = x.Product.ItemName,
            ProductPrice = (double)x.Product.ItemPrice,
            ProductQuantity = x.Quantity,
            Reason = "",
            RestockCharge = (double)x.Product.ReStockCharge
          }).ToList();
        return refundList;
      }
    }
    public int Do_CartTransaction(List<CartDisplay> cartItems)
    {
      if (cartItems.Count < 1)
      {
        throw new BusinessRuleException("Cart Error: ", new List<string> { "No Items in Cart" });

      }
      else
      {
        using (var context = new RaceContext())
        {
          double subtotal = 0;
          double total = 0;
          double tax = 0;
          foreach (CartDisplay item in cartItems)
          {
            CartDisplay tempItem = context.Products
              .Where(x => x.ProductID == item.ProductID && x.CategoryID == item.CategoryID)
              .Select(x => new CartDisplay { }).Single();
            if (tempItem == null)
            {
              throw new BusinessRuleException("Product not in database", new List<String> { item.ProductName });
            }
          }
          // update product quantities
          foreach (CartDisplay item in cartItems)
          {
            subtotal += item.ExtendedAmount;
            tax += subtotal * 0.05;
            total += subtotal + tax;
            Product temp = context.Products
              .Where(x => x.CategoryID == item.CategoryID && x.ProductID == item.ProductID)
              .Single();
            temp.QuantityOnHand -= item.ProductQuantity;
            context.Entry(temp).State = System.Data.Entity.EntityState.Modified;
          }
          // Create invoice
          Invoice invoice = new Invoice();
          invoice.EmployeeID = cartItems[0].EmployeeID;
          invoice.GST = (decimal)tax;
          invoice.InvoiceDate = DateTime.Now;
          invoice.SubTotal = (decimal)subtotal;
          invoice.Total = (decimal)total;

          invoice = context.Invoices.Add(invoice);

          // Create invoice details
          foreach (CartDisplay item in cartItems)
          {
            InvoiceDetail temp = new InvoiceDetail();
            temp.InvoiceID = invoice.InvoiceID;
            temp.ProductID = item.ProductID;
            temp.Quantity = item.ProductQuantity;
            temp.Price = (decimal)item.ProductPrice;
            context.InvoiceDetails.Add(temp);
          }
          int test = context.SaveChanges();
          return invoice.InvoiceID;

        }
      }
    }
    
    public int Do_RefundTransaction(List<RefundItem> list)
    {
      // Check refund for Oinvoice already there
      using (var context = new RaceContext())
      {
        int invoiceID = list[0].InvoiceID;
        if (context.StoreRefunds.Where(x => x.InvoiceID == invoiceID) != null)
        {
          var totals = CalculateTotals(list);
          // Invoice with negative values
          Invoice invoice = new Invoice();
          invoice.EmployeeID = list[0].EmployeeID;
          invoice.GST = (decimal)totals.Item2;
          invoice.SubTotal = (decimal)totals.Item1;
          invoice.Total = (decimal)totals.Item3;
          invoice.InvoiceDate = DateTime.Now;

          context.Invoices.Add(invoice);

          // Invoice details

          foreach(RefundItem item in list)
          {
            InvoiceDetail temp = new InvoiceDetail();
            StoreRefund retemp = new StoreRefund();
            if (item.ItemToBeRefunded)
            {
              temp.InvoiceID = invoice.InvoiceID;              
              temp.ProductID = item.ProductID;
              temp.Quantity = item.ProductQuantity;
              temp.Price = (decimal)item.ProductPrice;

              retemp.InvoiceID = invoice.InvoiceID;
              retemp.ProductID = item.ProductID;
              retemp.OriginalInvoiceID = item.InvoiceID;
              retemp.Reason = item.Reason;
              context.StoreRefunds.Add(retemp);

              Product prodTemp = context.Products.Where(x => x.ProductID == item.ProductID && x.CategoryID == item.CategoryID).Single();
              prodTemp.QuantityOnHand += item.ProductQuantity;
              context.Entry(prodTemp).State = System.Data.Entity.EntityState.Modified;
            }
          }
          context.SaveChanges();
          return invoice.InvoiceID;
        }
        else
        {
          throw new BusinessRuleException("Refund Error", new List<string> { "Refund already exsists" });
        }
      }






    }
    private Tuple<double, double, double> CalculateTotals(List<RefundItem> list)
    {
      double subtotal = 0;
      double total = 0;
      double tax = 0;
      double restockCharge = 0;
      foreach (RefundItem item in list)
      {
        if (item.ItemToBeRefunded)
        {
          if (item.HasRestockCharge)
          {
            subtotal += item.ExtenedAmount;
            tax += subtotal * 0.05;
            subtotal -= restockCharge;
            total += tax + subtotal;
          }
          else
          {
            subtotal += item.ExtenedAmount;
            tax += subtotal * 0.05;
            total += tax + subtotal;
          }
        }
      }
      subtotal *= -1;
      tax *= -1;
      total *= -1;
      return new Tuple<double, double, double>(subtotal, tax, total);
    }
  }

}
