<%@ Page Language="c#" Inherits="BVNetwork.Attend.Views.Blocks.ScheduledEmailBlockPreview" CodeBehind="ScheduledEmailBlockPreview.aspx.cs" MasterPageFile="~/modules/BVNetwork.Attend/Views/MasterPages/Attend.Master" %>

<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Email" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderArea">
    <%=CurrentData.Name %>
</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="MainContent">
<EPiServer:FullRefreshPropertiesMetaData runat="server"/>
    <div class="container">

        <div class="row">
            <div class="col-lg-12">
                <a target="_top" style="margin-top: 20px;" class="btn btn-default pull-right" href='<%=EventUrl %>'>
                    <EPiServer:Translate runat="server" text="/attend/edit/returntoevent" />
                </a>
                <h1 id="BlockName" runat="server"><%=(CurrentData as EPiServer.Core.IContent).Name %></h1>

            </div>

            <div class="col-lg-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            <EPiServer:Translate runat="server" text="/attend/edit/emailtemplates" />
                            <div id="help-button" class="btn btn-primary btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("emailtemplates")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("emailtemplates") %>">
                                <span class="glyphicon glyphicon-info-sign"></span>
                            </div>
                        </h3>
                    </div>
                    <div class="panel-body">

                        <EPiServer:Property runat="server" PropertyName="EmailTemplate" Visible='<%#((CurrentData as BVNetwork.Attend.Models.Blocks.ScheduledEmailBlock).EmailTemplateContentReference == null)  %>' id="ConfirmMailTemplate" />
                        <div class="">
                            <EPiServer:Translate runat="server" text="/attend/edit/selectglobaltemplate" />
                            <EPiServer:Property runat="server" PropertyName="EmailTemplateContentReference" id="EmailTemplateContentReference" />
                            <asp:PlaceHolder runat="server" ID="ConfirmMailTemplateBlockPreviewPlaceHolder">
                                <EPiServer:Property runat="server" ID="MailTemplateBlockPreview"></EPiServer:Property>
                                <asp:LinkButton runat="server" ID="ConvertLocalConfirmBlock" Text="<%$ Resources: EPiServer, attend.edit.convertlocal %>" CssClass="btn btn-primary" OnClick="ConvertLocalConfirmBlock_OnClick"></asp:LinkButton>
                                &nbsp;
                            <asp:LinkButton runat="server" ID="EditConfirmMailTemplate" Text="<%$ Resources: EPiServer, attend.edit.editmailtemplate %>" CssClass="btn btn-primary" OnClick="EditConfirmMailTemplate_OnClick"></asp:LinkButton>
                            </asp:PlaceHolder>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-lg-6">

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            <EPiServer:Translate runat="server" text="/attend/edit/scheduling" />
                            <div id="help-button" class="btn btn-primary btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("emailschedule")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("emailschedule") %>">
                                <span class="glyphicon glyphicon-info-sign"></span>
                            </div>

                        </h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-lg-12">
                            <div class="row">
                                <table class="table table-responsive">
                                    <tr>
                                        <td class="active">
                                            <EPiServer:Translate runat="server" text="/attend/edit/oneWhatTriggers" />
                                            <div id="help-button" class="btn btn-default btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("emailtriggers")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("emailtriggers") %>">
                                                <span class="glyphicon glyphicon-info-sign"></span>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="SendOptionsControl" runat="server"><%=Enum.Parse(typeof(SendOptions), (CurrentData as ScheduledEmailBlock).EmailSendOptions.ToString()).ToString() %></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="active">
                                            <EPiServer:Translate runat="server" text="/attend/edit/twoWhenShouldBeSent" />
                                            <div id="help-button" class="btn btn-default btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("emailwhen")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("emailwhen") %>">
                                                <span class="glyphicon glyphicon-info-sign"></span>
                                            </div>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>

                                            <asp:PlaceHolder runat="server" Visible='<%#(CurrentData as ScheduledEmailBlock).EmailSendOptions == SendOptions.Specific %>'>Date scheduled:<br />
                                                <EPiServer:Property propertyname="SpecificDateScheduled" runat="server"></EPiServer:Property>
                                            </asp:PlaceHolder>


                                            <asp:PlaceHolder runat="server" Visible='<%#(CurrentData as ScheduledEmailBlock).EmailSendOptions == SendOptions.Relative %>'>
                                                <br />
                                                <ul class="nav nav-pills">
                                                    <li>&nbsp;<div id="ScheduledRelativeAmountControl" runat="server" class="label label-default"><%#(CurrentData as ScheduledEmailBlock).ScheduledRelativeAmount.ToString() %></div>
                                                        &nbsp;<span class="badge"></span></li>
                                                    <li>&nbsp;
                                    <div id="ScheduledRelativeUnitControl" runat="server" class="label label-default"><%#(CurrentData as ScheduledEmailBlock).ScheduledRelativeUnit.ToString() %></div>
                                                        &nbsp; <span class="badge"></span></li>
                                                    <li>&nbsp;<div id="ScheduledRelativeToControl" runat="server" class="label label-default"><%#(CurrentData as ScheduledEmailBlock).ScheduledRelativeTo.ToString() %></div>
                                                        &nbsp; <span class="badge"></span></li>
                                                </ul>
                                            </asp:PlaceHolder>


                                            <asp:PlaceHolder runat="server" Visible='<%#(CurrentData as ScheduledEmailBlock).EmailSendOptions == SendOptions.Action %>'>
                                                <div id="SendOnStatusControl" runat="server">E-mail trigger: <%#(CurrentData as ScheduledEmailBlock).SendOnStatus.ToString() %></div>
                                            </asp:PlaceHolder>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="active">
                                            <EPiServer:Translate runat="server" text="/attend/edit/threeStatusFilter" />
                                            <div id="help-button" class="btn btn-default btn-xs pull-right helpbutton" data-helptitle="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpTitle("emailfilter")%>" data-helptext="<%=BVNetwork.Attend.Business.Localization.LanguageHelper.GetHelpText("emailfilter") %>">
                                                <span class="glyphicon glyphicon-info-sign"></span>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <EPiServer:Property PropertyName="AttendStatusFilter" runat="server"></EPiServer:Property>
                                        </td>
                                    </tr>


                                </table>

                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <br />
            <asp:PlaceHolder runat="server" ID="ResetDatePlaceHolder" Visible='<%#((this.CurrentData as ScheduledEmailBlock).DateSent != null && (this.CurrentData as ScheduledEmailBlock).DateSent > DateTime.Now.AddYears(-100)) %>'>
                <div class="well">
                    <b>E-mail is sent - click button to reschedule this e-mail.</b>
                    <asp:LinkButton runat="server" ID="Reset" Text="Reset send date" CssClass="btn btn-default" OnClick="Reset_OnClick" />
                </div>
            </asp:PlaceHolder>

        </div>
    </div>

    </div>

    </div>
    </div>
</asp:Content>
