<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="row">
    <div class="col-md-12">
      <h2>Application Info</h2>
      <p>
        <b>Connection String</b> RaceContext"
    connectionString="data source=RIRI;initial catalog=eRaceDb;integrated 
    security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient"
      </p>
    </div>
  </div>
  <div class="row">
    <div class="col-md-12">
      <h1>Security Info</h1>
      <div class="col-md-12" style="display: flex; justify-content: space-between; align-items: flex-start; flex-wrap: wrap;">
        <ul>
          <li>
            <h3>Sales</h3>
          </li>
          <li>Clerk</li>
        </ul>
        <ul>
          <li>
            <h3>Receiving</h3>
          </li>
          <li>Clerk</li>
          <li>Food Service</li>
        </ul>
        <ul>
          <li>
            <h3>Race</h3>
          </li>
          <li>Race Coordinator</li>
        </ul>
        <ul>
          <li>
            <h3>Purchasing</h3>
          </li>
          <li>Director</li>
          <li>Office Manager</li>
        </ul>
      </div>
    </div>
  </div>



</asp:Content>
