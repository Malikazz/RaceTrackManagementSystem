<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Sales.Default" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <h1>Sales</h1>
  <h2><a href="Refunds.aspx">Refunds</a></h2>
  <div class="row">
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <h2 style="margin-bottom: 1em; margin-top: 2em;">In-Store Purchase</h2>
        </div>
      </div>
      <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        <div class="col-md-12" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1em;">
          <asp:DropDownList AutoPostBack="true" runat="server" ID="CategorySelectDDL" DataSourceID="CategoryDDL" DataTextField="CategoryName" DataValueField="CategoryID"></asp:DropDownList>
          <asp:DropDownList runat="server" ID="ProductSelectDDL" DataSourceID="ProductDDL" DataTextField="ProductName" DataValueField="ProductID"></asp:DropDownList>
          <asp:TextBox Style="width: 50px" runat="server" ID="NumberOfProductsTextBox" type="Integer"></asp:TextBox>
          <asp:Button UseSubmitBehavior="false" CssClass="btn btn-primary" runat="server" ID="AddProductButton" OnClick="AddProductButton_Click" Text="Add" type="submit" />
        </div>
      </div>
      <div class="row">
        <div class="col-md-12">
          <asp:GridView ID="CartGridView" runat="server" AutoGenerateColumns="False">
            <Columns>
              <asp:TemplateField HeaderText="CategoryID" SortExpression="CategoryID" Visible="False">
                <ItemTemplate>
                  <asp:Label runat="server" Text='<%# Bind("CategoryID") %>' ID="CategoryID"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID" Visible="False">
                <ItemTemplate>
                  <asp:Label runat="server" Text='<%# Bind("ProductID") %>' ID="ProductID"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="ProductName" SortExpression="ProductName">
                <ItemTemplate>
                  <asp:Label runat="server" Text='<%# Bind("ProductName") %>' ID="ProductName"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="ProductQuantity" SortExpression="ProductQuantity" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                  <asp:TextBox type="Integer" OnTextChanged="ProductRowQuantity_TextChanged" Style="width: 50px; text-align: right;" runat="server" Text='<%# Bind("ProductQuantity") %>' ID="ProductRowQuantity"></asp:TextBox>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="ProductPrice" SortExpression="ProductPrice" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                  <asp:LinkButton OnClick="RefreshButton_Click" runat="server" ID="RefreshButton" CssClass="material-icons">refresh</asp:LinkButton>
                  <asp:Label runat="server" Text='<%# string.Format("{0:c2}", Eval("ProductPrice")) %>' ID="PriceLabel" Style="text-align: right"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="ExtendedAmount" SortExpression="ExtendedAmount" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                  <asp:Label runat="server" Text='<%# string.Format("{0:c2}", Eval("ExtendedAmount")) %>' ID="ExtendedAmount" Style="text-align: right;"></asp:Label>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="">
                <ItemTemplate>
                  <asp:LinkButton OnClick="DeleteItemButton_Click" runat="server" ID="DeleteItemButton" CssClass="material-icons">delete</asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </div>
      </div>
      <div class="row" style="display: flex; justify-content: space-between; align-items: flex-start">
        <div class="col-md-6" style="display: flex; justify-content: space-between; align-items: center">
          <asp:Button CssClass="btn btn-success" runat="server" ID="PaymentButton" Text="Payment" OnClick="PaymentButton_Click" />
          <asp:Button CssClass="btn btn-secondary" runat="server" ID="ClearForm" Text="Clear Cart" type="clear" OnClick="ClearForm_Click" />
        </div>
        <div class="col-md-6" style="display: flex; flex-direction: column; justify-content: space-between; align-items: flex-end;">
          <div>
            <asp:Label runat="server" ID="Sub" Text="SubTotal:"></asp:Label>
            <asp:Label Style="min-width: 100px" runat="server" ID="SubTotalLabel"></asp:Label>
          </div>
          <div>
            <asp:Label runat="server" ID="Label1" Text="Tax:"></asp:Label>
            <asp:Label Style="min-width: 100px" runat="server" ID="TaxTotal"></asp:Label>
          </div>
          <div>
            <asp:Label runat="server" ID="Label2" Text="Total:"></asp:Label>
            <asp:Label Style="min-width: 100px" runat="server" ID="TotalLabel"></asp:Label>
          </div>
          <div>
            <asp:Label runat="server" ID="Label3" Text="Transaction:"></asp:Label>
            <asp:Label Style="min-width: 100px" runat="server" ID="TransactionLabel"></asp:Label>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-6">
    </div>
  </div>

  <asp:ObjectDataSource ID="CategoryDDL" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_CategoryDDL" TypeName="DeRacersSystem.BLL.Sales.SalesController"></asp:ObjectDataSource>
  <asp:ObjectDataSource ID="ProductDDL" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_ProductDDL" TypeName="DeRacersSystem.BLL.Sales.SalesController">

    <SelectParameters>
      <asp:ControlParameter ControlID="CategorySelectDDL" PropertyName="SelectedValue" DefaultValue="999999999999" Name="categoryID" Type="Int32"></asp:ControlParameter>
    </SelectParameters>
  </asp:ObjectDataSource>
</asp:Content>
