using DeRacersSystem.BLL.Sales;
using DeRacersSystem.Data.SalesPocos;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.Sales
{
  public partial class Refunds : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LookUpInvoice_Click(object sender, EventArgs e)
    {
      int invoiceNumber;
      MessageUserControl.TryRun(() =>
      {
        if (InvoiceNumberTextBox.Text == "" || !(int.TryParse(InvoiceNumberTextBox.Text, out invoiceNumber)))
        {
          throw new BusinessRuleException("Lookup Error", new List<String> { "Invoice number required" });
        }
        else
        {
          SalesController sc = new SalesController();
          List<RefundItem> refundList = sc.Get_RefundItemsByInvoiceNumber(invoiceNumber);
          if (refundList.Count == 0)
          {
            throw new BusinessRuleException("Lookup Error", new List<String> { "Invoice not in database" });
          }
          else
          {
            BindNewGridView(refundList);
            updatedTotals(refundList);
          }
        }
      }, "Success", "Invoice found");

    }

    protected void RefundButton_Click(object sender, EventArgs e)
    {
      SalesController sc = new SalesController();
      MessageUserControl.TryRun(() =>
      {
        RefundInvoiceTextBoxDisplay.Text = sc.Do_RefundTransaction(DumpGridView()).ToString();
      }, "Success", "Refund Completed");

      TextBox textBox = new TextBox();
      foreach (GridViewRow row in InvoiceGridView.Rows)
      {
        textBox = (TextBox)row.FindControl("Reason");
        textBox.ReadOnly = true;
      }
      RefundButton.Enabled = false;
    }
    public void clearTotals()
    {
      SubtotalTextBox.Text = "";
      TaxTextBox.Text = "";
      RefundTotalTextBox.Text = "";
    }
    public void updatedTotals(List<RefundItem> refundList)
    {
      double subTotal = 0;
      double tax = 0;
      double total = 0;
      foreach (RefundItem item in refundList)
      {
        if (item.ItemToBeRefunded)
        {
          subTotal += item.ExtenedAmount;
          tax += (subTotal * 0.05);
          if (item.HasRestockCharge)
          {
            subTotal -= item.RestockCharge;
          }
        }
      }
      total += (subTotal + tax);
      total *= -1;
      tax *= -1;
      subTotal *= -1;
      SubtotalTextBox.Text = string.Format("{0:c2}", subTotal);
      TaxTextBox.Text = string.Format("{0:c2}", tax);
      RefundTotalTextBox.Text = string.Format("{0:c2}", total);
    }

    protected void ClearButton_Click(object sender, EventArgs e)
    {
      RefundInvoiceTextBoxDisplay.Text = "";
      InvoiceNumberTextBox.Text = "";
      clearTotals();
      InvoiceGridView.DataSource = new List<RefundItem>();
      InvoiceGridView.DataBind();
      RefundButton.Enabled = true;
    }

    protected void Reason_TextChanged(object sender, EventArgs e)
    {
      List<RefundItem> refundList = DumpGridView();
      BindNewGridView(refundList);
      updatedTotals(refundList);
    }
    private List<RefundItem> DumpGridView()
    {
      List<RefundItem> refundList = new List<RefundItem>();
      Label label;
      TextBox textBox;
      CheckBox checkBox;
      foreach (GridViewRow row in InvoiceGridView.Rows)
      {
        RefundItem temp = new RefundItem();

        label = (Label)row.FindControl("ProductID");
        temp.ProductID = int.Parse(label.Text);

        label = (Label)row.FindControl("InvoiceID");
        temp.InvoiceID = int.Parse(label.Text);

        label = (Label)row.FindControl("CategoryID");
        temp.CategoryID = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductName");
        temp.ProductName = label.Text;

        label = (Label)row.FindControl("ProductQuantity");
        temp.ProductQuantity = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductPrice");
        temp.ProductPrice = Double.Parse(label.Text, System.Globalization.NumberStyles.Currency);

        label = (Label)row.FindControl("ProductPrice");
        temp.ProductPrice = Double.Parse(label.Text, System.Globalization.NumberStyles.Currency);

        checkBox = (CheckBox)row.FindControl("HasRestockCharge");
        if (checkBox.Checked)
        {
          label = (Label)row.FindControl("RestockCharge");
          temp.RestockCharge = Double.Parse(label.Text, System.Globalization.NumberStyles.Currency);
        }
        else
        {
          temp.RestockCharge = 0;
        }

        textBox = (TextBox)row.FindControl("Reason");
        temp.Reason = textBox.Text;

        refundList.Add(temp);
      }
      return refundList;
    }
    public void MakeConfectionaryItemInvisible(GridView gr)
    {
      foreach(GridViewRow row in gr.Rows)
      {
        Label label;
        CheckBox checkBox;
        TextBox textBox;
        label = (Label)row.FindControl("CategoryID");
        if(label.Text == "3")
        {
          checkBox = (CheckBox)row.FindControl("HasRestockCharge");
          checkBox.Visible = false;

          checkBox = (CheckBox)row.FindControl("ItemToBeRefunded");
          checkBox.Visible = false;

          label = (Label)row.FindControl("RestockCharge");
          label.Visible = false;

          textBox = (TextBox)row.FindControl("Reason");
          textBox.Visible = false;
        }
      }
    }
    private void BindNewGridView(List<RefundItem> list)
    {
      InvoiceGridView.DataSource = list;
      InvoiceGridView.DataBind();
      MakeConfectionaryItemInvisible(InvoiceGridView);
    }
  }
}