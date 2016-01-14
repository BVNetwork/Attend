<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="BVNetwork.Attend.Admin.Import" MasterPageFile="/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>

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
        <episerver:translate text="/attend/admin/importfromeventx" runat="server" />
    </h1>
    <div class="">
        <div class="well">
            <div class="panel panel-body">
                <episerver:translate text="/attend/admin/urltoimportfrom" runat="server" />
                :<br />
                <asp:TextBox runat="server" ID="RemoteSiteTextBox"></asp:TextBox><br />
                <br />
                <episerver:translate text="/attend/admin/rootpageatsource" runat="server" />
                <br />
                <asp:TextBox runat="server" ID="RemoteRootTextBox"></asp:TextBox><br />
                <br />
                <episerver:translate text="/attend/admin/rootpageatdest" runat="server" />
                <br />
                <ux:inputpagereference runat="server" id="RootTextBox" />
                <br />
                <episerver:translate text="/attend/admin/registrationform" runat="server" />
                <br />
                <ux:inputxform runat="server" id="XFormInput" />
                <br />
                <br />
                <episerver:translate text="/attend/admin/language" runat="server" />
                <br />
                <asp:TextBox runat="server" ID="LanguageTextBox"></asp:TextBox><br />
                <br />
                <asp:Button runat="server" OnClick="TestBtn_Click" Text="<%$ Resources: EPiServer, attend.admin.testconnection %>" />
                <asp:Button runat="server" OnClick="ImportBtn_Click" Text="<%$ Resources: EPiServer, attend.admin.importevents %>" />

            </div>
            <br />
            <div class="panel panel-body">
                <asp:Literal runat="server" ID="StatusLiteral"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
