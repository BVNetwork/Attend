<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettingsEdit.aspx.cs" Inherits="BVNetwork.Attend.Admin.SettingsEdit" EnableViewState="true" MasterPageFile="~/modules/BVNetwork.Attend/Admin/MasterPages/AttendAdminMaster.Master" %>

<%@ Import Namespace="BVNetwork.Attend.Business.Settings" %>

<asp:Content runat="server" ContentPlaceHolderID="MainArea">

    <h1>
        <episerver:translate text="/attend/admin/settings" runat="server" />
    </h1>
    <div class="">
        <div class="row">
            <div class="col-lg-12">
                <div class="well">
                    <h2>Provider Settings</h2>
                    <div id="ProviderDropDowns" visible="false" runat="server">
                        <div class="row">

                            <asp:Label AssociatedControlID="ParticipantProvidersDropDown" CssClass="col-sm-3" Text="Participant Provider:" runat="server"></asp:Label>
                            <div class="col-sm-5">
                                <asp:DropDownList runat="server" ID="ParticipantProvidersDropDown" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                Use Episerver Forms instead of XForms
                            </div>
                            <div class="col-sm-5">
                                <asp:CheckBox runat="server" ID="UseFormsCheckBox" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">

                                <asp:LinkButton runat="server" OnClick="SaveSettings_Click" CssClass="btn btn-primary" ID="SaveProviderButton">Save</asp:LinkButton>
                            </div>

                        </div>


                    </div>


                    <div id="ProviderOverview" visible="true" runat="server">
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <b>Current Participant Provider:</b>
                            </div>
                            <div class="col-sm-6">
                                <%=Settings.GetSetting("DefaultParticipantProviderString") %>
                            </div>
                            <asp:LinkButton runat="server" CssClass="btn btn-primary" OnClick="SearchProviders_Click">Search for providers</asp:LinkButton>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">

            <div class="well">
                <div class="row">
                    <div class="col-lg-6">
                        <h2>SMS Settings</h2>
                        <div class="input-group">
                            <asp:Label runat="server" AssociatedControlID="SMSUrlTextBox" Text="SMS URL" CssClass="control-label"></asp:Label>
                            <asp:TextBox ID="SMSUrlTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <asp:LinkButton runat="server" Text="Save settings" CssClass="btn btn-primary" OnClick="Save_OnClick"></asp:LinkButton>
                        </div>

                    </div>
                    <div class="col-lg-6">
                        <div class="well">
                            <h3>Test SMS</h3>
                            <div class="input-group">
                                <asp:Label runat="server" AssociatedControlID="TestToTextBox" Text="To" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="TestToTextBox" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group">
                                <asp:Label runat="server" AssociatedControlID="TestFromTextBox" Text="From" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="TestFromTextBox" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group">
                                <asp:Label runat="server" AssociatedControlID="TestMessageTextBox" Text="Message" CssClass="control-label"></asp:Label>
                                <asp:TextBox runat="server" ID="TestMessageTextBox" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group">
                                <asp:LinkButton runat="server" Text="Send SMS" CssClass="btn btn-primary" OnClick="Test_OnClick"></asp:LinkButton>
                            </div>
                            <asp:Literal runat="server" ID="ResultLiteral"></asp:Literal>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
