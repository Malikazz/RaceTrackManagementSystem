<%@ Page Title="Racing -" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Race.Default" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Race</h1><br />
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" /><br />
    <div class="row">
        <div class="col-md-4">
            <asp:DropDownList ID="EmployeeDDL" runat="server" DataSourceID="EmployeeDDLODS" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            <br /><br />
            <asp:Calendar ID="RaceCalendar" runat="server" OnSelectionChanged="RaceCalendar_SelectionChanged"></asp:Calendar>
        </div>
        <asp:Panel ID="SchedulePanel" runat="server" Visible="false">
            <div class="col-md-8">
                <h2>Schedule</h2><br />
                <asp:GridView ID="ScheduleGV" runat="server" 
                    AutoGenerateColumns="False" 
                    DataSourceID="SceduleViewODS" 
                    AllowPaging="True" 
                    OnRowCommand="ScheduleGV_RowCommand"
                    DataKeyNames="RaceID"> 
                    <Columns>
                        <asp:TemplateField HeaderStyle-BackColor="#c0c0c0">
                            <ItemTemplate>
                                <asp:LinkButton ID="ScheduleViewButton" runat="server" CommandArgument='<%# Eval("RaceID") %>' Text="View" Width="50px"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time" SortExpression="Time" HeaderStyle-BackColor="#c0c0c0">
                            <ItemTemplate>
                                <asp:Label ID="TimeID" runat="server" Text='<%# ((DateTime)Eval("Time")).ToString("h tt") %>' Width="60px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Competition" SortExpression="Competition" HeaderStyle-BackColor="#c0c0c0">
                            <ItemTemplate>
                                <asp:Label ID="CompetitionID" runat="server" Text='<%# Eval("Competition") %>' Width="350px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Run" SortExpression="Run" HeaderStyle-BackColor="#c0c0c0">
                            <ItemTemplate>
                                <asp:Label ID="RunID" runat="server" Text='<%# Eval("Run") %>' Width="40px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Drivers" SortExpression="Drivers" HeaderStyle-BackColor="#c0c0c0">
                            <ItemTemplate>
                                <asp:Label ID="DriversID" runat="server" Text='<%# Eval("Drivers") %>' Width="60px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>There are no races scheduled on this day.</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <asp:Panel ID="RosterPanel" runat="server" Visible="false">
            <div class="col-md-6">
                <h2>Roster</h2> <asp:Button ID="RecordRaceTimesButtonID" runat="server" Text="Record Race Times" OnClick="RecordRaceTimesButtonID_Click" />
                <asp:Label ID="RaceID" runat="server" Text="" Visible="false"></asp:Label>
                <asp:ListView ID="RosterLV" runat="server"
                    InsertItemPosition="LastItem"
                    OnItemCommand="RosterLV_ItemCommand">
                    <AlternatingItemTemplate>
                        <asp:Panel ID="ItemView" runat="server" Visible="true">
                            <tr style="background-color: #FFFFFF; color: #284775;">
                                <td>
                                    <asp:Button runat="server" CommandName="EditContestant" Text="Edit" ID="EditButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("RaceDetailID") %>' runat="server" ID="RaceDetailIDLabel" Visible="false"/>
                                    <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" Width="200px"/>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RaceFee")), 2) %>' runat="server" ID="RaceFeeLabel" Width="100px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RentalFee")), 2) %>' runat="server" ID="RentalFeeLabel" Width="100px" style="text-align: right" />
                                </td> 
                                <td>
                                    <asp:Label Text='<%# Eval("Placement") %>' runat="server" ID="PlacementLabel" Width="75px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:CheckBox Checked='<%# Eval("Refunded") %>' runat="server" ID="RefundedCheckBox" Enabled="false" Width="75px"/>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="EditView" runat="server" Visible="false">
                            <tr style="background-color: #FFFFFF; color: #284775;">
                                <td>
                                    <asp:Button runat="server" CommandName="UpdateContestant" Text="Update" ID="UpdateButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("RaceDetailID") %>' runat="server" ID="ERaceDetailIDLabel" Visible="false"/>
                                    <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="ENameLabel" Width="250px"/>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RaceFee")), 2) %>' runat="server" ID="ERaceFeeLabel" Width="100px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ECarClassDDL" runat="server"
                                        DataSourceID="CarClassListODS"
                                        DataTextField="CarClassName"
                                        DataValueField="CarClassID"
                                        AppendDataBoundItems="true"
                                        Width="100px"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0" Text="N/A"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ECarDDL" runat="server"
                                        DataSourceID="EVINListODS"
                                        DataTextField="SerialNumber"
                                        DataValueField="CarID"
                                        AppendDataBoundItems="true"
                                        Width="150px">
                                        <asp:ListItem Value="0" Text="Select a Car"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="background-color: #FFFFFF; color: #284775;">
                                <td>
                                    <asp:Button runat="server" CommandName="CancelContestant" Text="Cancel" ID="CancelButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="ECommentTextBox" runat="server" Text='<%# Eval("Comment") %>' placeholder="Comment" Width="300px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="EReasonTextBox" runat="server" Text='<%# Eval("RefundReason") %>' placeholder="Refund Reason" Width ="175px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox Checked='<%# Eval("Refunded") %>' runat="server" ID="ERefundedCheckbox" Width="75px"/>
                                </td>
                            </tr>
                            <asp:ObjectDataSource ID="EVINListODS" runat="server" 
                                OldValuesParameterFormatString="original_{0}" 
                                SelectMethod="Get_VINList" 
                                TypeName="DeRacersSystem.BLL.Racing.CarController">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ECarClassDDL" 
                                        PropertyName="SelectedValue" 
                                        DefaultValue="0" 
                                        Name="carclassid" 
                                        Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </AlternatingItemTemplate>
                    
                    <EmptyDataTemplate>
                        <table runat="server" style="">
                            <tr>
                                <td>No data was returned.</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                        <tr style="background-color: #DDDDDD; color: #284775;">
                            <td>
                                <asp:Button runat="server" CommandName="InsertContestant" Text="Add" ID="Button1" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="IDriverDDL" runat="server"
                                    DataSourceID="DriverListODS"
                                    DataTextField="Name"
                                    DataValueField="MemberID"
                                    AppendDataBoundItems="true" 
                                    Width="250px">
                                    <asp:ListItem Value="0" Text="Driver"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label Text="" runat="server" ID="IRaceFeeLabel" Width="100px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ICarClassDDL" runat="server"
                                    DataSourceID="CarClassListODS"
                                    DataTextField="CarClassName"
                                    DataValueField="CarClassID"
                                    AppendDataBoundItems="true"
                                    Width="100px"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0" Text="N/A"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ICarDDL" runat="server"
                                    DataSourceID="IVINListODS"
                                    DataTextField="SerialNumber"
                                    DataValueField="CarID"
                                    AppendDataBoundItems="true"
                                    Width="150px">
                                    <asp:ListItem Value="0" Text="Select a Car"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <asp:ObjectDataSource ID="IVINListODS" runat="server" 
                            OldValuesParameterFormatString="original_{0}" 
                            SelectMethod="Get_VINList" 
                            TypeName="DeRacersSystem.BLL.Racing.CarController">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ICarClassDDL" 
                                    PropertyName="SelectedValue" 
                                    Name="carclassid" 
                                    Type="Int32"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Panel ID="ItemView" runat="server" Visible="true">
                            <tr style="background-color: #E0FFFF; color: #333333;">
                                <td>
                                    <asp:Button runat="server" CommandName="EditContestant" Text="Edit" ID="EditButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("RaceDetailID") %>' runat="server" ID="RaceDetailIDLabel" Visible="false"/>
                                    <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" Width="250px"/>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RaceFee")), 2) %>' runat="server" ID="RaceFeeLabel" Width="100px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RentalFee")), 2) %>' runat="server" ID="RentalFeeLabel" Width="100px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("Placement") %>' runat="server" ID="PlacementLabel" Width="75px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:CheckBox Checked='<%# Eval("Refunded") %>' runat="server" ID="RefundedCheckBox" Enabled="false" Width="75px"/>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="EditView" runat="server" Visible="false">
                            <tr style="background-color: #E0FFFF; color: #333333;">
                                <td>
                                    <asp:Button runat="server" CommandName="UpdateContestant" Text="Update" ID="UpdateButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("RaceDetailID") %>' runat="server" ID="ERaceDetailIDLabel" Visible="false"/>
                                    <asp:Label Text='<%# Eval("Name") %>' runat="server" ID="ENameLabel" Width="250px"/>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Math.Round((Decimal)(Eval("RaceFee")), 2) %>' runat="server" ID="ERaceFeeLabel" Width="100px" style="text-align: right" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ECarClassDDL" runat="server"
                                        DataSourceID="CarClassListODS"
                                        DataTextField="CarClassName"
                                        DataValueField="CarClassID"
                                        AppendDataBoundItems="true"
                                        Width="100px"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0" Text="N/A"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ECarDDL" runat="server"
                                        DataSourceID="EVINListODS"
                                        DataTextField="SerialNumber"
                                        DataValueField="CarID"
                                        AppendDataBoundItems="true"
                                        Width="150px">
                                        <asp:ListItem Value="0" Text="Select a Car"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="background-color: #E0FFFF; color: #333333;">
                                <td>
                                    <asp:Button runat="server" CommandName="CancelContestant" Text="Cancel" ID="CancelButton" CommandArgument="<%# Container.DataItemIndex %>" Width="70px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="ECommentTextBox" runat="server" Text='<%# Eval("Comment") %>' placeholder="Comment" Width="300px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="EReasonTextBox" runat="server" Text='<%# Eval("RefundReason") %>' placeholder="Refund Reason" Width ="175px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox Checked='<%# Eval("Refunded") %>' runat="server" ID="ERefundedCheckbox" Width="75px"/>
                                </td>
                            </tr>
                            <asp:ObjectDataSource ID="EVINListODS" runat="server" 
                                OldValuesParameterFormatString="original_{0}" 
                                SelectMethod="Get_VINList" 
                                TypeName="DeRacersSystem.BLL.Racing.CarController">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ECarClassDDL" 
                                        PropertyName="SelectedValue" 
                                        DefaultValue="0" 
                                        Name="carclassid" 
                                        Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                        <tr runat="server" style="background-color: #DDDDDD; color: #284775;">
                                            <th runat="server"></th>
                                            <th runat="server">Name</th>
                                            <th runat="server">RaceFee</th>
                                            <th runat="server">RentalFee</th>
                                            <th runat="server">Placement</th>
                                            <th runat="server">Refunded</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" style=""></td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </asp:Panel>
        <asp:Panel ID="RaceResultPanel" runat="server" Visible="false">
            <div class="col-md-6">
                <h2>Race Results</h2> <asp:Button ID="SaveRaceTimesButtonID" runat="server" Text="Save Times" OnClick="SaveRaceTimesButtonID_Click" />
                <asp:GridView ID="RaceResultsGV" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="RaceDetailID" runat="server" Text='<%# Eval("RaceDetailID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="NameID" runat="server" Text='<%# Eval("Name") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle BackColor="Silver"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time (HH:mm:ss)">
                            <ItemTemplate>
                                <asp:TextBox ID="TimeID" runat="server" Text='<%# Eval("Time") %>' Width="150px"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle BackColor="Silver"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Penalties">
                            <ItemTemplate>
                                <asp:DropDownList ID="PenaltyDDL" runat="server"
                                    DataSourceID="PenaltyListODS"
                                    DataTextField="Description"
                                    DataValueField="PenaltyID"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%# Eval("Penalties") %>'
                                    Width="75px">
                                    <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle BackColor="Silver"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    <asp:ObjectDataSource ID="EmployeeDDLODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Get_Employees" 
        TypeName="DeRacersSystem.BLL.Common.EmployeeController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="SceduleViewODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Get_ScheduleView" 
        TypeName="DeRacersSystem.BLL.Racing.RaceController">
        <SelectParameters>
            <asp:ControlParameter ControlID="RaceCalendar" PropertyName="SelectedDate" Name="date" Type="DateTime"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="CarClassListODS" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="Get_CarClassList"
        TypeName="DeRacersSystem.BLL.Racing.CarClassController">
        <SelectParameters>
            <asp:ControlParameter ControlID="RaceID"
                PropertyName="Text"
                Name="raceid"
                Type="Int32"
                DefaultValue="0"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="DriverListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Get_DriverList" 
        TypeName="DeRacersSystem.BLL.Racing.MemberController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PenaltyListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Get_PenaltyList" 
        TypeName="DeRacersSystem.BLL.Racing.RacePenaltyController">
    </asp:ObjectDataSource>

</asp:Content>
