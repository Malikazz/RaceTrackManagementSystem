<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Receiving.Default" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1>Receiving</h1>
    </div>
        <uc1:MessageUserControl runat="server" id="MessageUserControl" />
    <div class="row">
        <asp:DropDownList runat="server" ID="VendorDDL" DataSourceID="VendorDataSource" DataTextField="VendorOrderNumber" DataValueField="VendorID" Width="275px" AppendDataBoundItems="true">
            <asp:ListItem Value="0" Selected="true">Select a PO</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="OrderFetch" runat="server" Text="Open" CssClass="btn btn-primary" OnClick="PurchaseOrderFetch_Click"/>
    </div>
     &nbsp;
    &nbsp;
    <div class="row">
        <div class="col-md-8">
            <asp:Repeater ID="VendorContact" runat="server" DataSourceID="VendorContactDataSource" ItemType="DeRacersSystem.Data.ReceivingPOCOs.VendorContact">
                <ItemTemplate>
                <div class="row"> 
                    <div class="col-sm-4">
                        <p><b><%# Item.VendorName %></b></p>
                        <p><%# Item.Contact %></p>
                    </div>
                   <div class="col-sm-4">
                    <p><%# Item.Address %></p>
                    <p><%# Item.Phone %></p>
                   </div>        
                </div>
                              
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div>
            <asp:Button ID="ReceiveOrder" runat="server" Text="Receive Shipment" CssClass="btn btn-success" Visible="false" OnClick="ReceiveShipment_Click"/>
        </div>    
    </div>

    &nbsp;
    &nbsp;
    <div class="row">
        <asp:GridView runat="server" ID="PurchaseOrderGridView" AutoGenerateColumns="False" DataSourceID="PurchaseOrderDataSource">
            <Columns>
                <asp:TemplateField SortExpression="OrderID" Visible="false">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("OrderID") %>' runat="server" ID="OrderID" Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item"></asp:BoundField>
                <asp:BoundField DataField="QuantityOrdered" HeaderText="Quantity Ordered" SortExpression="QuantityOrdered" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="OrderedUnits" HeaderText="Ordered Units" SortExpression="OrderedUnits"></asp:BoundField>
                <asp:TemplateField  HeaderText="Quantity Outstanding" ItemStyle-Width="80px"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("QuantityOutstanding") %>' runat="server" ID="QuantityOutstanding"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Received Units">
                    <ItemTemplate>
                        <asp:Textbox runat="server" ID="ReceivedUnitsBox" Width="30px"/>
                        x case of
                        <asp:Label Text='<%# Eval("ReceivedUnitSize") %>' runat="server" ID="UnitLabel"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rejected Units / Reason" ItemStyle-Width="218px">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="RejectedUnitsBox" Width="30px" ></asp:TextBox>
                        <asp:TextBox runat="server" ID="RejectedReason"></asp:TextBox>
                    </ItemTemplate>                  
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Salvaged Items" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="SalvagedItemsBox" Width="40px"></asp:TextBox>
                    </ItemTemplate>                  
                </asp:TemplateField>               
            </Columns>
        </asp:GridView>
    </div>
    &nbsp;
    &nbsp;
    <div class="row">
        <div class="col-md-4">
            <h4>Unordered Items</h4>     
            <asp:ListView ID="UnOrderedItems" runat="server" DataSourceID="UnorderedItemDataSource" InsertItemPosition="LastItem" Visible="false" DataKeyNames="ItemID">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFF8DC;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CssClass="btn btn-danger"/>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                        <td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemID") %>' runat="server" ID="ItemIDLabel" Visible="false"/></td>
                            <td>
                            <asp:Label Text='<%# Eval("OrderID") %>' runat="server" ID="Label1" Visible="false"/></td>
                        
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductIDLabel" Visible="false" /></td>
                        
                    </tr>
                </AlternatingItemTemplate>
                <EditItemTemplate>
                    <tr style="background-color: #008A8C; color: #FFFFFF;">
                        <td>
                            <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                            <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                        </td> 
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemName") %>' runat="server" ID="ItemNameTextBox"/></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox"  /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemID") %>' runat="server" ID="ItemIDTextBox" Visible="false" /></td>
                         <td>
                            <asp:TextBox Text='<%# Bind("OrderID") %>' runat="server" ID="TextBox1" Visible="false" /></td>   
                        <td>
                            <asp:TextBox Text='<%# Bind("VendorProductID") %>' runat="server" ID="VendorProductIDTextBox" Visible="false" /></td>
                        
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" CssClass="btn btn-success"/>
                            <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" CssClass="btn btn-danger"/>
                        </td>
                        
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemName") %>' runat="server" ID="ItemNameTextBox" Width="250px"/></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" Width="90px" /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemID") %>' runat="server" ID="ItemIDTextBox" Visible="false"/></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("VendorProductID") %>' runat="server" ID="VendorProductIDTextBox" Visible="false"/></td>
                        
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr style="background-color: #DCDCDC; color: #000000;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CssClass="btn btn-danger"/>
                        </td>
                       
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel"/></td>
                         <td>
                            <asp:Label Text='<%# Eval("ItemID") %>' runat="server" ID="ItemIDLabel" Visible="false" Enabled="false"/></td> 
                        <td>
                            <asp:Label Text='<%# Eval("ItemID") %>' runat="server" ID="OrderIDLabel" Visible="false"/></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductIDLabel" Visible="false"/></td>
                        
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                        <th runat="server"></th>
                                        <th runat="server">ItemName</th>
                                        <th runat="server">Quantity</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <SelectedItemTemplate>
                    <tr style="background-color: #008A8C; font-weight: bold; color: #FFFFFF;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CssClass="btn btn-danger"/>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemID") %>' runat="server" ID="ItemIDLabel"  /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" Visible="false" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductIDLabel" Visible="false" /></td>

                    </tr>
                </SelectedItemTemplate>
            </asp:ListView>
             &nbsp;
             &nbsp;
            <div class="col-md-12">                      
                <asp:Button ID="Close" runat="server" Text="Force Close" CssClass="btn btn-danger" Visible="false" OnClientClick="return confirm('Are you sure you wish to force close the order?')" OnClick="ForceClose_Click"/>
                <asp:TextBox ID="CommentBox" runat="server" Rows="5" Columns="20" TextMode="MultiLine" Width="391px" Visible="false"></asp:TextBox>             
        </div>
        </div>
        </div>
        
   

    <asp:ObjectDataSource runat="server" ID="VendorContactDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="GetVendorContacts" TypeName="DeRacersSystem.BLL.Receiving.ReceivingController">
        <SelectParameters>
            <asp:ControlParameter ControlID="VendorDDL" PropertyName="SelectedValue" Name="vendorid" Type="Int32" DefaultValue="0"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="PurchaseOrderDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="GetPurchaseOrder" TypeName="DeRacersSystem.BLL.Receiving.ReceivingController">
        <SelectParameters>
            <asp:ControlParameter ControlID="VendorDDL" PropertyName="SelectedValue" DefaultValue="0" Name="vendorid" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="UnorderedItemDataSource" DataObjectTypeName="DeRacersSystem.Data.Entities.UnOrderedItem" DeleteMethod="Item_Delete" InsertMethod="UnOrderedItem_Add" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetUnOrderedItems" TypeName="DeRacersSystem.BLL.Receiving.ReceivingController"
        OnInserted="InsertCheckForException"
        OnDeleted="DeleteCheckForException">
        <InsertParameters>
            <asp:ControlParameter ControlID="VendorDDL" PropertyName="SelectedValue" DefaultValue="0" Name="vendorid" Type="Int32"></asp:ControlParameter>
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="VendorDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="GetVendors" TypeName="DeRacersSystem.BLL.Receiving.ReceivingController"></asp:ObjectDataSource>
</asp:Content>
