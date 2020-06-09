using DeRacersSystem.DAL;
using DeRacersSystem.Data.PurchasingPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DMIT2018Common.UserControls;
using DeRacersSystem.Data.Entities;
using DeRacersSystem.Data.ReceivingPOCOs;

namespace DeRacersSystem.BLL.Receiving
{
    [DataObject]
    public class ReceivingController
    {
        private List<string> reasons = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<VendorOrder> GetVendors()
        {
            using (var context = new RaceContext())
            {
                var result = from x in context.Vendors
                             from y in x.Orders
                             where y.Closed == false && y.OrderNumber != null && y.OrderDate != null
                             select new VendorOrder
                             {
                                 VendorID = x.VendorID,
                                 VendorOrderNumber = y.OrderNumber + " - " + x.Name
                             };
                return result.ToList();
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<VendorContact> GetVendorContacts(int vendorid)
        {
            using (var context = new RaceContext())
            {
                var result = from x in context.Vendors
                             where x.VendorID == vendorid
                             select new VendorContact
                             {
                                 VendorName = x.Name,
                                 Contact = x.Contact,
                                 Address = x.Address +" "+x.City,
                                 Phone = x.Phone
                             };
                return result.ToList();
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<PurchaseOrderPOCO> GetPurchaseOrder(int vendorid)
        {
            using (var context = new RaceContext())
            {
                var result = (from x in context.Orders
                              from y in x.OrderDetails
                              join prod in context.Products on y.ProductID equals prod.ProductID
                              join order in context.ReceiveOrders on x.OrderID equals order.OrderID
                              join item in context.ReceiveOrderItems on order.ReceiveOrderID equals item.ReceiveOrderID
                              where x.VendorID == vendorid
                              select new PurchaseOrderPOCO
                              {
                                  OrderID = x.OrderID,
                                  Item = prod.ItemName,
                                  QuantityOrdered = y.Quantity * y.OrderUnitSize,
                                  OrderedUnits = y.Quantity + " x case of " + y.OrderUnitSize,
                                  QuantityOutstanding = y.ReceiveOrderItems.Count() == 0 ? y.Quantity * y.OrderUnitSize : (y.Quantity * y.OrderUnitSize) - (from sum in y.ReceiveOrderItems where sum.OrderDetailID == y.OrderDetailID select (sum.ItemQuantity)).Sum(),
                                  ReceivedUnitSize = //((from roi in y.ReceiveOrderItems where roi.OrderDetailID == y.OrderDetailID select (roi.ItemQuantity)).FirstOrDefault() / y.OrderUnitSize) + 
                                  y.OrderUnitSize,
                                  //RejectedUnitsReason = (from ret in y.ReturnOrderItems where ret.OrderDetailID == y.OrderDetailID select (ret.ItemQuantity + " " + ret.Comment)).FirstOrDefault(),
                                  //SalvagedItems = 0
                              }).Distinct().OrderBy(x => x.QuantityOrdered);

                return result.ToList();
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnOrderedItem> GetUnOrderedItems()
        {
            using (var context = new RaceContext())
            {
                return context.UnOrderedItems.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int UnOrderedItem_Add(UnOrderedItem item)
        {
            using (var context = new RaceContext())
            {
                if (string.IsNullOrEmpty(item.ItemName))
                {
                    throw new BusinessRuleException("Please enter an unordered item.", reasons);
                }
                else if(item.Quantity <= 0)
                {
                    throw new BusinessRuleException("The Unordered item quantity must be greater than 0", reasons);
                }
                else
                {
                    var exists = (from x in context.Products
                              where x.ItemName == item.ItemName
                              select x.ProductID).FirstOrDefault().ToString();


                    item.VendorProductID = exists;

                     item.OrderID = 999;

                    if (item.VendorProductID == null || int.Parse(item.VendorProductID) ==  0)
                    {
                        throw new BusinessRuleException("Item does not exist in the database.", reasons);
                    }
                    else
                    {
                        context.UnOrderedItems.Add(item);   //staging
                        context.SaveChanges();      //actual commit to the database
                        return item.ItemID;
                    }
                }

                
            }
        }

        

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Item_Delete(UnOrderedItem item)
        {
            return Item_Delete(item.ItemID);
        }

        public int Item_Delete(int itemid)
        {
            using (var context = new RaceContext())
            {
                var existing = context.UnOrderedItems.Find(itemid);

                context.UnOrderedItems.Remove(existing);
                return context.SaveChanges();
            }
        }

      public void ForceCloseOrder(int orderid, string reason, List<ForceClearPOCO> clearitem, int vendorid)
        {
            List<string> errors = new List<string>();

            using (var context = new RaceContext())
            {
                var closed = (from x in context.Orders
                              where x.OrderID == orderid
                              select x.Closed).FirstOrDefault();
                var closeorderlist = new List<ForceClearPOCO>();

                if (closed)
                {
                    errors.Add("You cannot close an already closed order");
                }
                else
                {
                    
                    foreach (var item in clearitem)
                    {
                        var closesearch = (from x in context.Products
                                           where x.ItemName == item.ItemName
                                           select x).FirstOrDefault();

                        closesearch.QuantityOnOrder = closesearch.QuantityOnOrder - item.Remaining;
                    }
                    var query = (from x in context.Orders
                                 where x.OrderID == orderid && x.VendorID == vendorid
                                 select x).FirstOrDefault();
                    query.Comment = reason;
                    CloseOrder(orderid);
                    context.SaveChanges();
                    
                }

            }
        }

        public void ReceiveShipment(int orderid, List<TransactionItemPOCO> received, List<RejectedItemPOCO> rejected)
        {
            List<string> errors = new List<string>();

            using (var context = new RaceContext())
            {
                var closed = (from x in context.Orders
                             where x.OrderID == orderid
                             select x.Closed).FirstOrDefault();

                
                if(closed)
                {
                    errors.Add("You cannot receive a closed order");
                }
                else if (received.Any( x => x.ItemQuantity < 0))
                {
                    errors.Add("You cannot have a negative received amount");
                }
                else
                {
                    int newrecieveorderid = context.ReceiveOrders.Select(x => x.ReceiveOrderID).Max();
                    newrecieveorderid++;

                  
                    var recieveorderlist = new List<ReceiveOrderItem>();
                    var returnorderlist = new List<ReturnOrderItem>();

                    foreach (var item in received)
                    {
                        int newrecieveorderitemid = context.ReceiveOrderItems.Select(x => x.ReceiveOrderItemID).Max();
                        newrecieveorderid++;

                        int orderDetailID = (from x in context.Products
                                             from y in x.OrderDetails
                                             where x.ItemName == item.ItemName && y.OrderID == orderid
                                             select y.OrderDetailID).FirstOrDefault();


                        recieveorderlist.Add(new ReceiveOrderItem()
                        {
                            ReceiveOrderItemID = newrecieveorderitemid,
                            ReceiveOrderID = newrecieveorderid,
                            OrderDetailID = orderDetailID,
                            ItemQuantity = item.ItemQuantity,
                            OrderDetail = (from x in context.OrderDetails
                                           where x.OrderDetailID == orderDetailID
                                           select x).FirstOrDefault(),
                            ReceiveOrder = (from x in context.ReceiveOrders
                                            where x.ReceiveOrderID == newrecieveorderid
                                            select x).FirstOrDefault()
                        });
                    }

                    if(rejected.Count() > 0)
                    {
                        foreach (var item in rejected)
                        {
                            int newreturnorderitemid = context.ReturnOrderItems.Select(x => x.ReturnOrderItemID).Max();
                            newreturnorderitemid++;

                            int orderDetailID = (from x in context.Products
                                                 from y in x.OrderDetails
                                                 where x.ItemName == item.ItemName && y.OrderID == orderid
                                                 select y.OrderDetailID).FirstOrDefault();


                            returnorderlist.Add(new ReturnOrderItem()
                            {
                                ReturnOrderItemID = newreturnorderitemid,
                                ReceiveOrderID = newrecieveorderid,
                                OrderDetailID = orderDetailID,
                                UnOrderedItem = null,
                                ItemQuantity = item.ItemQuantity,
                                Comment = item.Reason,
                                VendorProductID = null,
                                OrderDetail = (from x in context.OrderDetails
                                               where x.OrderDetailID == orderDetailID
                                               select x).FirstOrDefault(),
                                ReceiveOrder = (from x in context.ReceiveOrders
                                                where x.ReceiveOrderID == newrecieveorderid
                                                select x).FirstOrDefault()
                            });
                        }
                    }
                  

                    ReceiveOrder receiveOrder = new ReceiveOrder
                    {
                        ReceiveOrderID = newrecieveorderid,
                        OrderID = orderid,
                        ReceiveDate = DateTime.Now,
                        EmployeeID = 56,
                        Employee = (from x in context.Employees
                                    where x.EmployeeID == 56
                                    select x).FirstOrDefault(),
                        Order = (from x in context.Orders
                                 where x.OrderID == orderid
                                 select x).FirstOrDefault(),
                        ReceiveOrderItems = recieveorderlist,
                        ReturnOrderItems = returnorderlist
                    };
                    context.ReceiveOrders.Add(receiveOrder);


                    foreach(var item in recieveorderlist.ToList())
                    {
                        ReceiveOrderItem receiveOrderItem = new ReceiveOrderItem
                        {
                            ReceiveOrderItemID = item.ReceiveOrderItemID,
                            ReceiveOrderID = item.ReceiveOrderID,
                            OrderDetailID = item.OrderDetailID,
                            ItemQuantity = item.ItemQuantity,
                            OrderDetail = item.OrderDetail,
                            ReceiveOrder = item.ReceiveOrder
                        };

                        var query = (from x in context.OrderDetails
                                     join prod in context.Products on x.ProductID equals prod.ProductID
                                     where x.OrderDetailID == receiveOrderItem.OrderDetailID
                                     select prod).FirstOrDefault();

                        query.QuantityOnHand = query.QuantityOnHand + receiveOrderItem.ItemQuantity;
                        query.QuantityOnOrder = query.QuantityOnOrder - receiveOrderItem.ItemQuantity;

                    }

                    if(returnorderlist.Count > 0)
                    {
                        foreach(var item in returnorderlist.ToList())
                        {
                            ReturnOrderItem returnOrderItem = new ReturnOrderItem()
                            {
                                ReturnOrderItemID = item.ReturnOrderItemID,
                                ReceiveOrderID = item.ReceiveOrderID,
                                OrderDetailID = item.OrderDetailID,
                                UnOrderedItem = item.UnOrderedItem,
                                ItemQuantity = item.ItemQuantity,
                                Comment = item.Comment,
                                VendorProductID = item.VendorProductID,
                                OrderDetail = item.OrderDetail,
                                ReceiveOrder = item.ReceiveOrder
                            };

                            var query = (from x in context.OrderDetails
                                         join prod in context.Products on x.ProductID equals prod.ProductID
                                         where x.OrderDetailID == returnOrderItem.OrderDetailID
                                         select prod).FirstOrDefault();

                        }
                    }


                    context.SaveChanges();
                }                
                if(errors.Count > 0)
                {
                    throw new BusinessRuleException("Receive Shipment", errors);
                }

            }
        }

        public void CloseOrder(int orderid)
        {
            using (var context = new RaceContext())
            {
                var query = (from q in context.Orders
                             where q.OrderID == orderid
                             select q).First();
                query.Closed = true;
                context.SaveChanges();
            }
        }
        

    }
}
