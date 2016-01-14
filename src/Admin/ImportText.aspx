<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportText.aspx.cs" Inherits="BVNetwork.Attend.Admin.ImportText" MasterPageFile="/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>

<%@ Import Namespace="EPiServer" %>
<%@ Register TagPrefix="Attend" TagName="ImportMenu" Src="~/Modules/BVNetwork.Attend/Admin/Partials/ImportMenu.ascx" %>
<%@ Register TagPrefix="sc" Assembly="EPiServer.Shell" Namespace="EPiServer.Shell.Web.UI.WebControls" %>
<%@ Register TagPrefix="ux" Namespace="EPiServer.Web.WebControls" Assembly="EPiServer" %>
<%@ Register TagPrefix="ux" Namespace="EPiServer.Web.PropertyControls" Assembly="EPiServer" %>
<asp:Content runat="server" ContentPlaceHolderID="Sidebar">
    <Attend:ImportMenu runat="server"></Attend:ImportMenu>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    <h1>
        <episerver:translate text="/attend/admin/importfromtext" runat="server" />
    </h1>
    <div class="">
        <div class="well">
            <div class="panel panel-body">
                <episerver:translate text="/attend/admin/rootpageatdest" runat="server" />
                <br />
                <ux:inputpagereference runat="server" id="RootTextBox" />
                <br />
                Participants<br/>
                <asp:TextBox runat="server" ID="ParticipantsTextBox" TextMode="MultiLine" Width="500" Height="200"></asp:TextBox><br />
                <br />
                <asp:Button runat="server" OnClick="ImportBtn_Click" Text="<%$ Resources: EPiServer, attend.admin.importparticipants %>" />

            </div>
            <br />
            <div class="panel panel-body">
                <asp:Literal runat="server" ID="StatusLiteral"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
