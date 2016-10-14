<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailList.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.EmailList" %>
<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Participant" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Pages" %>

<div class="row">
    <div class="col-lg-12">
        <asp:PlaceHolder runat="server" Visible=<%#(GetScheduledEmailBlocks().Count() == 0) %>>
            <div class="well">
                <h4><episerver:Translate runat="server" text="/attend/admin/noMessages"></episerver:Translate></h4>
            </div>
        </asp:PlaceHolder>
            <asp:Repeater runat="server" Visible='<%#GetScheduledEmailBlocks().Count() > 0 %>' DataSource="<%#GetScheduledEmailBlocks() %>">
                <HeaderTemplate>
                </HeaderTemplate>

                <ItemTemplate>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title"><strong><span class="<%#((Container.DataItem as ScheduledEmailBlock).GetEmailTemplateBlock().SendAsSms ? "glyphicon glyphicon-phone" : "glyphicon glyphicon-envelope")%>"></span>&nbsp; &nbsp;<%#(Container.DataItem as ScheduledEmailBlock).SendDateTime.ToShortDateString() %> <%#(Container.DataItem as ScheduledEmailBlock).SendDateTime.ToShortTimeString() %>&nbsp; (<%#(Container.DataItem as ScheduledEmailBlock).ScheduledText() %>)</strong>
                            <span class="pull-right"><%#AttendScheduledEmailEngine.GetStatus(Container.DataItem as ScheduledEmailBlock) %></span>
                            </h3>
                        </div>
                        <div class="panel-body">
                            <div class="well">
                                <strong><%#GetPageName(Container.DataItem as ScheduledEmailBlock) %><br /></strong>
                                <%#(Container.DataItem as ScheduledEmailBlock).GetEmailTemplateBlock().Subject ?? ((Container.DataItem as ScheduledEmailBlock).GetEmailTemplateBlock().SendAsSms ? "SMS" : "E-mail") %>
                                <a class="btn btn-default btn-sm pull-right" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                                    <episerver:translate runat="server" text="/attend/edit/edit"></episerver:translate>
                                </a>
                            </div>
                            <asp:Repeater runat="server" ID="ParticipantsRepeater" DataSource='<%#AttendScheduledEmailEngine.GetParticipantsForScheduledEmail(Container.DataItem as ScheduledEmailBlock)%>'>
                                <HeaderTemplate>
                                    <h3><episerver:translate runat="server" text="/Attend/admin/participants"></episerver:translate></h3>
                                    <table class="table table-hover table-striped table-responsive">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#(Container.DataItem as IParticipant).Email %></td>
                                        <td><%#(Container.DataItem as IParticipant).Code %></td>
                                        <td><%#(Container.DataItem as IParticipant).AttendStatusText %></td>
                                        <td><a class="btn btn-default btn-sm" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                                            <episerver:translate runat="server" text="/attend/edit/edit"></episerver:translate>
                                        </a></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
    </div>
</div>
