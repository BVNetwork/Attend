<%@ Page Language="C#" Inherits="BVNetwork.Attend.Views.Blocks.EMailTemplatePreview" EnableViewState="false" %>

<%@ Import Namespace="BVNetwork.Attend.Business.API" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" style="margin: 0; padding: 0;">
<head id="Head1" runat="server">
    <title>
        <EPiServer:Translate runat="server" text="/attend/edit/eventblockpreview" />
    </title>
    <link rel="Stylesheet" type="text/css" href="/Modules/BVNetwork.Attend/Static/AttendStyle.css" />
    <link rel="Stylesheet" type="text/css" href="/Modules/BVNetwork.Attend/Static/css/bootstrap.css" />
    <script type="text/javascript" src="/Modules/BVNetwork.Attend/Static/js/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="/Modules/BVNetwork.Attend/Static/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="Form1" runat="server" style="background-color: #fff; margin-top: -22px;">
        <div class="">
            <EPiServer:FullRefreshPropertiesMetaData runat="server" />

            <div class="darkarea">
                <div class="container">
                    <div class="left">
                        <img src="/Modules/BVNetwork.Attend/static/attend_blue.png" />
                    </div>
                    <div class="left">
                        <h1>
                            <EPiServer:Translate runat="server" text="/attend/edit/attend" />
                            <br />
                            <%=CurrentData.Name %></h1>
                    </div>


                </div>
            </div>


            <div class="container">
                <h2>
                    <EPiServer:Translate runat="server" text="/attend/edit/emailtemplates" />
                </h2>
                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-12">
                        <div class="well">


                            <EPiServer:Property ID="propertyControl" CssClass="row preview" runat="server">
                                <rendersettings enableeditfeaturesforchildren="true" />
                            </EPiServer:Property>
                        </div>
                    </div>
                </div>
            </div>
    </form>
</body>
</html>

