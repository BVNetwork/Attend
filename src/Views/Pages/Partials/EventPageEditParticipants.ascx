<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EventPageEditParticipants.ascx.cs" Inherits="BVNetwork.Attend.Views.Pages.Partials.EventPageEditParticipants" %>
<%@ Register Src="/modules/bvnetwork.attend/Views/Pages/Partials/EventPageEditParticipantsParticipant.ascx" TagPrefix="Attend" TagName="Participant" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Participant" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>


<asp:PlaceHolder runat="server" ID="ParticipantsListPlaceHolder">
    <div class="container">
        <div class="">
            <asp:PlaceHolder runat="server" ID="NoParticipants">
                <EPiServer:translate runat="server" text="/attend/edit/noparticipants" />
            </asp:PlaceHolder>


            <asp:PlaceHolder runat="server" ID="CopyMovePlaceHolder">

                <div class="btn-toolbar">
                    <div class="btn-group">

                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myModal">
                            <EPiServer:translate runat="server" text="/attend/edit/numberselected"></EPiServer:translate>
                            <%=AttendRegistrationEngine.GetOrCreateParticipantsClipboard().Count %>
                        </button>
                        <asp:LinkButton runat="server" ID="PasteParticipantsExport" Text="&nbsp;<span class='glyphicon glyphicon-save'></span>" CssClass="btn btn-default" ToolTip="<%$ Resources: EPiServer, attend.edit.export %>" OnClick="PasteParticipantsExport_OnClick"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="RemoveSelections" Text="&nbsp;<span class='glyphicon glyphicon-remove'></span>" CssClass="btn btn-default" ToolTip="<%$ Resources: EPiServer, attend.edit.deselectall %>" OnClick="PasteParticipantsRemove_OnClick"></asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton runat="server" ID="PasteParticipantsMove" CssClass="btn btn-default" Text="&nbsp;<span class='glyphicon glyphicon-paste'></span>" ToolTip="<%$ Resources: EPiServer, attend.edit.movehere %>" OnClick="PasteParticipantsMove_OnClick"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="PasteParticipantsCopy" CssClass="btn btn-default" Text="&nbsp;<span class='glyphicon glyphicon-duplicate'></span>" ToolTip="<%$ Resources: EPiServer, attend.edit.copyhere %>" OnClick="PasteParticipantsCopy_OnClick"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="DeleteParticipantsCopy" CssClass="btn btn-default" Text="&nbsp;<span class='glyphicon glyphicon-trash'></span>" ToolTip="<%$ Resources: EPiServer, attend.edit.permanentlydelete %>" OnClick="DeleteParticipantsCopy_OnClick"></asp:LinkButton>
                    </div>
                    <%--                    <div class="btn-group">
                                                        <asp:LinkButton runat="server" ID="SendNotificationSelected" CssClass="btn btn-primary" Text="<%$ Resources: EPiServer, attend.edit.sendnotificationtoselected %>" OnClick="SendNotificationSelected_OnClick"></asp:LinkButton>

                    </div>--%>
                    <div class="col-lg-4 pull-right">
                        <div class="btn-group">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <asp:CheckBox runat="server" ID="StatusMailCheckBox" CssClass="" ToolTip="<%$ Resources: EPiServer, attend.edit.changestatus %>" />
                                    <span class='glyphicon glyphicon-envelope'></span>
                                </div>
                                <asp:DropDownList runat="server" CssClass="form-control" ID="StatusDropDown"></asp:DropDownList>

                                <span class="input-group-btn">
                                    <asp:LinkButton runat="server" ID="ChangeStatus" CssClass="btn btn-primary" OnClick="ChangeStatus_Click" Text="&nbsp;<span class='glyphicon glyphicon-floppy-disk'></span>">
                                    </asp:LinkButton></span>
                            </div>
                        </div>

                    </div>

                </div>

                <div class="modal fade" id="myModal" tabindex="-1000" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">X</span></button>
                                <h4 class="modal-title" id="myModalLabel">Selected</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Repeater runat="server" ID="SelectedRepeater">
                                    <ItemTemplate>
                                        <%#(Container.DataItem as IParticipant).Email %><br />
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>

            <asp:Literal runat="server" ID="NumberOfParticipantsLiteral"></asp:Literal>

            <table class="table table-striped table-hover table-condensed">
                <tr>
                    <td>
                        <asp:LinkButton runat="server" ID="CheckAll" OnClick="CheckAll_OnClick" CssClass="btn btn-default btn-sm"><span class='glyphicon glyphicon-check'></span></asp:LinkButton>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="StatusFilterDropDown" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="StatusFilterDropDown_OnSelectedIndexChanged" /></td>
                    <td>
                        <asp:DropDownList runat="server" ID="EMailDropDownList" CssClass="form-control" AutoPostBack="True" /></td>
                    <td>
                        <asp:DropDownList runat="server" ID="SessionDropDownList" CssClass="form-control" AutoPostBack="True" /></td>
                    <td colspan='4'>
                        <div class="form-horizontal">
                            <div class="input-group">

                                <asp:TextBox runat="server" ID="SearchTextBox" CssClass="form-control" AutoPostBack="True"></asp:TextBox>

                                <span class="input-group-btn">
                                    <asp:LinkButton runat="server" CssClass="btn btn-default" Text="<%$ Resources: EPiServer, attend.edit.search %>" />
                                </span>
                            </div>
                        </div>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" CssClass="btn btn-default pull-left" ID="RemoveFiltersLinkButton" OnClick="RemoveFiltersLinkButton_OnClick" Text="<%$ Resources: EPiServer, attend.edit.showall %>" />

                    </td>
                    <td colspan="4">
                            <div id="help-button" class="btn btn-primary btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("participantslist")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("participantslist") %>">
                                <span class="glyphicon glyphicon-info-sign"></span>
                            </div>


                    </td>
                </tr>
                <asp:Repeater runat="server" ID="ParticipantsRepeater">
                    <ItemTemplate>
                        <Attend:Participant runat="server" CurrentData='<%#Container.DataItem as IParticipant %>'></Attend:Participant>

                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <%--                       <EventX:EventEditorList runat="server" id="ParticipantsList" EventPageBaseId="<%#CurrentData.ContentLink.ID %>" />
            --%>


            <div class="col-lg-12">
                <div class="row">
                    <div class="col-lg-12">
                        <asp:Literal runat="server" ID="PageResultLiteral"></asp:Literal>

                    </div>

                    <div class="col-lg-2">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="PagingPrevious" CssClass="btn btn-default" OnClick="PagingPrevious_OnClick">
                            <EPiServer:Translate runat="server" text="/attend/edit/previous" />
                                        </asp:LinkButton>
                                    </div>
                                    <asp:TextBox runat="server" ID="PagingPage" CssClass="form-control"></asp:TextBox>

                                    <div class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="PagingNext" CssClass="btn btn-default" OnClick="PagingNext_OnClick"><EPiServer:Translate runat="server" text="/attend/edit/next" /></asp:LinkButton>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-lg-2">
                        <asp:DropDownList runat="server" ID="PagingPrPaging" CssClass="form-control" AutoPostBack="true">
                            <asp:ListItem Value="150" Text="<%$ Resources: EPiServer, attend.edit.numberPrPage %>"></asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                            <asp:ListItem Value="0" Text="<%$ Resources: EPiServer, attend.edit.all %>"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                    <div class="col-lg-2">

                        <asp:LinkButton runat="server" CssClass="btn btn-default " ID="ExportButton" Text="<%$ Resources: EPiServer, attend.edit.exportfiltered %>" OnClick="ExportButton_OnClick"></asp:LinkButton>

                    </div>

                </div>

            </div>



            <%-- 
                        <div class="well">
                            <div class="left">
                                <asp:DropDownList runat="server" ID="StatusDropDown"></asp:DropDownList>
                            </div>
                            <div class="left">
                                <asp:CheckBox runat="server" ID="StatusMailCheckBox" translate="/attend/edit/changestatus" />
                                <EPiServer:Translate runat="server" text="/attend/edit/sendstatusmail" />
                            </div>
                            <div class="left">

                                <asp:LinkButton runat="server" ID="ChangeStatus" CssClass="button iconTag paddingtop" OnClick="ChangeStatus_Click" translate="/attend/edit/changestatus">
                 
                                </asp:LinkButton>
                            </div>
                        </div>
            --%>
        </div>
        <br />
        <div class="row">


            <div class="col-lg-6">
                <div class="well">
                    <div class="form-group">
                        <div class="input-group">

                            <asp:Label AssociatedControlID="EMailTextBox" translate="/attend/edit/email" CssClass="input-group-addon" runat="server"></asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="EMailTextBox"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton runat="server" OnClick="CreateParticipant_Click" translate="/attend/edit/createnew" CssClass="btn btn-primary" /></span>
                        </div>
                    </div>
                    <asp:PlaceHolder runat="server" ID="EmailPlaceHolder" Visible="false">
                        <EPiServer:translate runat="server" text="/attend/edit/musthaveemail" />
                    </asp:PlaceHolder>
                </div>

            </div>

        </div>
    </div>
</asp:PlaceHolder>


