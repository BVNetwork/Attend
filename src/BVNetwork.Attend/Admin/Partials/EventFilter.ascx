<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventFilter.ascx.cs" Inherits="BVNetwork.Attend.Admin.Partials.EventFilter" %>
<asp:TextBox runat="server" ID="DateStart"></asp:TextBox>
&nbsp;
            <asp:TextBox runat="server" ID="DateEnd"></asp:TextBox>
<asp:Button runat="server" ID="GetInvoiceData" OnClick="GetInvoiceData_Click" Text="Vis fakturagrunnlag" />
<br />
<div style="font-size: 12px;">
    Vis:
    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="InvoicesLastMonth_Click" Text="Forrige måned"></asp:LinkButton>
    |
    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="InvoicesThisMonth_Click" Text="Denne måned"></asp:LinkButton>
    |
    <asp:LinkButton ID="LinkButton3" runat="server" OnClick="InvoicesNextMonth_Click" Text="Neste måned"></asp:LinkButton>
    |
    <asp:LinkButton runat="server" OnClick="InvoicesFuture_Click" Text="Alle fremtidige kurs"></asp:LinkButton>
</div>
