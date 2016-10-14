<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventPagePartial.ascx.cs" Inherits="BVNetwork.Attend.Views.Pages.Partials.EventPagePartial" %>

<episerver:Property runat="server" id="ContentProperty"></episerver:Property>

<asp:PlaceHolder runat="server" ID="SessionsPanel">
</asp:PlaceHolder>

<xforms:xformcontrol runat="server" id="DetailsXFormControl" />



<asp:HiddenField runat="server" ID="HiddenCode" />
<asp:HiddenField runat="server" ID="HiddenEmail" />
<br />
<asp:LinkButton runat="server" ID="AttendButton" OnClick="AttendButton_Click" Text="<%$ Resources: EPiServer, attend.edit.register %>" CssClass="btn attendButton" />
