<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SessionBlockListViewControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.SessionBlockListViewControl" %>
<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Text" %>

<tr>
    <td>
        <div style="margin: 0px 0;"><%=NumberOfParticipants %></div>
    </td>
    <td>
        <%=(CurrentBlock as EPiServer.Core.IContent).Name %>
    </td>

    <td>
        <%=CurrentData.Start %>
    </td>

    <td>
        <div class="">
            <%=CurrentData.End %>
        </div>
    </td>

    <td><a class="btn btn-default btn-sm" target="_top" href='<%=EPiServer.Editor.PageEditing.GetEditUrl((CurrentBlock as EPiServer.Core.IContent).ContentLink) %>'>
        <episerver:translate runat="server" text="/attend/edit/edit" />
    </a></td>

</tr>
