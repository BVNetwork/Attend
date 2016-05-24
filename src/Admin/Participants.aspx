<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Participants.aspx.cs" Inherits="BVNetwork.Attend.Admin.Participants" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>
<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Participant" %>

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

                <asp:DropDownList runat="server" CssClass="form-control" AutoPostBack="true" ID="DatePeriod"  OnSelectedIndexChanged="DatePeriod_OnSelectedIndexChanged">
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.selectperiod %>" Value=""></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.thisyear %>" Value="thisyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.lastyear %>" Value="lastyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.nextyear %>" Value="nextyear"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.thismonth %>" Value="thismonth"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.lastmonth %>" Value="lastmonth"></asp:ListItem>
                    <asp:ListItem runat="server" Text="<%$ Resources: EPiServer, attend.edit.nextmonth %>" Value="nextmonth"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton runat="server" CssClass="btn btn-default btn-block" Text="<span class='glyphicon glyphicon-search'></span>&nbsp; Search" OnClick="ChangeDate_OnClick"></asp:LinkButton>
                <br />
                <div class="panel panel-default">
                    <div class="panel-body">

                        <div class="form-group">

                            <div class="col-lg-12">
                                <div class="col-lg-12 ">
                                    <span class="help-inline"><episerver:Translate runat="server" text="/attend/admin/fieldstodisplay"></episerver:Translate></span>
                                    <asp:CheckBoxList runat="server" ID="FormFieldsCheckBoxList" CssClass="checkbox col-lg-12" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <asp:LinkButton runat="server" CssClass="btn btn-primary btn-block" ID="LinkButton1" Text="<%$ Resources: EPiServer, attend.edit.exportfiltered %>" OnClick="ExportButton_OnClick"></asp:LinkButton>

            </div>
        </div>
    

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">
    
    
    


        <div class="row">
            <div class="col-lg-8">
                <h1>Participant filter</h1>
                <asp:Literal runat="server" ID="NumberOfParticipantsLiteral"></asp:Literal>

            </div>
            <div class="col-lg-2 pull-right">
                
                <div class="form-group pull-right">
                    <br/>
                    <asp:Literal runat="server" ID="PageResultLiteral2"></asp:Literal>
                    <div class="input-group">
                        <asp:Button runat="server" ID="Button1" CssClass="btn btn-default" OnClick="PagingPrevious_OnClick" Text="<%$ Resources: EPiServer, attend.edit.previous %>" />

                        <asp:Button runat="server" ID="Button2" CssClass="btn btn-default" OnClick="PagingNext_OnClick" Text="<%$ Resources: EPiServer, attend.edit.next %>" />
                    </div>

                </div>
            </div>


        </div>
        <table cellpadding="5" cellspacing="0" class="table table-striped table-hover">
            <tr>
                <td colspan="3">
                <div style="float: left;">
                        <asp:TextBox runat="server" ID="SearchTextBox" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                    </div>
                    <asp:LinkButton runat="server" CssClass="btn btn-default" Text="<%$ Resources: EPiServer, attend.edit.search %>" />
                    <asp:LinkButton runat="server" CssClass="btn btn-default" ID="RemoveFiltersLinkButton" OnClick="RemoveFiltersLinkButton_OnClick" Text="<%$ Resources: EPiServer, attend.edit.showall %>" />
                   

                </td><td colspan="1">
                    <asp:DropDownList runat="server" ID="StatusFilterDropDown" CssClass="form-control" AutoPostBack="True" /></td>
                <td>
                    <asp:DropDownList runat="server" ID="EMailDropDownList" CssClass="form-control" AutoPostBack="True" /></td>
                <td colspan="2">
                    <asp:DropDownList runat="server" ID="EventsDropDownList" CssClass="form-control" AutoPostBack="true"/>
                </td>
                
            </tr>
            <asp:Repeater runat="server" ID="ParticipantsRepeater">
                <ItemTemplate>
                    <tr>
                        <td><a href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>" class="btn btn-primary btn-xs">
                            <episerver:translate runat="server" text="/attend/edit/edit" />
                        </a></td>
                        <td>
                            <%#(Container.DataItem as IParticipant).Code %>
                        </td>
                        <td>
                            <%#(Container.DataItem as IParticipant).DateSubmitted.ToShortDateString() %>
                        </td>                        <td>
                            <%#(Container.DataItem as IParticipant).AttendStatus %>
                        </td>

                        <td>
                            <%#(Container.DataItem as IParticipant).Email %>
                        </td>
                        <td>
                            <%#(Container.DataItem as IParticipant).Price %>
                        </td>
                        <td>
                            <%#AttendRegistrationEngine.GetParticipantInfo((Container.DataItem as IParticipant), "eventname") %>
                        </td>

                        <asp:Repeater runat="server" DataSource="<%#GetFormFields(Container.DataItem as IParticipant) %>">
                            <ItemTemplate>
                                <td><%#Container.DataItem %></td>
                            </ItemTemplate>

                        </asp:Repeater>
                    </tr>

                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="row">

            <div class="col-lg-2 pull-right">

                <div class="form-group">
                    <span class="pull-left">
                    <asp:Literal runat="server" ID="PageResultLiteral"></asp:Literal>
                        </span>
                    <div class="input-group pull-left">
                        <div class="input-group-btn">
                            <asp:Button runat="server" ID="PagingPrevious" CssClass="btn btn-default" OnClick="PagingPrevious_OnClick" Text="<%$ Resources: EPiServer, attend.edit.previous %>" />
                        </div>
                        <asp:TextBox runat="server" ID="PagingPage" CssClass="form-control"></asp:TextBox>
                        <div class="input-group-btn">

                            <asp:Button runat="server" ID="PagingNext" CssClass="btn btn-default" OnClick="PagingNext_OnClick" Text="<%$ Resources: EPiServer, attend.edit.next %>" />
                        </div>
                    </div>
                    <asp:DropDownList runat="server" ID="PagingPrPaging" CssClass="form-control pull-left" AutoPostBack="true">
                        <asp:ListItem Value="150" Text="<%$ Resources: EPiServer, attend.edit.numberPrPage %>"></asp:ListItem>
                        <asp:ListItem Value="10">10</asp:ListItem>
                        <asp:ListItem Value="50">50</asp:ListItem>
                        <asp:ListItem Value="100">100</asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources: EPiServer, attend.edit.all %>"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>


        </div>

        <div style="clear: both;">
            <br />
        </div>
 
</asp:Content>
