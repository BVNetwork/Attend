<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="BVNetwork.Attend.Admin.Events" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>
<%@ Register TagPrefix="Attend" TagName="ParticipantList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/ParticipantList.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Sidebar">
            <div class="form-horizontal">
            <div class="col-lg-12">

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

                <asp:DropDownList runat="server" CssClass="form-control" AutoPostBack="true" ID="DatePeriod" >
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
    

</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="MainArea">

    <h1>
        <EPiServer:Translate Text="/attend/admin/events" runat="server" />
    </h1>
    <div class="">
        <div class="row">

            <div class="col-lg-12">
                <Attend:ParticipantList runat="server" ID="AttendParticipantList" />
            </div>
        </div>
    </div>

</asp:Content>