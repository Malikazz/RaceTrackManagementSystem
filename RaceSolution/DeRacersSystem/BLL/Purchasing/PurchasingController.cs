using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeRacersSystem.DAL;
using DeRacersSystem.Data.Entities;
using DeRacersSystem.Data.PurchasingDTOs;
using DeRacersSystem.Data.PurchasingPOCOs;
using DMIT2018Common.UserControls;

namespace DeRacersSystem.BLL.Purchasing
{
    [DataObject]
    public class PurchasingController
    {
        private List<string> errors = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> User_List()
        {
            using (var context =new RaceContext())
            {
                var results = from x in context.Employees
                              where x.PositionID == 1 || x.PositionID == 10
                              orderby x.LastName, x.FirstName
                              select new SelectionList
                              {
                                  ValueField = x.EmployeeID,
                                  TextField = x.FirstName + " " + x.LastName
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> Vendor_List()
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Vendors
                              orderby x.Name
                              select new SelectionList
                              {
                                  ValueField = x.VendorID,
                                  TextField = x.Name
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VendorInfo Vendor_Info(int id)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.Vendors
                              where x.VendorID == id
                              select new VendorInfo
                              {
                                  VendorName = x.Name,
                                  Contact = x.Contact,
                                  Phone = x.Phone,
                                  Comments = x.Orders.Where(y => y.VendorID == id).Select(y => y.Comment).FirstOrDefault()
                              };
                return results.FirstOrDefault();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<PurchaseOrder> Purchase_Order(int id)
        {
            using (var context = new RaceContext())
            {
                List<PurchaseOrder> results = null;
                results = (from x in context.OrderDetails
                              where x.Order.VendorID == id && x.Order.OrderNumber == null && x.Order.OrderDate == null
                              select new PurchaseOrder
                              {
                                  ProductID = x.ProductID,
                                  Product = x.Product.ItemName,
                                  OrderQty = x.Quantity,
                                  UnitSize = x.OrderUnitSize,
                                  UnitType = x.Product.VendorCatalogs.Where(y => y.ProductID == x.ProductID)
                                            .Select(y => y.OrderUnitType).FirstOrDefault(),
                                  UnitCost = x.Cost,
                                  Warning = " ",
                                  PerItemCost = x.Cost/x.OrderUnitSize,
                                  ExtendedCost = x.Cost * x.Quantity
                              }).ToList();

                if (results.Count() == 0)
                {
                    results = (from x in context.VendorCatalogs
                               where x.VendorID == id &&
                               x.Product.ReOrderLevel >= (x.Product.QuantityOnHand + x.Product.QuantityOnOrder)
                               select new PurchaseOrder
                               {
                                   ProductID = x.ProductID,
                                   Product = x.Product.ItemName,
                                   OrderQty = 1,
                                   UnitSize = x.OrderUnitSize,
                                   UnitType = x.OrderUnitType,
                                   UnitCost = x.OrderUnitCost,
                                   Warning = " ",
                                   PerItemCost = x.OrderUnitCost / x.OrderUnitSize,
                                   ExtendedCost = x.OrderUnitCost
                               }).ToList();

                }
                return results;
            }
        }

        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ProdCategoryInfo> ProdCategory_List (int id)
        {
            using (var context = new RaceContext())
            {
                var results = from x in context.VendorCatalogs
                              where x.VendorID == id
                              group x by x.Product.Category into gTemp
                              orderby gTemp.Key.Description
                              select new ProdCategoryInfo
                              {
                                  Category = gTemp.Key.Description,
                                  ProductList = from y in gTemp
                                                select new InventoryInfo
                                                {
                                                    ProductID = y.ProductID,
                                                    ProductName = y.Product.ItemName,
                                                    Reorder = y.Product.ReOrderLevel,
                                                    InStock = y.Product.QuantityOnHand,
                                                    OnOrder = y.Product.QuantityOnOrder,
                                                    UnitType = y.OrderUnitType,
                                                    UnitSize = y.OrderUnitSize
                                                }
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public string Warning (int prodid, List<PurchaseOrder> po)
        {
            using (var context = new RaceContext())
            {
                string message = "";
                decimal price = (from x in context.Products
                            where x.ProductID == prodid
                            select x.ItemPrice).FirstOrDefault();
                foreach (PurchaseOrder row in po)
                {
                    if (row.ProductID == prodid && row.PerItemCost > price)
                        message = "!!";
                }

                return message;

            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PurchaseOrder InventoryToPO(int vendorid, int productid, List<PurchaseOrder> list)
        {
            using (var context = new RaceContext())
            {
                int items = list.Where(x => x.ProductID == productid).Select(x => x).Count();
                PurchaseOrder results = null;
                if (items == 0)
                {
                    results = (from x in context.VendorCatalogs
                              where x.VendorID == vendorid && x.ProductID == productid
                              select new PurchaseOrder
                              {
                                  ProductID = x.ProductID,
                                  Product = x.Product.ItemName,
                                  OrderQty = 1,
                                  UnitSize = x.OrderUnitSize,
                                  UnitType = x.OrderUnitType,
                                  UnitCost = x.OrderUnitCost,
                                  Warning = " ",
                                  PerItemCost = x.OrderUnitCost / x.OrderUnitSize,
                                  ExtendedCost = x.OrderUnitCost * x.OrderUnitSize
                              }).FirstOrDefault();
                }
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]

        public void DeletePO(int vendorid)
        {
            using (var context = new RaceContext())
            {
                Order oldorder = null;
                oldorder = context.Orders.Where(x => x.VendorID == vendorid
                                            && x.OrderNumber == null && x.OrderDate == null)
                                    .Select(x => x).FirstOrDefault();

                if (oldorder != null)
                {
                    foreach (var row in context.OrderDetails)
                    {
                        if (row.OrderID == oldorder.OrderID)
                            context.OrderDetails.Remove(row);
                    }
                    context.Orders.Remove(oldorder);
                }

                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]

        public void SavePO (List<PurchaseOrder> po, InfoToSave infolist)
        {
            using (var context = new RaceContext())
            {
                var results = po.Select(x => x);
                if (results.Count() == 0)
                    errors.Add("The purchase order is empty. There is nothing to save.");
                if (infolist.EmployeeID == 0)
                    errors.Add("Select an employee.");
                if (infolist.VendorID == 0)
                    errors.Add("Select a vendor.");
                if (errors.Count > 0)
                {
                    throw new BusinessRuleException("Saving Purchase Order", errors);
                }

                Order oldorder = null;
                oldorder = context.Orders.Where(x => x.VendorID == infolist.VendorID 
                                            && x.OrderNumber == null && x.OrderDate == null)
                                    .Select(x => x).FirstOrDefault();
                
                if (oldorder != null)
                { 
                    foreach (var row in context.OrderDetails)
                    {
                        if (row.OrderID == oldorder.OrderID)
                            context.OrderDetails.Remove(row);
                    }
                    context.Orders.Remove(oldorder);
                }

                Order neworder = new Order
                {
                    EmployeeID = infolist.EmployeeID,
                    VendorID = infolist.VendorID,
                    Comment = infolist.Comments,
                    SubTotal = infolist.Subtotal,
                    TaxGST = infolist.Tax
                };
                context.Orders.Add(neworder);

                foreach (var row in po)
                {
                    context.OrderDetails.Add(new OrderDetail
                    {
                        ProductID = row.ProductID,
                        Quantity = row.OrderQty,
                        OrderUnitSize = row.UnitSize,
                        Cost = row.UnitCost
                    });
                }

                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]

        public void PlacePO(List<PurchaseOrder> po, InfoToSave infolist)
        {
            using (var context = new RaceContext())
            {
                var results = po.Select(x => x);
                if (results.Count() == 0)
                    errors.Add("The purchase order is empty. There is no purchase order to place.");
                if (infolist.EmployeeID == 0)
                    errors.Add("Select an employee.");
                if (infolist.VendorID == 0)
                    errors.Add("Select a vendor.");
                if (errors.Count > 0)
                {
                    throw new BusinessRuleException("Placing Purchase Order", errors);
                }

                Order oldorder = null;
                oldorder = context.Orders.Where(x => x.VendorID == infolist.VendorID
                                                && x.OrderNumber == null && x.OrderDate == null)
                                    .Select(x => x).FirstOrDefault();

                if (oldorder != null)
                {
                    foreach (var row in context.OrderDetails)
                        {
                            if (row.OrderID == oldorder.OrderID)
                                context.OrderDetails.Remove(row);
                        }
                    context.Orders.Remove(oldorder);
                }

                int? neworderno = context.Orders.Where(x => x.OrderNumber != null).Select(x => x.OrderNumber).Max();
                neworderno++;

                Order neworder = new Order
                {
                    OrderNumber = neworderno,
                    OrderDate = DateTime.Now,
                    EmployeeID = infolist.EmployeeID,
                    VendorID = infolist.VendorID,
                    Comment = infolist.Comments,
                    SubTotal = infolist.Subtotal,
                    TaxGST = infolist.Tax
                };
                context.Orders.Add(neworder);

                foreach (var row in po)
                {
                    context.OrderDetails.Add(new OrderDetail
                    {
                        ProductID = row.ProductID,
                        Quantity = row.OrderQty,
                        OrderUnitSize = row.UnitSize,
                        Cost = row.UnitCost
                    });
                }

                Product item = null;
                foreach (var row in po)
                {
                    item = context.Products
                            .Where(p => p.ProductID == row.ProductID)
                            .Select(od => od).FirstOrDefault();
                    if (item != null)
                    {
                        item.QuantityOnOrder += row.OrderQty * row.UnitSize;
                        context.Entry(item).Property(x => x.QuantityOnOrder).IsModified = true;
                    }
                }

                context.SaveChanges();
            }
        }

    }
}
