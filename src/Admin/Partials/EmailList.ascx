<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailList.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.EmailList" %>
<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Participant" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Pages" %>

<div class="row">
    <div class="col-lg-12">

        <asp:Repeater runat="server" ID="ScheduledEmailsRepeater">
            <HeaderTemplate>
                <table class="table table-hover table-striped table-responsive">
                    <tr>

                        <th>Name / Subject / E-mail</th>
                        <th>Status filter</th>
                        <th>Subject</th>
                        <th>Scheduled date</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:PlaceHolder runat="server" Visible="<%#(GetScheduledEmailBlocks(Container.DataItem as EventPageBase).Count() > 0) %>">
                    <tr class="info">

                        <td colspan="3">
                            <strong><span class="glyphicon glyphicon-file"></span>&nbsp;<%#(Container.DataItem as EventPageBase).PageName %></strong></td>
                        <td><a class="btn btn-default btn-sm" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                            <episerver:translate runat="server" text="/attend/edit/edit"></episerver:translate>
                        </a></td>

                    </tr>
                    <asp:Repeater runat="server" DataSource="<%#GetScheduledEmailBlocks(Container.DataItem as EventPageBase) %>">
                        <ItemTemplate>
                            <tr class="active">

                                <td><strong><%#(Container.DataItem as ScheduledEmailBlock).GetEmailTemplateBlock().Subject %></strong></td>
                                <td><span class="glyphicon glyphicon-time"></span>&nbsp;<%#AttendScheduledEmailEngine.GetSendDate(Container.DataItem as ScheduledEmailBlock, ((RepeaterItem)Container.Parent.Parent.Parent).DataItem as EventPageBase).ToShortDateString() %></td>
                                <td><%#(Container.DataItem as ScheduledEmailBlock).ScheduledText() %></td>
                                <td><a class="btn btn-default btn-sm" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                                    <episerver:translate runat="server" text="/attend/edit/edit"></episerver:translate>
                                </a></td>
                            </tr>
                            <tr>
                                <asp:Repeater runat="server" ID="ParticipantsRepeater" DataSource='<%#AttendScheduledEmailEngine.GetParticipantsForScheduledEmail(Container.DataItem as ScheduledEmailBlock)%>'>
                                    <ItemTemplate>
                                        <tr>

                                            <td><span class="glyphicon glyphicon-envelope"></span>&nbsp;<%#(Container.DataItem as IParticipant).Email %></td>
                                            <td><%#(Container.DataItem as IParticipant).Code %></td>
                                            <td><%#(Container.DataItem as IParticipant).AttendStatusText %></td>
                                            <td><a class="btn btn-default btn-sm" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                                                <episerver:translate runat="server" text="/attend/edit/edit"></episerver:translate>
                                            </a></td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                </asp:PlaceHolder>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

    </div>
</div>
