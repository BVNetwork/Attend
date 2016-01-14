<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.TopMenu" %>

<div class="navbar navbar-default">
  <div class="navbar-header">
    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-responsive-collapse">
      <span class="icon-bar"></span>
      <span class="icon-bar"></span>
      <span class="icon-bar"></span>
    </button>
    <a class="navbar-brand" href="/modules/BVNetwork.Attend/Admin/Participants.aspx" ><span class="pull-left"> ATTEND</span></a>
  </div>
  <div class="navbar-collapse collapse navbar-responsive-collapse">
    <ul class="nav navbar-nav">
      <li class=""><a href="/modules/BVNetwork.Attend/Admin/Events.aspx">Events</a></li>
      <li><a href="/modules/BVNetwork.Attend/Admin/Participants.aspx">Participants</a></li>
          <li><a href="/modules/BVNetwork.Attend/Admin/Import.aspx">Import</a></li>
          <li><a href="/modules/BVNetwork.Attend/Admin/Invoicing.aspx">Invoicing</a></li>
          <li><a href="/modules/BVNetwork.Attend/Admin/ScheduledEmails.aspx">Scheduled E-mails</a></li>
          <li><a href="/modules/BVNetwork.Attend/Admin/SettingsEdit.aspx">Settings</a></li>
    </ul>
  </div>
</div>
