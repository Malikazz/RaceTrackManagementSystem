using DeRacersSystem.BLL.Purchasing;
using DeRacersSystem.Data.PurchasingDTOs;
using DeRacersSystem.Data.PurchasingPOCOs;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.Purchasing
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Subtotal.Text = "0.00";
            Tax.Text = "0.00";
            Total.Text = "0.00";
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void SelectVendor_Click(object sender, EventArgs e)
        {
            if (VendorList.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Select Error", "You must select a vendor.");
                Subtotal.Text = "0.00";
                Tax.Text = "0.00";
                Total.Text = "0.00";
                Cancel_Click(sender, e);
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    int vendorid = int.Parse(VendorList.SelectedValue);
                    PurchasingController sysmgr = new PurchasingController();
                    VendorInfo newinfo = sysmgr.Vendor_Info(vendorid);
                    VName.Text = newinfo.VendorName;
                    Contact.Text = newinfo.Contact;
                    Phone.Text = newinfo.Phone;
                    Comments.Text = newinfo.Comments;
                    List<PurchaseOrder> order = sysmgr.Purchase_Order(vendorid);
                    PurchaseOrderGV.DataSource = order;
                    PurchaseOrderGV.DataBind();
                    calculatePOValue();
                    List<ProdCategoryInfo> inventory = sysmgr.ProdCategory_List(vendorid);
                    ProdCatListDTO.DataSource = inventory;
                    ProdCatListDTO.DataBind();
                }, "Purchase Order Info", "Create purchase order for this vendor.");
            }
        }

        protected void DeleteProduct_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int index;
                LinkButton button = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)button.Parent.Parent;
                index = gvr.RowIndex;
                List<PurchaseOrder> POList = new List<PurchaseOrder>();
                loadPOGridView(POList);
                POList.RemoveAt(index);
                PurchaseOrderGV.DataSource = POList;
                PurchaseOrderGV.DataBind();
                calculatePOValue();
            }, "Deleted", "The product has been deleted from the purchase order.");
        }

        protected void InventoryList_SelectedIndexChanged(object sender, EventArgs e)

        {
            GridView grd = (GridView)sender;
            GridViewRow agvrow = grd.Rows[grd.SelectedIndex];
            int prodid = int.Parse((agvrow.FindControl("InvProductID") as Label).Text);
            int vendorid = int.Parse(VendorList.SelectedValue);
            MessageUserControl.TryRun(() =>
            {
                PurchasingController sysmgr = new PurchasingController();
                List<PurchaseOrder> purchaseorder = new List<PurchaseOrder>();
                loadPOGridView(purchaseorder);
                PurchaseOrder newitem = sysmgr.InventoryToPO(vendorid, prodid, purchaseorder);
                purchaseorder.Add(newitem);
                if (newitem == null)
                {
                    throw new BusinessRuleException("Input Error", new List<string> {
                        "This item is in the purchase order already. You can modify the Order Qty if you need." });
                }
                PurchaseOrderGV.DataSource = purchaseorder;
                PurchaseOrderGV.DataBind();
                calculatePOValue();
            }, "Item added", "The item has been added to the purchase order.");
            //MessageUserControl.ShowInfo("The Product ID selected",
            //         (agvrow.FindControl("InvProductID") as Label).Text);
        }

        protected void loadPOGridView(List<PurchaseOrder> POList)
        {
            foreach (GridViewRow row in PurchaseOrderGV.Rows)
            {
                if (!int.TryParse((row.FindControl("OrderQty") as TextBox).Text, out int qtycheck) || qtycheck <= 0)
                {
                    throw new BusinessRuleException("Input Error", new List<string> { "Order Quantity must be an integer number greater than zero." });
                }
                if (!decimal.TryParse((row.FindControl("UnitCost") as TextBox).Text, out decimal costcheck) || costcheck <= 0)
                {
                    throw new BusinessRuleException("Input Error", new List<string> { "Unit Cost must be a number greater than zero." });
                }
                POList.Add(new PurchaseOrder
                {
                    ProductID = int.Parse((row.FindControl("ProductID") as Label).Text),
                    Product = (row.FindControl("Product") as Label).Text,
                    OrderQty = int.Parse((row.FindControl("OrderQty") as TextBox).Text),
                    UnitSize = int.Parse((row.FindControl("UnitSize") as Label).Text),
                    UnitType = (row.FindControl("UnitType") as Label).Text,
                    UnitCost = decimal.Parse((row.FindControl("UnitCost") as TextBox).Text),
                    Warning = " ",
                    PerItemCost = decimal.Parse((row.FindControl("PerItemCost") as Label).Text),
                    ExtendedCost = decimal.Parse((row.FindControl("ExtendedCost") as Label).Text)
                });
            }
        }

        public void calculatePOValue()
        {
            decimal subtotal = 0;
            foreach (GridViewRow row in PurchaseOrderGV.Rows)
            {
                decimal extendedcost = decimal.Parse((row.FindControl("ExtendedCost") as Label).Text);
                //if ((row.FindControl("ExtendedCost") as Label).Text != null)
                    subtotal += extendedcost;
            }
            Subtotal.Text = $"{subtotal: #,0.00}";
            Tax.Text = $"{subtotal * 5 / 100: #,0.00}";
            Total.Text = $"{subtotal + (subtotal * 5 / 100): #,0.00}";
        }

        protected void RefreshPrice_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int index;
                LinkButton button = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)button.Parent.Parent;
                index = gvr.RowIndex;
                PurchasingController sysmgr = new PurchasingController();
                List<PurchaseOrder> refreshlist = new List<PurchaseOrder>();
                loadPOGridView(refreshlist);
                refreshlist[index].PerItemCost = refreshlist[index].UnitCost / refreshlist[index].UnitSize;
                refreshlist[index].ExtendedCost = refreshlist[index].UnitCost * refreshlist[index].OrderQty;
                int prodid = refreshlist[index].ProductID;
                refreshlist[index].Warning = sysmgr.Warning(prodid, refreshlist);
                PurchaseOrderGV.DataSource = refreshlist;
                PurchaseOrderGV.DataBind();
                calculatePOValue();
            }, "Refresh", "The purchase order is updated.");
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            int vendorid = int.Parse(VendorList.SelectedValue.ToString());

            MessageUserControl.TryRun(() =>
            {
                PurchasingController sysmgr = new PurchasingController();
                sysmgr.DeletePO(vendorid);
            }, "Order Deleted", "The order has been deleted from the database.");

            Cancel_Click(sender, e);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            VendorList.SelectedValue = "0";
            VName.Text = "";
            Contact.Text = "";
            Phone.Text = "";
            Comments.Text = "";
            UsersList.SelectedValue = "0";

            List<PurchaseOrder> emptypo = null;
            PurchaseOrderGV.DataSource = emptypo;
            PurchaseOrderGV.DataBind();

            List<ProdCategoryInfo> emptyinv = null;
            ProdCatListDTO.DataSource = emptyinv;
            ProdCatListDTO.DataBind();
            calculatePOValue();
        }


        protected void Save_Click(object sender, EventArgs e)
        {

            PurchasingController sysmgr = new PurchasingController();
            List<PurchaseOrder> savelist = new List<PurchaseOrder>();
            loadPOGridView(savelist);
            calculatePOValue();
            InfoToSave infoToSave = new InfoToSave
            {
                EmployeeID = int.Parse(UsersList.SelectedValue),
                VendorID = int.Parse(VendorList.SelectedValue),
                Comments = Comments.Text,
                Subtotal = decimal.Parse(Subtotal.Text),
                Tax = decimal.Parse(Tax.Text)
            };
            MessageUserControl.TryRun(() =>
            {
                sysmgr.SavePO(savelist, infoToSave);
            }, "Saving Purchase Order", "The purchase order has been saved.");

        }

        protected void PlaceOrder_Click(object sender, EventArgs e)
        {
            PurchasingController sysmgr = new PurchasingController();
            List<PurchaseOrder> savelist = new List<PurchaseOrder>();
            loadPOGridView(savelist);
            calculatePOValue();
            InfoToSave infoToSave = new InfoToSave
            {
                EmployeeID = int.Parse(UsersList.SelectedValue),
                VendorID = int.Parse(VendorList.SelectedValue),
                Comments = Comments.Text,
                Subtotal = decimal.Parse(Subtotal.Text),
                Tax = decimal.Parse(Tax.Text)
            };
            MessageUserControl.TryRun(() =>
            {
                sysmgr.PlacePO(savelist, infoToSave);
            }, "Placing Order", "The purchase order has been placed.");
            if (UsersList.SelectedValue != "0")
                Cancel_Click(sender, e);

        }
    }
}