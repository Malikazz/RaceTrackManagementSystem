<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="text-center">
    <asp:Image ImageUrl="img/logo.png" runat="server" CssClass="" AlternateText="Team D Logo" />
  </div>
  <div class="row">
    <div class="col-md-12">
      <h1>Team D</h1>
      <div class="col-md-12" style="display: flex; justify-content: space-between; align-items: flex-start; flex-wrap: wrap;">
        <ul>
          <li>Paul Malcolm
                    <ul>
                      <li>
                        <a href="Sales/Default.aspx">Sales</a>
                        <ul>
                          <li><b>Known Bugs</b></li>
                          <li>None currently known</li>
                        </ul>

                      </li>
                      <li>
                        Project Setup Tasks
                        <ul>
                          <li>Setup Readme</li>
                          <li>Setup main page</li>
                          <li>Setup About page</li>
                          <li>Changed required classes to internal</li>
                          <li>BootStrap4 Menu</li>
                          <li>Connection Strings</li>
                        </ul>
                      </li>
                    </ul>
          </li>
        </ul>
        <ul>
          <li>John Klassen
                    <ul>
                      <li>
                        <a href="Race/Default.aspx">Race</a>
                        <ul>
                          <li><b>Known Bugs</b></li>
                          <li>None currently known</li>
                        </ul>
                      </li>
                        <li>
                            Project Setup Tasks
                            <ul>
                                <li>
                                    Customized Validation Error Messages
                                </li>
                                <li>
                                    Setup Common Employee ID List and Controller
                                </li>
                            </ul>
                        </li>
                    </ul>
          </li>
        </ul>
        <ul>
          <li>Jeremy Cortez
                    <ul>
                      <li>
                        <a href="Receiving/Default.aspx">Receiving</a>
                        <ul>
                          <li><b>Known Bugs</b></li>
                          <li>Unordered Items not fully functional for BLL Transactions</li>
                           <li>Unordered Items does not clear on PostBack</li>
                        </ul>

                      </li>
                    </ul>
          </li>
        </ul>
        <ul>
          <li>Gigel Gherghelau
                    <ul>
                      <li>
                        <a href="Purchasing/Default.aspx">Purchasing</a>
                        <ul>
                          <li><b>Known Bugs</b></li>
                          <li>None currently known</li>
                        </ul>
                      </li>
                      <li>
                        Project Setup Tasks
                        <ul>
                          <li>Setup the new project</li>
                          <li>Solution Structure: Web Application, Class Library(ies), Folders for BLL, DAL</li>
                          <li>Setup web.config</li>
                          <li>Created Entities</li>
                          <li>DbContext class </li>
                          <li>NuGet Package Update</li>
                          <li>Created Group logo</li>
                        </ul>
                      </li>
                    </ul>
          </li>
        </ul>
      </div>
    </div>
  </div>

</asp:Content>
