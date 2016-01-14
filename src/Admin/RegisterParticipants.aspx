<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterParticipants.aspx.cs" Inherits="BVNetwork.Attend.Admin.RegisterParticipants" MasterPageFile="/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>

<%@ Import Namespace="EPiServer.Framework.Web.Resources" %>
<%@ Import Namespace="EPiServer.Framework.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer" %>
<%@ Register TagPrefix="Attend" TagName="InvoiceList" Src="~/Modules/BVNetwork.Attend/Admin/Partials/InvoiceList.ascx" %>
<%@ Register TagPrefix="Attend" TagName="EventFilter" Src="~/Modules/BVNetwork.Attend/Admin/Partials/EventFilter.ascx" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<%@ Register TagPrefix="sc" Assembly="EPiServer.Shell" Namespace="EPiServer.Shell.Web.UI.WebControls" %>


<asp:Content runat="server" ContentPlaceHolderID="Sidebar">
    <div class="col-lg-12">
    <div class="form-horizontal">
        <div class="row">
            <asp:Panel DefaultButton="RegisterByCodeButton" runat="server">
                    <h3>
                        <EPiServer:Translate runat="server" text="/attend/edit/code"></EPiServer:Translate></h3>
                    <div class='input-group'>
                        <span class="input-group-addon"><span class="glyphicon glyphicon-barcode"></span></span>

                        <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxBarCode"></asp:TextBox>
                        <span class="input-group-btn">
                            <asp:LinkButton runat="server" CssClass="btn btn-success btn-md" ID="RegisterByCodeButton" OnClick="RegisterByCodeButton_Click">&nbsp;<span class="glyphicon glyphicon-chevron-right"></span></asp:LinkButton>
                        </span>
                    </div>
                    <div class="form-control">
                        <asp:CheckBox ID="NoshowCheckBox" runat="server" Checked="false" />
                        <EPiServer:translate runat="server" text="/attend/admin/noshow" />
                    </div>

            </asp:Panel>
        </div></div>
                </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">
<div class="col-lg-6">
    <h1>
        <EPiServer:Translate Text="/attend/admin/registerparticipant" runat="server" />
    </h1>
    </div>
                <div class="col-lg-6">
    
        <br />
    </div>
    <div class="col-lg-6">
    <div class="well">
        Participation status:<br />
        <div class="progress" style="background-color: #ccc; margin-top: 5px;">
            <%=GetProgressBar(CurrentEvent) %>
        </div>
        <%=ParticipatedParticipants() %> of <%=ExpectedParticipants() %> participants registered.

    </div>

    <asp:PlaceHolder runat="server" ID="StatusPlaceHolder" Visible="false">
        <div class="well">
            <asp:Literal ID="StatusLiteral" runat="server"></asp:Literal>
        </div>
    </asp:PlaceHolder>
        </div>

    <div class="row">
    <div class="col-lg-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title"><EPiServer:translate runat="server" text="/attend/edit/confirmed"></EPiServer:translate></h3>
            </div>
            <div class="panel-body">

                <asp:Repeater runat="server" ID="NotRegisteredRepeater">
                    <HeaderTemplate>
                        <table class="table table-hover table-responsive table-striped">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Email %></td>
                            <td><%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Code %></td>
                            <td>
                                <asp:LinkButton runat="server" CssClass="btn btn-success" ID="RegisterParticipantButton" CommandArgument='<%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Email+","+(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Code%>' OnCommand="RegisterParticipantButton_Command"><span class='glyphicon glyphicon-chevron-right'></span></asp:LinkButton></td>

                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr class="info">
                            <td>
                                <EPiServer:translate runat="server" text="/attend/admin/registerall"></EPiServer:translate></td>
                            <td></td>
                            <td>
                                <asp:LinkButton runat="server" CssClass="btn btn-success" ID="RegisterAllParticipantsButton" OnClick="RegisterAllParticipantsButton_Click"><span class='glyphicon glyphicon-chevron-right'></span><span class='glyphicon glyphicon-chevron-right'></span></asp:LinkButton></td>
                            </td>
                        </tr>
                        </table>

                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div class="col-lg-6">
        <div class="panel panel-success">
            <div class="panel-heading">
                <h3 class="panel-title"> <EPiServer:translate runat="server" text="/attend/admin/participated"></EPiServer:translate></h3>
            </div>
            <div class="panel-body">
                <asp:Repeater runat="server" ID="RegisteredRepeater">
                    <HeaderTemplate>
                        <table class="table table-hover table-responsive table-striped">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="">
                            <td><%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Email %></td>
                                                        <td><%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Code %></td>

                            <td>
                                <asp:LinkButton runat="server" CssClass="btn btn-default" ID="RegisterParticipantButton" CommandArgument='<%#(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Email+","+(Container.DataItem as BVNetwork.Attend.Business.Participant.IParticipant).Code%>' OnCommand="UnRegisterParticipantButton_Command"><span class='glyphicon glyphicon-chevron-left'></span></asp:LinkButton></td>

                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>

                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>

    </div>
</asp:Content>
