<%@ Page Title="eRace - Purchasing" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Purchasing.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <style type="text/css">
        .rightAlign { text-align:right; }
    </style>

    <%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

        <blockquote class="alert alert-info">
            <uc1:messageusercontrol runat="server" id="MessageUserControl" />
        </blockquote>
        <br />

    <%--Employee Name DDL--%>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-8"></div>
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Employee Name:" Font-Size="Medium" ></asp:Label>&nbsp
                    <asp:DropDownList ID="UsersList" runat="server" Width="200px"
                        AppendDataBoundItems="true" AutoPostBack="true" DataSourceID="UserListODS" 
                        DataTextField="TextField" DataValueField="ValueField" Font-Size="Small">
                        <asp:ListItem Value="0">Select a name...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    </br></br></br></br>
    
    <div class="row">
        <div class="col-md-12" style="padding:0px">
            <div class="row">
            <div class="col-md-6" style="padding:0px">
                <h1><b>Purchase Order</b></h1></br></br>
                <%--Vendor info--%>
                <div class="row">
                    <asp:Label runat="server" Text="Vendor:" Font-Size="Medium"></asp:Label>&nbsp
                    <asp:DropDownList ID="VendorList" runat="server" Width="180px" 
                        AppendDataBoundItems="true" AutoPostBack="true" Font-Size="Small"
                        DataSourceID="VendorListODS" DataTextField="TextField" DataValueField="ValueField">
                        <asp:ListItem Value="0">[Select a Vendor]</asp:ListItem>
                    </asp:DropDownList>&nbsp&nbsp
                    <asp:Button ID="SelectVendor" runat="server" Text="Select" CssClass="btn btn-primary btn-sm" OnClick="SelectVendor_Click" />&nbsp
                    <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" 
                                    OnClientClick="return confirm(' Are you sure you wish to delete this purchase order?')" OnClick="Delete_Click" />&nbsp&nbsp&nbsp&nbsp
                    <asp:Button ID="PlaceOrder" runat="server" Text="Place Order" CssClass="btn btn-info btn-sm" OnClick="PlaceOrder_Click" />&nbsp
                    <asp:Button ID="Save" runat="server" Text="Save" CssClass="btn btn-success btn-sm" OnClick="Save_Click" />&nbsp&nbsp&nbsp&nbsp
                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-secondary btn-sm" OnClick="Cancel_Click" />
                </div>
                </br>
                <div class="row">
                    <div class="col-md-8">
                        <div class="row">
                            <div class="form" id="VendorInfo" runat="server">
                                <div class="row">
                                    <div class="col-md-5">
                                        <asp:Label runat="server" Text="Vendor Name: " Font-Size="Small"></asp:Label></br>
                                        <asp:Label runat="server" Text="Contact: " Font-Size="Small"></asp:Label></br>
                                        <asp:Label runat="server" Text="Phone: " Font-Size="Small"></asp:Label></br>
                                    </div>
                                    <div class="col-md-7" >
                                        <asp:Label ID="VName" runat="server"></asp:Label></br>
                                        <asp:Label ID="Contact" runat="server"></asp:Label></br>
                                        <asp:Label ID="Phone" runat="server"></asp:Label></br>
                                    </div>
                                </div>
                                <asp:TextBox ID="Comments" runat="server" TextMode="MultiLine" Columns="55" Rows="3" placeholder="Comments" 
                                    Font-Size="Small" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <%--Order amounts--%>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-5" style="padding:0px">
                                <asp:Label runat="server" Text="Subtotal ($): " Font-Size="Small" ></asp:Label></br></br>
                                <asp:Label runat="server" Text="Tax ($): " Font-Size="Small" ></asp:Label></br></br>
                                <asp:Label runat="server" Text="Total ($): " Font-Size="Small" ></asp:Label></br></br>
                            </div>
                            <div class="col-md-7">
                                <asp:TextBox ID="Subtotal" runat="server" Width="80px" Height="20px" Font-Size="Small" CssClass="pull-left rightAlign" ></asp:TextBox></br></br>
                                <asp:TextBox ID="Tax" runat="server" Width="80px" Height="20px" Font-Size="Small" CssClass="pull-left rightAlign"></asp:TextBox></br></br>
                                <asp:TextBox ID="Total" runat="server" Width="80px" Height="20px" Font-Size="Small" CssClass="pull-left rightAlign"></asp:TextBox></br></br>
                            </div>
                        </div>
                    </div>
                </div>
            <%--</div>--%>
            </br></br>

            <%--Purchase Order--%>
            
                <asp:GridView ID="PurchaseOrderGV" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" BackColor="LightCyan" HeaderStyle-BackColor="PaleTurquoise"
                    DataKeyNames="ProductID" Font-Size="8pt" BorderStyle="None" Width="100%" HeaderStyle-HorizontalAlign="Center" CellPadding="2">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="35px" >
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteProduct" runat="server" CssClass="btn" OnClick="DeleteProduct_Click" >
                                    <i class="fa fa-times" aria-hidden="true" style="color:red"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product">
                            <ItemTemplate>
                                <asp:Label ID="ProductID" runat="server" Text='<%# Eval("ProductID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="Product" runat="server" Text='<%# Eval("Product") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="50px" >
                            <ItemTemplate>
                                <asp:TextBox ID="OrderQty" runat="server" Text='<%# Eval("OrderQty") %>'
                                                style="text-align:right" Width="45px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Size" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="UnitSize" runat="server" Text='<%# Eval("UnitSize") %>'></asp:Label>
                                <asp:Label runat="server" Text=" per "></asp:Label>
                                <asp:Label ID="UnitType" runat="server" Text='<%# Eval("UnitType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Cost ($)" HeaderStyle-Width="50px" >
                            <ItemTemplate>
                                <asp:TextBox ID="UnitCost" runat="server" Text='<%# string.Format("{0:#0.##}", Eval("UnitCost")) %>' 
                                            style="text-align:right" Width="50px" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Per Item Cost ($)" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  >
                            <ItemTemplate>
                                <asp:LinkButton ID="RefreshPrice" runat="server" CssClass="btn" OnClick="RefreshPrice_Click" >
                                    <i class="fa fa-refresh" aria-hidden="true" style="color:blue; width:5px; padding:0px"></i>
                                </asp:LinkButton>
                                <asp:Label ID="Warning" runat="server" Text='<%# Eval("Warning") %>'  ForeColor="Red" ></asp:Label>
                                <asp:Label ID="PerItemCost" runat="server" Text='<%# string.Format("{0:#0.##}", Eval("PerItemCost")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Extended Cost ($)"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="ExtendedCost" runat="server" Text='<%# string.Format("{0:#0.00}", Eval("ExtendedCost")) %>'
                                            style="text-align: right" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

                <div class="col-md-1"></div>
            
            <%--Inventory list--%>
            <div class="col-md-5" style="padding:0px">
                <h1><b>Inventory</b></h1></br></br>
                <asp:Repeater ID="ProdCatListDTO" runat="server" 
                    ItemType="DeRacersSystem.Data.PurchasingDTOs.ProdCategoryInfo">
                <ItemTemplate>
                    <h3><%# Item.Category %></h3>
                    <asp:GridView ID="InventoryList" runat="server" AutoGenerateColumns="false" DataSource="<%# Item.ProductList %>" HeaderStyle-BackColor="LightGray"
                        ItemType="DeRacersSystem.Data.PurchasingPOCOs.InventoryInfo" OnSelectedIndexChanged="InventoryList_SelectedIndexChanged"
                                 DataKeyNames="ProductID" CssClass="table table-hover" Font-Size="8pt" >
                        <Columns>
                            <asp:CommandField SelectText="" ShowSelectButton="True" 
                                ControlStyle-CssClass="fa fa-plus" ControlStyle-ForeColor="Green">
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="InvProductID" runat="server" Text='<%# Eval("ProductID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="InvProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reorder" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="Reorder" runat="server" Text='<%# Eval("Reorder") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Stock" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="InStock" runat="server" Text='<%# Eval("InStock") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="On Order" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="OnOrder" runat="server" Text='<%# Eval("OnOrder") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Size">
                                <ItemTemplate>
                                    <asp:Label ID="UnitType" runat="server" Text='<%# Eval("UnitType") %>' ></asp:Label>
                                    <asp:Label ID="UnitSize" runat="server" Text='<%# (Eval("UnitSize").ToString() == "1")? "" : " (" + Eval("UnitSize") + ")" %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:Repeater>
            </div>
            </div>
        </div>
    </div>

    










    <asp:ObjectDataSource ID="UserListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="User_List" 
        TypeName="DeRacersSystem.BLL.Purchasing.PurchasingController"
        OnSelected="CheckForException">
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="VendorListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Vendor_List" 
        TypeName="DeRacersSystem.BLL.Purchasing.PurchasingController"
        OnSelected="CheckForException">
    </asp:ObjectDataSource>


</asp:Content>
