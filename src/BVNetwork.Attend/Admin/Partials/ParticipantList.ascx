<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParticipantList.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.ParticipantList" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Pages" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<script type="text/javascript" src="/Modules/BVNetwork.Attend/Static/js/jquery-2.1.1.min.js"></script>
<script type="text/javascript" src="/Modules/BVNetwork.Attend/Static/js/bootstrap.min.js"></script>
<asp:Repeater runat="server" ID="EventsOverviewRepeater">
    <HeaderTemplate>
        <div class="well">
            <table class="table table-striped table-hover">
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <EPiServer:Translate runat="server" text="/attend/edit/sessions" />
                    </td>
                    <td style="text-align: right;">
                        <EPiServer:translate runat="server" text="/attend/admin/numberofparticipants"></EPiServer:translate>
                    </td>
                    <td style="text-align: right;">
                        <EPiServer:translate runat="server" text="/attend/admin/numberofavailableseats"></EPiServer:translate>
                    </td>
                    <td style="text-align: right;">
                        <EPiServer:translate runat="server" text="/attend/admin/numberofseats"></EPiServer:translate>
                    </td>
                    <td style="text-align: right;">
                        <EPiServer:translate runat="server" text="/attend/admin/totalincome"></EPiServer:translate>
                    </td>
                    <td colspan="2"></td>
                </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td width="200">
                <div class="progress" style="background-color: #ccc; margin-top: 5px;">
                    <%#GetProgressBar(Container.DataItem as EventPageBase) %>
                </div>
            </td>
            <td>

                <button type="button" class="btn btn-default btn-sm btn-block" data-toggle="modal" data-target="#modal-<%#(Container.DataItem as EPiServer.Core.PageData).PageLink.ID %>">
                    <span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;<EPiServer:Translate runat="server" text="/attend/edit/participants"></EPiServer:Translate>
                </button>

            </td>
            <td><%#(Container.DataItem as EPiServer.Core.PageData).PageName %></td>
            <td><%#((Container.DataItem as EventPageBase).EventDetails.EventStart).ToShortDateString()+" "+((Container.DataItem as EventPageBase).EventDetails.EventStart).ToShortTimeString() %></td>
            <td><%#((Container.DataItem as EventPageBase).EventDetails.EventEnd.Subtract((Container.DataItem as EventPageBase).EventDetails.EventStart).Days+1) %></td>

            <td style="text-align: right;"><%#CalculateParticipants((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>
            <td style="text-align: right;"><%#BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetAvailableSeats((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>
            <td style="text-align: right;"><%#BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfSeats((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>
            <td style="text-align: right;"><%#CalculateIncome((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>

            <td>
                <asp:LinkButton CssClass="btn btn-default btn-sm" runat="server" ID="ExportParticipantsButton" CommandArgument="<%#(Container.DataItem as EPiServer.Core.PageData).PageLink.ID %>" OnCommand="ExportParticipantsButton_OnClick"><span class="glyphicon glyphicon-export"></span> <episerver:Translate runat="server" text="/attend/admin/export"></episerver:Translate></asp:LinkButton>
            </td>
            <td><a class="btn btn-default btn-sm" href="/modules/bvnetwork.attend/admin/registerparticipants.aspx?eventid=<%#(Container.DataItem as EPiServer.Core.PageData).PageLink.ID %>"><span class="glyphicon glyphicon-ok"></span>  <EPiServer:translate runat="server" text="/attend/admin/registerparticipant"></EPiServer:translate></td>
            <td><a class="btn btn-default btn-sm" href="<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>">
                <span class="glyphicon glyphicon-pencil"></span> <EPiServer:translate runat="server" text="/attend/edit/edit"></EPiServer:translate>
            </a></td>

        </tr>

    </ItemTemplate>
    <FooterTemplate>
        <tr>
            <td colspan="5"></td>
            <td style="text-align: right;"><%#TotalCount %></td>
            <td></td>
            <td></td>
            <td style="text-align: right;"><%#TotalIncome %></td>
            <td colspan="2"></td>
        </tr>
        </table></div>
    </FooterTemplate>
</asp:Repeater>


<asp:Repeater runat="server" ID="EventsDetailsRepeater" OnItemDataBound="previewRepeater_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>

        <div class="modal fade" id="modal-<%#(Container.DataItem as EPiServer.Core.PageData).PageLink.ID %>" tabindex="-1000" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">X</span></button>
                        <h4 class="modal-title" id="myModalLabel">Participants</h4>
                    </div>
                    <div class="modal-body">

                        <a name="<%#(Container.DataItem as EPiServer.Core.PageData).PageLink.ID %>"></a>
                        <div class="well">

                            <h3><%#(Container.DataItem as EPiServer.Core.PageData).PageName %></h3>

                            <div class="">
                                <div class="right">
                                    <a class="btn btn-primary pull-right" target="_top" href='<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>'>
                                        <EPiServer:translate text="/attend/edit/edit" runat="server" />
                                    </a></td>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <EPiServer:translate text="/attend/admin/eventname" runat="server" />
                                            :</td>
                                        <td>
                                            <EPiServer:property propertyname="PageLink" runat="server"></EPiServer:property>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <EPiServer:translate runat="server" text="/attend/admin/numberofparticipants"></EPiServer:translate>
                                        </td>
                                        <td><%#BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfParticipants((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <EPiServer:translate runat="server" text="/attend/admin/numberofseats"></EPiServer:translate>
                                        </td>
                                        <td><%#BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfSeats((Container.DataItem as EPiServer.Core.PageData).PageLink) %></td>
                                    </tr>


                                    <br />
                                </table>

                            </div>
                            <br />

                            <table class="table table-striped table-hover">
                                <asp:Repeater runat="server" ID="Participants">
                                    <ItemTemplate>

                                        <tr>
                                            <td>
                                                <%#(Container.DataItem as ParticipantBlock).AttendStatusText%>
                                            </td>



                                            <td>
                                                <%#(Container.DataItem as ParticipantBlock).Price%>
                                            </td>

                                            <td>
                                                <div class="">
                                                    <%#(Container.DataItem as ParticipantBlock).Code%>
                                                </div>
                                            </td>

                                            <td>
                                                <%#(Container.DataItem as ParticipantBlock).Email%>
                                
                                            </td>
                                            <td><a class="btn btn-default btn-sm" target="_top" href='<%#EPiServer.Editor.PageEditing.GetEditUrl((Container.DataItem as EPiServer.Core.IContent).ContentLink) %>'>
                                                <EPiServer:Translate runat="server" text="/attend/edit/edit" />
                                            </a></td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>




                        </div>
                        <br />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>
