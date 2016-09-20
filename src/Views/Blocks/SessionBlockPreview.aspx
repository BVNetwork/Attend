<%@ Page Language="C#" Inherits="BVNetwork.Attend.Views.Blocks.SessionBlockPreview" MasterPageFile="~/modules/BVNetwork.Attend/Views/MasterPages/Attend.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">

    <div class="container">
        <episerver:property id="propertyControl" cssclass="row preview" runat="server">
            <rendersettings enableeditfeaturesforchildren="true" />
        </episerver:property>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="HeaderArea">
    <%=CurrentData.Name %>
</asp:Content>

