<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventPageEditScheduledEmail.ascx.cs" Inherits="BVNetwork.Attend.Views.Pages.Partials.EventPageEditScheduledEmail" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<table class="table table-striped table-hover table-responsive">
    <tr>
        <th>Status</th>
        <th>E-mail</th>
        <th>Scheduled</th>
        <th>Filter</th>
        <th>Type</th>
        <th></th>
    </tr>
    <EPiServer:Property runat="server" id="ScheduledEmailContentArea">
        <rendersettings tag="ListView" />
    </EPiServer:Property>


</table>



<div class="margin-top:20px;">
<asp:Button runat="server" CssClass="btn btn-primary " OnClick="CreateNew_Click" Text="Create new"/> </div>

<div class="margin-top:20px;"><br/></div>