<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoicing.aspx.cs" Inherits="BVNetwork.Attend.Admin.Invoicing" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>
<%@ Register TagPrefix="Attend" TagName="InvoiceList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/InvoiceList.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Sidebar">
    <div class="row">
        <div class="col-lg-12">
            <div class="form-horizontal">

                <div class='input-group date' id='datetimepicker1'>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxFromDate"></asp:TextBox>

                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>

                <div class='input-group date' id='datetimepicker1'>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxToDate"></asp:TextBox>

                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>

                <asp:DropDownList runat="server" CssClass="form-control" AutoPostBack="true" ID="DatePeriod">
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.selectperiod %>" Value=""></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.thisyear %>" Value="thisyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.lastyear %>" Value="lastyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.nextyear %>" Value="nextyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.thismonth %>" Value="thismonth"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.lastmonth %>" Value="lastmonth"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.nextmonth %>" Value="nextmonth"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton runat="server" CssClass="btn btn-default btn-block" Text="Search" OnClick="ChangeDate_OnClick"></asp:LinkButton>
                <br />

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">

            <div class="">
                Fields to display
        <asp:TextBox runat="server" CssClass="form-control" Height="150" TextMode="MultiLine" ID="InvoiceFieldsTextBox"></asp:TextBox>
                <asp:LinkButton runat="server" CssClass="btn btn-default btn-block" ID="SaveSettingsButton" Text="Save settings" OnClick="SaveSettingsButton_OnClick"></asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <br/>
            <asp:LinkButton runat="server" ID="ExportButton" CssClass="btn btn-block btn-primary" Text="<%$ Resources: EPiServer, attend.edit.export %>" OnClick="ExportButton_OnClick"></asp:LinkButton>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    <h1>
        <EPiServer:Translate Text="/attend/admin/invoicing" runat="server" />
    </h1>
    <div class="well">
        <br />
        <Attend:InvoiceList runat="server" ID="AttendInvoiceList" />
    </div>


</asp:Content>
