<%@ Page Language="C#" Inherits="BVNetwork.Attend.Views.Blocks.EMailTemplatePreview" EnableViewState="false" MasterPageFile="~/modules/BVNetwork.Attend/Views/MasterPages/Attend.Master" %>

<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="container">
        <h2>
            <episerver:translate runat="server" text="/attend/edit/emailtemplates" />
        </h2>

        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-12">
                <div class="well">
                    <episerver:property id="propertyControl" cssclass="row preview" runat="server">
                                <rendersettings enableeditfeaturesforchildren="true" />
                            </episerver:property>
                </div>
            </div>
        </div>


    </div>
</asp:Content>



<asp:Content runat="server" ContentPlaceHolderID="HeaderArea">
    <%=CurrentData.Name %>
</asp:Content>
