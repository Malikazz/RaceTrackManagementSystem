using DeRacersSystem.BLL.Receiving;
using DeRacersSystem.Data.ReceivingPOCOs;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApp.Receiving
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void PurchaseOrderFetch_Click(object sender, EventArgs e)
        {
            if(int.Parse(VendorDDL.SelectedValue) == 0)
            {
                ReceiveOrder.Visible = false;
                CommentBox.Visible = false;
                Close.Visible = false;
                UnOrderedItems.Visible = false;
            }
            else
            {
                UnOrderedItems.Visible = true;
                ReceiveOrder.Visible = true;
                CommentBox.Visible = true;
                Close.Visible = true;
                
            }

                               
        }

        protected void ForceClose_Click(object sender, EventArgs e)
        {
            int vendorid = 0;
            int orderid = 0;

            vendorid = int.Parse(VendorDDL.SelectedValue);

            GridViewRow orderrow = PurchaseOrderGridView.Rows[1];
            orderid = int.Parse((orderrow.FindControl("OrderID") as Label).Text);
            var closingitems = new List<ForceClearPOCO>();

            MessageUserControl.TryRun(() => 
            {
                foreach (GridViewRow row in PurchaseOrderGridView.Rows)
                {
                    if (int.Parse(((row.FindControl("QuantityOutstanding") as Label).Text).ToString()) > 0)
                    {
                        int remaining = int.Parse(((row.FindControl("QuantityOutstanding") as Label).Text).ToString());
                        string itemname = row.Cells[1].Text;

                        closingitems.Add(new ForceClearPOCO()
                        {
                            ItemName = itemname,
                            Remaining = remaining
                        });
                    }
                }

                string reason = (CommentBox.Text).ToString();

                if(string.IsNullOrEmpty(reason))
                {
                    throw new BusinessRuleException("Input Error", new List<String> { "There must be a reason when force closing." });
                }
                else
                {
                    ReceivingController sysmgr = new ReceivingController();
                    sysmgr.ForceCloseOrder(orderid, reason, closingitems, vendorid);
                }
            }, "Force Close", "Purchase Order has been closed." );
        }
         
        protected void ReceiveShipment_Click(object sender, EventArgs e)
        {
            var receiveditems = new List<TransactionItemPOCO>();
            var rejecteditems = new List<RejectedItemPOCO>();

            int orderid = 0;

            GridViewRow orderrow = PurchaseOrderGridView.Rows[1];
            orderid = int.Parse((orderrow.FindControl("OrderID") as Label).Text);      
                MessageUserControl.TryRun(() =>
                {
                    foreach (GridViewRow row in PurchaseOrderGridView.Rows)
                    {
                        if (int.Parse(((row.FindControl("QuantityOutstanding") as Label).Text).ToString()) > 0)
                        {
                            if (!int.TryParse(((row.FindControl("ReceivedUnitsBox") as TextBox).Text), out int receivecheck) || receivecheck <= 0)
                            {
                                throw new BusinessRuleException("Input Error", new List<String> { "Received items must be an integer number greater than zero." });
                            }
                            int salvageditems = 0;

                            int itemunitsize = int.Parse(((row.FindControl("ReceivedUnitsBox") as TextBox).Text));
                            int maxorder = int.Parse(row.Cells[2].Text);
                            string itemname = row.Cells[1].Text;
                            int itemquantity = int.Parse(((row.FindControl("UnitLabel") as Label).Text).ToString()) * itemunitsize;

                            if (string.IsNullOrEmpty(((row.FindControl("SalvagedItemsBox") as TextBox).Text)) == false)
                            {
                                if (!int.TryParse(((row.FindControl("SalvagedItemsBox") as TextBox).Text), out int salvagedcheck) || salvagedcheck <= 0)
                                {
                                    throw new BusinessRuleException("Input Error", new List<String> { "Salvaged items must be an integer number greater than zero." });
                                }
                                else if (itemquantity + int.Parse(((row.FindControl("SalvagedItemsBox") as TextBox).Text)) > maxorder)
                                {
                                    throw new BusinessRuleException("Input Error", new List<String> { "Total Item quantity cannot be greater than the initial item order." });
                                }
                                else
                                {
                                    salvageditems = int.Parse(((row.FindControl("SalvagedItemsBox") as TextBox).Text));
                                    itemquantity = itemquantity + salvageditems;
                                }

                            }

                            itemquantity = itemquantity + salvageditems;

                            if (!string.IsNullOrEmpty((row.FindControl("RejectedUnitsBox") as TextBox).Text) && !string.IsNullOrEmpty((row.FindControl("RejectedReason") as TextBox).Text))
                            {
                                if (!int.TryParse(((row.FindControl("RejectedUnitsBox") as TextBox).Text), out int rejectedcheck) || rejectedcheck <= 0)
                                {
                                    throw new BusinessRuleException("Input Error", new List<String> { "Rejected items must be an integer number greater than zero." });
                                }
                                else if (int.TryParse(((row.FindControl("RejectedReasons") as TextBox).Text), out int rejected))
                                {
                                    throw new BusinessRuleException("Input Error", new List<String> { "Reason for rejection cannot be a number." });
                                }
                                else
                                {
                                    int rejectitems = int.Parse((row.FindControl("RejectedUnitsBox") as TextBox).Text);
                                    string reasons = (row.FindControl("RejectedReason") as TextBox).ToString();

                                    rejecteditems.Add(new RejectedItemPOCO()
                                    {
                                        ItemName = itemname,
                                        ItemQuantity = rejectitems,
                                        Reason = reasons
                                    });
                                }
                            }

                            receiveditems.Add(new TransactionItemPOCO()
                            {
                                ItemName = itemname,
                                ItemQuantity = itemquantity
                            });


                        }

                    }
                });

             MessageUserControl.TryRun(() =>
             {
              ReceivingController sysmgr = new ReceivingController();

              sysmgr.ReceiveShipment(orderid, receiveditems, rejecteditems);
             }, "Transaction Processed", "Order Received Successfully"
             );
           
            
            PurchaseOrderGridView.DataBind();
            int remainingitems = 0;
            foreach (GridViewRow row in PurchaseOrderGridView.Rows)
            {
                remainingitems = remainingitems + int.Parse(((row.FindControl("QuantityOutstanding") as Label).Text).ToString());
            }

            if (remainingitems == 0)
            {
                MessageUserControl.TryRun(() => {
                    ReceivingController sysmgr = new ReceivingController();
                    sysmgr.CloseOrder(orderid);
                },"Order Complete", "This Order will now be closed");

                Response.Redirect(Request.RawUrl);
                MessageUserControl.TryRun(() => { }, "Order Complete", "The Order was closed. Please select a new one.");
            }

        }

       

        protected void InsertCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Item Added");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }

        protected void DeleteCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Item Removed");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
    }
}