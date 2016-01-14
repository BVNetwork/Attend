<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceList.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.InvoiceList" %>

<asp:Repeater runat="server" ID="EventsOverviewRepeater">
    <HeaderTemplate>
        <div class="thinborder">
            <table>
    </HeaderTemplate>
    <ItemTemplate>


    </ItemTemplate>
    <FooterTemplate>
        <tr>
            </table>

    </FooterTemplate>
</asp:Repeater>


<asp:Repeater runat="server" ID="ParticipantsRepeater" OnItemDataBound="previewRepeater_ItemDataBound">
    <HeaderTemplate>
        <div>
            <table class="table table-bordered table-hover table-responsive table-striped table-condensed">
               
    </HeaderTemplate>

    <ItemTemplate>
         <%#Income((Container.DataItem as BVNetwork.Attend.Models.Blocks.ParticipantBlock).Price) %>
        <tr>
            <asp:Repeater runat="server" ID="InvoiceDetailsRepeater" DataSource='<%#GetInvoiceValues(Container.DataItem as BVNetwork.Attend.Models.Blocks.ParticipantBlock) %>'>
                <ItemTemplate>
                    <td><%#Container.DataItem.ToString() %></td>
                </ItemTemplate>
            </asp:Repeater>

        </tr>

    </ItemTemplate>

    <FooterTemplate>
        </table>
                    <EPiServer:Translate Text="/attend/admin/totalparticipants" runat="server"/>: <%#TotalCount %>

            <EPiServer:Translate Text="/attend/admin/totalincome" runat="server"/>: <%#TotalIncome %>
        </div>

    </FooterTemplate>
</asp:Repeater>
