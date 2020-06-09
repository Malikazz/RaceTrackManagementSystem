using DeRacersSystem.BLL.Sales;
using DeRacersSystem.Data.SalesPocos;
using DMIT2018Common.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.Sales
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void AddProductButton_Click(object sender, EventArgs e)
    {
      // all local vars
      int categoryId, productId, productQuantity;
      CartDisplay cartItem;
      bool isNewItem = true;
      List<CartDisplay> cartList = new List<CartDisplay>();
      // Make sure we have numbers
      refreshSalesGridView(cartList);
      
      // Run BLL
      MessageUserControl.TryRun(() =>
      {
         if (int.TryParse(CategorySelectDDL.SelectedValue, out categoryId) && int.TryParse(ProductSelectDDL.SelectedValue, out productId))
        {
          foreach(CartDisplay item in cartList)
          {
            if(item.ProductID == productId && item.CategoryID == categoryId)
            {
              isNewItem = false;
              int.TryParse(NumberOfProductsTextBox.Text, out productQuantity);
              if(productQuantity > 0)
              {
                item.ProductQuantity += productQuantity;
                updateTotals(item);
              }
              
            }
          }
          if (int.TryParse(NumberOfProductsTextBox.Text, out productQuantity) && productQuantity > 0)
          {
            if (isNewItem)
            {
              SalesController sc = new SalesController();
              cartItem = sc.Get_CartDisplayItem(productId, categoryId);
              if(productQuantity > 0)
              {
                cartItem.ProductQuantity = productQuantity;
                cartList.Add(cartItem);
                updateTotals(cartItem);
              }
            }
          }
          else
          {
            throw new BusinessRuleException("Add Product Error", new List<String> { "Quantity must be greater then 0" });
          }
        }
        else
        {
          throw new BusinessRuleException("Selected Value Error", new List<String> { "Values where not numbers" });
        }
      });
      resetInputs();
      CartGridView.DataSource = cartList;
      CartGridView.DataBind();
    }

    protected void ClearForm_Click(object sender, EventArgs e)
    {
      List<CartDisplay> cartList = new List<CartDisplay>();
      clearTotals();
      resetInputs();
      CartGridView.DataSource = cartList;
      CartGridView.DataBind();
      AddProductButton.Enabled = true;
    }

    protected void PaymentButton_Click(object sender, EventArgs e)
    {
      MessageUserControl.TryRun(() =>
      {
        if (TransactionLabel.Text != "")
        {
          throw new BusinessRuleException("Cart Error", new List<string> { "Must clear current transaction before starting a new one" });
        }
        else
        {
          SalesController sc = new SalesController();
          List<CartDisplay> cartList = new List<CartDisplay>();
          int transactionID;
          refreshSalesGridView(cartList, true);
          transactionID = sc.Do_CartTransaction(cartList);
          resetInputs();
          TransactionLabel.Text = "#" + transactionID.ToString();
        }
        
      },"Transaction: ", "Was successful clear the cart for next order");
      AddProductButton.Enabled = false;
      LinkButton link = new LinkButton();
      foreach(GridViewRow row in CartGridView.Rows)
      {
        link = (LinkButton)row.FindControl("DeleteItemButton");
        link.Enabled = false;
      }
    }

    protected void RefreshButton_Click(object sender, EventArgs e)
    {
      List<CartDisplay> cartList = new List<CartDisplay>();
      refreshSalesGridView(cartList);

      CartGridView.DataSource = cartList;
      CartGridView.DataBind();
    }

    protected void DeleteItemButton_Click(object sender, EventArgs e)
    {
      List < CartDisplay > cartList = new List<CartDisplay>();
      int index;
      GridViewRow gvr;
      LinkButton button = (LinkButton)sender;
      gvr = (GridViewRow)button.Parent.Parent;
      index = gvr.RowIndex;

      refreshSalesGridView(cartList);
      cartList.Remove(cartList[index]);

      CartGridView.DataSource = cartList;
      CartGridView.DataBind();
    }
    private void refreshSalesGridView(List<CartDisplay> cartList)
    {
      TextBox textBox;
      Label label;
      int userInput;
      double userInputDouble;
      //zero totals
      SubTotalLabel.Text = "0";
      TotalLabel.Text = "0";
      TaxTotal.Text = "0";
      foreach (GridViewRow row in CartGridView.Rows)
      {
        CartDisplay cartItemTemp = new CartDisplay();

        label = (Label)row.FindControl("CategoryID");
        cartItemTemp.CategoryID = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductID");
        cartItemTemp.ProductID = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductName");
        cartItemTemp.ProductName = label.Text;

        textBox = (TextBox)row.FindControl("ProductRowQuantity");
        if (int.TryParse(textBox.Text, out userInput) && userInput > 0)
        {
          cartItemTemp.ProductQuantity = userInput;
        }
        else if (double.TryParse(textBox.Text, out userInputDouble) && userInputDouble > 0.0)
        {
          cartItemTemp.ProductQuantity = (int)userInputDouble;
        }
        else
        {
          cartItemTemp.ProductQuantity = 1;
        }

        label = (Label)row.FindControl("PriceLabel");
        cartItemTemp.ProductPrice = Double.Parse(label.Text, System.Globalization.NumberStyles.Currency);

        cartList.Add(cartItemTemp);
        updateTotals(cartItemTemp);
      }
    }
    private void refreshSalesGridView(List<CartDisplay> cartList, bool isTransaction)
    {
      TextBox textBox;
      Label label;
      //zero totals
      SubTotalLabel.Text = "0";
      TotalLabel.Text = "0";
      TaxTotal.Text = "0";
      foreach (GridViewRow row in CartGridView.Rows)
      {
        CartDisplay cartItemTemp = new CartDisplay();

        label = (Label)row.FindControl("CategoryID");
        cartItemTemp.CategoryID = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductID");
        cartItemTemp.ProductID = int.Parse(label.Text);

        label = (Label)row.FindControl("ProductName");
        cartItemTemp.ProductName = label.Text;

        textBox = (TextBox)row.FindControl("ProductRowQuantity");
        textBox.Attributes.Add("readonly", "true");
        cartItemTemp.ProductQuantity = int.Parse(textBox.Text);

        label = (Label)row.FindControl("PriceLabel");
        cartItemTemp.ProductPrice = Double.Parse(label.Text, System.Globalization.NumberStyles.Currency);

        cartList.Add(cartItemTemp);
        updateTotals(cartItemTemp);
      }
    }
    private void resetInputs()
    {
      NumberOfProductsTextBox.Text = "";
      CategorySelectDDL.SelectedIndex = 0;
      ProductSelectDDL.SelectedIndex = 0;
    }

    protected void ProductRowQuantity_TextChanged(object sender, EventArgs e)
    {
      List<CartDisplay> cartList = new List<CartDisplay>();
      refreshSalesGridView(cartList);

      CartGridView.DataSource = cartList;
      CartGridView.DataBind();
    }
    private void updateTotals(CartDisplay item)
    {
      double subtotal, tax, total;
      if(!(Double.TryParse(SubTotalLabel.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out subtotal)))
      {
        subtotal = 0;
      }
      if (!(Double.TryParse(TaxTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out tax)))
      {
        tax = 0;
      }
      if (!(Double.TryParse(TotalLabel.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out total)))
      {
        total = 0;
      }
      subtotal += item.ExtendedAmount;
      tax += subtotal * 0.05;
      total += subtotal + tax;

      SubTotalLabel.Text = string.Format("{0:c2}", subtotal);
      TaxTotal.Text = string.Format("{0:c2}", tax);
      TotalLabel.Text = string.Format("{0:c2}", total);
    }
    private void clearTotals()
    {
      SubTotalLabel.Text = "";
      TaxTotal.Text = "";
      TotalLabel.Text = "";
      TransactionLabel.Text = "";
    }
  }
}