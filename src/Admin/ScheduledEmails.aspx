<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduledEmails.aspx.cs" Inherits="BVNetwork.Attend.Admin.ScheduledEmails" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>
<%@ Register TagPrefix="Attend" TagName="EmailList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/EmailList.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    <h1>
        <EPiServer:Translate Text="/attend/admin/scheduledemail" runat="server" />
    </h1>
    <div class="well">
        <br />
        <Attend:EmailList runat="server"/>
    </div>
</asp:Content>
