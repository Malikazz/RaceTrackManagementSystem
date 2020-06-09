<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Refunds.aspx.cs" Inherits="WebApp.Sales.Refunds" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <h1>Refunds</h1>
  <h2><a href="Default.aspx">Sales</a></h2>

  <div class="row" style="margin-bottom: 30px; margin-top: 1em">
    <div class="col-md-12">
      <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
  </div>
  <div class="row" style="margin-bottom: 30px; margin-top: 1em">
    <div class="col-md-6" style="display: flex; align-items: center;">
      <asp:TextBox Style="margin-right: 15px" runat="server" ID="InvoiceNumberTextBox" placeholder="Search Invoice #"></asp:TextBox>
      <asp:Button Style="margin-right: 15px" runat="server" ID="LookUpInvoice" Text="Lookup Invoice" OnClick="LookUpInvoice_Click" UseSubmitBehavior="false" CssClass="btn btn-secondary" />
      <asp:Button runat="server" ID="ClearButton" Text="Clear" OnClick="ClearButton_Click" UseSubmitBehavior="false" CssClass="btn btn-secondary" />
    </div>
  </div>
  <div class="row" style="margin-bottom: 30px; margin-top: 1em">
    <asp:GridView ID="InvoiceGridView" runat="server" AutoGenerateColumns="False">
      <Columns>
        <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID" Visible="false">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# Eval("ProductID") %>' ID="ProductID" Visible="false"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CategoryID" SortExpression="CategoryID" Visible="false">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# Eval("CategoryID") %>' ID="CategoryID" Visible="false"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="InvoiceID" SortExpression="InvoiceID" Visible="false">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# Eval("InvoiceID") %>' ID="InvoiceID" Visible="false"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name" SortExpression="ProductName">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# Eval("ProductName") %>' ID="ProductName"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Quantity" SortExpression="ProductQuantity" ItemStyle-HorizontalAlign="Right">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# Eval("ProductQuantity") %>' ID="ProductQuantity"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Price" SortExpression="ProductPrice" ItemStyle-HorizontalAlign="Right">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%# string.Format("{0:c2}", Eval("ProductPrice")) %>' ID="ProductPrice"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount" SortExpression="ExtenedAmount" ItemStyle-HorizontalAlign="Right">
          <ItemTemplate>
            <asp:Label runat="server" Text='<%#string.Format("{0:c2}", Eval("ExtenedAmount")) %>' ID="ExtenedAmount"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Restock Charge" SortExpression="RestockCharge" ItemStyle-HorizontalAlign="Right">
          <ItemTemplate>
            <asp:CheckBox runat="server" Checked='<%# Eval("HasRestockCharge") %>' Enabled="false" ID="HasRestockCharge"></asp:CheckBox>
            <asp:Label runat="server" Text='<%# string.Format("{0:c2}", Eval("RestockCharge")) %>' ID="RestockCharge"></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Refund Reason" SortExpression="RefundReason">
          <ItemTemplate>
            <asp:CheckBox runat="server" Checked='<%# Eval("ItemToBeRefunded") %>' Enabled="false" ID="ItemToBeRefunded"></asp:CheckBox>
            <asp:TextBox runat="server" Text='<%# Eval("Reason") %>' ID="Reason" OnTextChanged="Reason_TextChanged" AutoPostBack="true"></asp:TextBox>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
  </div>
  <div class="row">
    <div class="col-md-6" style="display: flex; justify-content: space-between; align-items: flex-start; flex-direction: column;">
      <div style="display: flex; align-items: flex-start; justify-content: space-between; min-width: 250px">
        <asp:Label Style="flex-basis: 40%" ID="SubtotalLabel" runat="server" Text="Subtotal"></asp:Label>
        <asp:TextBox Style="flex-basis: 60%; max-width: 150px; text-align: right" ID="SubtotalTextBox" runat="server" ReadOnly="true"></asp:TextBox>
      </div>
      <div style="display: flex; align-items: flex-start; justify-content: space-between; min-width: 250px">
        <asp:Label Style="flex-basis: 40%" ID="TaxLabel" runat="server" Text="Tax"></asp:Label>
        <asp:TextBox Style="flex-basis: 60%; max-width: 150px; text-align: right" ID="TaxTextBox" runat="server" ReadOnly="true"></asp:TextBox>
      </div>
      <div style="display: flex; align-items: flex-start; justify-content: space-between; min-width: 250px">
        <asp:Label Style="flex-basis: 40%; font-weight: bold;" ID="TotalLabel" runat="server" Text="Refund Total"></asp:Label>
        <asp:TextBox Style="flex-basis: 60%; max-width: 150px; text-align: right" ID="RefundTotalTextBox" runat="server" ReadOnly="true"></asp:TextBox>
      </div>
    </div>
    <div class="col-md-6" style="display: flex; align-items: flex-start; flex-direction: column;">
      <asp:Button CssClass="btn btn-success" Style="margin-bottom: 1em;" runat="server" Text="Refund" ID="RefundButton" OnClick="RefundButton_Click" UseSubmitBehavior="false" />
      <asp:TextBox runat="server" placeholder="Refund Invoice #" ID="RefundInvoiceTextBoxDisplay" ReadOnly="true"></asp:TextBox>
    </div>
  </div>
</asp:Content>
