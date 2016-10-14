<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParticipantBlockListViewSmallControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.ParticipantBlockListViewSmallControl" %>

<tr>
    <td>
        <a class="color-<%=CurrentData.AttendStatus%>" target="_top" href='<%=EPiServer.Editor.PageEditing.GetEditUrl((CurrentBlock as EPiServer.Core.IContent).ContentLink) %>'>
            <%=CurrentData.AttendStatusText%>
        </a>
    </td>
    <td><a class="btn btn-default btn-sm" target="_top" href='<%=EPiServer.Editor.PageEditing.GetEditUrl((CurrentBlock as EPiServer.Core.IContent).ContentLink) %>'><EPiServer:Translate runat="server" text="/attend/edit/edit" /></a></td>


    <td>
        <%=CurrentData.Price%>
    </td>

    <td>
        <div class="">
            <%=CurrentData.Code%>
        </div>
    </td>

    <td>
        <episerver:property runat="server" propertyname="Email" id="PropertyEmail"></episerver:property>
    </td>

    <asp:Repeater runat="server" ID="FormFieldsRepeater">
        <ItemTemplate>
                <%#GetFormControlValue((Container.DataItem).ToString()) %>
        </ItemTemplate>
    </asp:Repeater>

</tr>
