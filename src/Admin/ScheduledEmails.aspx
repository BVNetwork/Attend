<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduledEmails.aspx.cs" Inherits="BVNetwork.Attend.Admin.ScheduledEmails" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>
<%@ Register TagPrefix="Attend" TagName="EmailList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/EmailList.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    <h1>
        <EPiServer:Translate Text="/attend/admin/scheduledemail" runat="server" />
    </h1>
    <div class="row">
        <div class="col-lg-6">
            <h2><episerver:translate runat="server" text="/attend/admin/messagesToBeSent"></episerver:translate></h2>
            <Attend:EmailList ID="UpcomingControl"  runat="server"/>
        </div>
        <div class="col-lg-6">
            <div class="well">
                <h2><episerver:translate runat="server" text="/attend/admin/messagesToBeSentNow"></episerver:translate></h2>
                <Attend:EmailList ID="SendNowControl"  runat="server"/>
            </div>
        </div>
    </div>
</asp:Content>
