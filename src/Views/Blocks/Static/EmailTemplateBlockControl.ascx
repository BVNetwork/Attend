<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateBlockControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.Static.EmailTemplateBlockControl" %>
<div style="background-color: #fff; padding: 10px; margin: 10px;">
    <span id="SendAsSMSProperty" runat="server" class="label label-primary">
        <%=CurrentBlock.SendAsSms ? "<span class='glyphicon glyphicon-phone'></span> " :  "<span class='glyphicon glyphicon-envelope'></span> " %>
        <episerver:translate runat="server" text="/attend/edit/sendAs" />
        <%=CurrentBlock.SendAsSms ? "SMS" :  "Mail" %>
    </span>
<br/><br/>
    <b>
        <episerver:translate runat="server" text="/attend/edit/from" />
    </b>

    <br />
    <episerver:property runat="server" propertyname="From" />
    <br />
    <b>
        <episerver:translate runat="server" text="/attend/edit/to" />
    </b>
    <br />
    <episerver:property editable="true" runat="server" propertyname="To" />
    <br />
    <asp:PlaceHolder runat="server" Visible='<%#!CurrentData.SendAsSms %>'>
        <b>
            <episerver:translate runat="server" text="/attend/edit/cc" />
        </b>
        <br />
        <episerver:property runat="server" propertyname="CC" />
        <br />
        <b>
            <episerver:translate runat="server" text="/attend/edit/bcc" />
        </b>
        <br />
        <episerver:property runat="server" propertyname="BCC" />
        <br />
        <b>
            <episerver:translate runat="server" text="/attend/edit/subject" />
        </b>
        <br />
        <episerver:property runat="server" propertyname="Subject" />
        <br />
        <b>
            <episerver:translate runat="server" text="/attend/edit/xhtmlbody" />
        </b>
        <br />
    </asp:PlaceHolder>
    <b>
        <episerver:translate runat="server" text="/attend/edit/plaintextbody" />
    </b>
    <br />
    <episerver:property runat="server" propertyname="MainTextBody" />
</div>
