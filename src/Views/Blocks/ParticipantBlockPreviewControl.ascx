<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParticipantBlockPreviewControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.ParticipantBlockPreviewControl" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>
<%@ Register Namespace="EPiServer.XForms.WebControls" TagPrefix="XForms" %>

    <script type="text/javascript">
        $(function () {
            // for bootstrap 3 use 'shown.bs.tab', for bootstrap 2 use 'shown' in the next line
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                // save the latest tab; use cookies if you like 'em better:
                localStorage.setItem('lastTab', $(this).attr('href'));
            });

            // go to the latest tab, if it exists:
            var lastTab = localStorage.getItem('lastTab');
            if (lastTab) {
                $('[href="' + lastTab + '"]').tab('show');
            }
        });
    </script>
<br/>
<a target="_top" class="btn btn-default pull-right" href='<%=EventUrl %>'>
    <EPiServer:Translate runat="server" text="/attend/edit/returntoevent" />
</a>


<EPiServer:Translate runat="server" text="/attend/edit/participantat" />
<EPiServer:property runat="server" propertyname="pageName" pagelinkproperty="EventPage"></EPiServer:property><br />
<br />




<div style="font-weight: bold;">
    <ul class="nav nav-tabs" role="tablist">
        <li role="presentation" class="active"><a href="#edit" aria-controls="edit" role="tab" data-toggle="tab">
            <EPiServer:Translate runat="server" text="/attend/edit/editparticipantdetails" />
        </a></li>
        <li role="presentation"><a href="#form" aria-controls="form" role="tab" data-toggle="tab">
            <EPiServer:Translate runat="server" text="/attend/edit/editregistrationform" />
        </a></li>
        <li role="presentation"><a href="#sessions" aria-controls="sessions" role="tab" data-toggle="tab">
            <EPiServer:Translate runat="server" text="/attend/edit/editsessions" />
        </a></li>

        <li role="presentation"><a href="#log" aria-controls="log" role="tab" data-toggle="tab">
            <EPiServer:Translate runat="server" text="/attend/edit/log" />
        </a></li>

    </ul>
</div>



<div class="tab-content">
    <br />
    <div role="tabpanel" class="tab-pane fade in active" id="edit">

        <div class="row">
            <div class="col-lg-6">

                <div class="">
                    <h2>
                        <EPiServer:Translate runat="server" text="/attend/edit/editparticipantdetails" />
                    </h2>
                    <table class="table table-responsive table-hover table-striped">
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/email" />
                            </td>
                            <td>
                                <EPiServer:property runat="server" propertyname="Email" id="PropertyEmail"></EPiServer:property></td>
                        </tr>
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/code" />
                            </td>
                            <td>
                                <EPiServer:property runat="server" propertyname="Code" id="Code"></EPiServer:property></td>
                        </tr>
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/status" />
                            </td>
                            <td>
                               <%=CurrentData.AttendStatus %></td>
                        </tr>
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/username" />
                            </td>
                            <td>
                                <EPiServer:property runat="server" propertyname="Username" id="Property3"></EPiServer:property></td>
                        </tr>
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/datesubmitted" />
                            </td>
                            <td>
                                <EPiServer:property runat="server" propertyname="DateSubmitted" id="Property4"></EPiServer:property></td>
                        </tr>
                        <tr>
                            <td>
                                <EPiServer:Translate runat="server" text="/attend/edit/price" />
                            </td>
                            <td>
                                <EPiServer:property runat="server" propertyname="Price" id="Property5"></EPiServer:property></td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>
    </div>

    <div role="tabpanel" class="tab-pane" id="form">

        <div class="well">
            <h2>
                <EPiServer:Translate runat="server" text="/attend/edit/editregistrationform" />
            </h2>
            <XForms:XFormControl runat="server" id="DetailsXFormControl" />
            <br />
            <asp:LinkButton runat="server" OnClick="UpdateParticipant_Click" Text="<%$ Resources: EPiServer, attend.edit.saveformdata %>" CssClass="btn btn-primary primary" />
            &nbsp;
        <asp:LinkButton runat="server" OnClick="SendMail_Click" Text="<%$ Resources: EPiServer, attend.edit.saveandsendmail %>" CssClass="btn btn-primary " />
            <br />
        </div>
    </div>

    <div role="tabpanel" class="tab-pane" id="sessions">

        <div class="well">
            <h2>
                <EPiServer:Translate runat="server" text="/attend/edit/editsessions"></EPiServer:Translate></h2>
            <asp:PlaceHolder runat="server" ID="SessionList"></asp:PlaceHolder>
            <br />
            <br />
        </div>
    </div>



    <div role="tabpanel" class="tab-pane" id="log">
        <h2>
            <EPiServer:Translate runat="server" text="/attend/edit/log" />
        </h2>
        <%=GetLogText %>

        <a target="_top" class="btn btn-default" href='<%=EventUrl %>'>
            <EPiServer:Translate runat="server" text="/attend/edit/returntoevent" />
        </a>

    </div>
</div>
<br />
