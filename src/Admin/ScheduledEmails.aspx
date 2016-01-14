<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduledEmails.aspx.cs" Inherits="BVNetwork.Attend.Admin.ScheduledEmails" MasterPageFile="/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>

<%@ Import Namespace="EPiServer.Framework.Web.Resources" %>
<%@ Import Namespace="EPiServer.Framework.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer" %>
<%@ Register TagPrefix="Attend" TagName="EmailList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/EmailList.ascx" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<%@ Register TagPrefix="sc" Assembly="EPiServer.Shell" Namespace="EPiServer.Shell.Web.UI.WebControls" %>


<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    <h1>
        <EPiServer:Translate Text="/attend/admin/scheduledemail" runat="server" />
    </h1>
    <div class="well">
        <br />
        <Attend:EmailList runat="server"/>
    </div>


</asp:Content>
