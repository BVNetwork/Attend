<%@ Page Language="C#" Inherits="BVNetwork.Attend.Views.Pages.EventPagePreview" CodeBehind="EventPagePreview.aspx.cs" EnableViewState="false" MasterPageFile="~/modules/BVNetwork.Attend/Views/MasterPages/Attend.Master" %>

<%@ Register Src="~/modules/bvnetwork.attend/Views/Pages/Partials/EventPageEditScheduledEmail.ascx" TagPrefix="Attend" TagName="EventPageEditScheduledEmail" %>
<%@ Register Src="~/modules/bvnetwork.attend/Views/Pages/Partials/EventPageEditParticipants.ascx" TagPrefix="Attend" TagName="EventPageEditParticipants" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderArea">
    <%=CurrentData.Name %>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">

        $(document).ready(function () {
            // for bootstrap 3 use 'shown.bs.tab', for bootstrap 2 use 'shown' in the next line


            var lastTab = localStorage.getItem('lastTab');

            // go to the latest tab, if it exists:

            $('#tabAll').parent().addClass('active');
            $('.tab-pane').addClass('active in');
            $('[data-toggle="tab"]').parent().removeClass('active', function () {
                if (lastTab) {
                    $('[href="' + lastTab + '"]').tab('show');
                }
                else {
                    $('[href="#participants"]').tab('show');
                }

            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                // save the latest tab; use cookies if you like 'em better:
                localStorage.setItem('lastTab', $(this).attr('href'));
            });

            $(window).scroll(function () {
                if ($(window).scrollTop() + $(window).height() == $(document).height()) {
                    alert("bottom!");
                }
            });

            setTimeout(function () {
                $('[href="#participants"]').tab('show');
                $('.progress-bar-loading').width('20%');
            }, 10);

            setTimeout(function () {
                $('[href="#details"]').tab('show');
                $('.progress-bar-loading').width('40%');
            }, 300);
            setTimeout(function () {
                $('[href="#xform"]').tab('show');
                $('.progress-bar-loading').width('60%');
            }, 600);
            setTimeout(function () {
                $('[href="#content"]').tab('show');
                $('.progress-bar-loading').width('80%');
            }, 900);

            setTimeout(function () {
                $('[href="#email"]').tab('show');
                $('.progress-bar-loading').width('100%');
            }, 1200);


            var lastTab = localStorage.getItem('lastTab');

            setTimeout(function () {
                if (lastTab) {
                    $('[href="' + lastTab + '"]').tab('show');
                }
                else {
                    $('[href="#participants"]').tab('show');
                }
                $('.loading-event').addClass('loading-event-finished');
                $('.tab-wrapper').addClass('tab-wrapper-active');
            }, 1500);


        });
    </script>
    <br />
    <div class="loading-event">
        <div class="container">
            <div class="progress">
                <div class="progress-bar progress-bar-striped active progress-bar-loading" role="progressbar"
                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                  
                </div>
            </div>
        </div>
    </div>
    <div class="tab-wrapper">
        <div style="font-weight: bold;">
            <div class="container">
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#participants" aria-controls="participants" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/participants" />
                    </a></li>
                    <li role="presentation"><a href="#details" aria-controls="details" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/eventdetails" />
                    </a></li>
                    <li role="presentation"><a href="#xform" aria-controls="xform" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/form" />
                    </a></li>

                    <li role="presentation"><a href="#content" aria-controls="content" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/content" />
                    </a></li>

                    <li role="presentation"><a href="#sessions" aria-controls="sessions" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/sessions" />
                    </a></li>

                    <li role="presentation"><a href="#email" aria-controls="email" role="tab" data-toggle="tab">
                        <episerver:translate runat="server" text="/attend/edit/emailtemplates" />
                    </a></li>
                </ul>
            </div>
        </div>
        <br />
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="participants">

                <attend:eventpageeditparticipants runat="server" currenteventpagebase='<%#CurrentPage %>'></attend:eventpageeditparticipants>

            </div>

            <div role="tabpanel" class="tab-pane" id="details">

                <div class="">
                    <div class="container">
                        <div class="row">

                            <div class="col-lg-12">
                                <div class="">
                                    <div class="">
                                        <episerver:property runat="server" propertyname="EventDetails" id="EventDetailsPropertyControl">
                                        <rendersettings enableeditfeaturesforchildren="true" />
                                    </episerver:property>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div role="tabpanel" class="tab-pane" id="xform">

                <div class="container">



                    <div class="row">

                        <div class="col-lg-6">
                            <episerver:property runat="server" propertyname="AvailableSeatsText" />
                            <br />
                            <asp:PlaceHolder runat="server" ID="FormsPlaceHolder">
                                <episerver:property propertyname="RegistrationForm" runat="server" visible='<%#BVNetwork.Attend.Business.Settings.Settings.GetSetting("UseEpiserverForms").ToString() != true.ToString() %>'></episerver:property>
                                <asp:Panel ID="FormsPlaceHolderContainer" runat="server" Visible='<%#BVNetwork.Attend.Business.Settings.Settings.GetSetting("UseEpiserverForms").ToString() == true.ToString() %>'>
                                    <asp:Repeater runat="server" ID="RegistrationFormContainerRepeater">
                                        <ItemTemplate>
                                            <div class="well well-sm">
                                                <%#GetBlockName(Container.DataItem) %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </asp:PlaceHolder>


                        </div>
                    </div>
                </div>

            </div>

            <div role="tabpanel" class="tab-pane" id="content">

                <div class="container">



                    <div class="row">
                        <div class="col-lg-6">

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">
                                        <episerver:translate runat="server" text="/attend/edit/open" />
                                    </h3>
                                </div>
                                <div class="panel-body">
                                    <p>
                                        <episerver:translate runat="server" text="/attend/edit/contentwhenopen" />
                                    </p>
                                        <asp:Panel runat="server" ID="DetailsContentXhtmlWrapper">
                                            <episerver:property propertyname="DetailsContentXhtml" runat="server"></episerver:property>
                                        </asp:Panel>

                                    <asp:Panel runat="server" ID="DetailsContentRepeaterWrapper">
                                        <asp:Repeater runat="server" ID="DetailsContentRepeater">
                                            <ItemTemplate>
                                                <div class="well well-sm">
                                                    <%#GetBlockName(Container.DataItem) %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>

                                </div>
                            </div>






                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">
                                        <episerver:translate runat="server" text="/attend/edit/closed" />
                                    </h3>
                                </div>
                                <div class="panel-body">
                                    <p>
                                        <episerver:translate runat="server" text="/attend/edit/contentwhenclosed" />
                                    </p>
                                        <asp:Panel runat="server" ID="ClosedContentXhtmlWrapper">
                                            <episerver:property propertyname="ClosedContentXhtml" runat="server"></episerver:property>
                                        </asp:Panel>

                                    <asp:Panel runat="server" ID="ClosedContentRepeaterWrapper">
                                        <asp:Repeater runat="server" ID="ClosedContentRepeater">
                                            <ItemTemplate>
                                                <div class="well well-sm">
                                                    <%#GetBlockName(Container.DataItem) %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </div>
                            </div>


                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">
                                        <episerver:translate runat="server" text="/attend/edit/noseats" />
                                    </h3>
                                </div>
                                <div class="panel-body">
                                    <p>
                                        <episerver:translate runat="server" text="/attend/edit/contentwhennoseats" />
                                    </p>
                                        <asp:Panel runat="server" ID="NoSeatsXhtmlWrapper">
                                            <episerver:property propertyname="NoSeatsContentXhtml" runat="server"></episerver:property>
                                        </asp:Panel>



                                    <asp:Panel runat="server" ID="NoSeatsContentWrapper">
                                        <asp:Repeater runat="server" ID="NoSeatsContentRepeater">
                                            <ItemTemplate>
                                                <div class="well well-sm">
                                                    <%#GetBlockName(Container.DataItem) %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </div>
                            </div>


                        </div>

                        <div class="col-lg-6">
                            <div class="">


                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">
                                            <episerver:translate runat="server" text="/attend/edit/confirmed" />
                                        </h3>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                            <episerver:translate runat="server" text="/attend/edit/contentwhenconfirmed" />
                                        </p>
                                        <asp:Panel runat="server" ID="ConfirmedXhtmlWrapper">
                                            <episerver:property propertyname="CompleteContentXhtml" runat="server"></episerver:property>
                                        </asp:Panel>

                                        <asp:Panel runat="server" ID="ConfirmedContentWrapper">
                                            <asp:Repeater runat="server" ID="ConfirmedContentRepeater">
                                                <ItemTemplate>
                                                    <div class="well well-sm">
                                                        <%#GetBlockName(Container.DataItem) %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">
                                            <episerver:translate runat="server" text="/attend/edit/submitted" />
                                        </h3>
                                    </div>
                                    <div class="panel-body">
                                        <p>
                                            <episerver:translate runat="server" text="/attend/edit/contentwhensubmitted" />
                                        </p>

                                        <asp:Panel runat="server" ID="SubmittedXhtmlWrapper">
                                            <episerver:property propertyname="SubmittedContentXhtml" runat="server"></episerver:property>
                                        </asp:Panel>

                                        <asp:Panel runat="server" ID="SubmittedContentWrapper">
                                            <asp:Repeater runat="server" ID="SubmittedContentRepeater">
                                                <ItemTemplate>
                                                    <div class="well well-sm">
                                                        <%#GetBlockName(Container.DataItem) %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div role="tabpanel" class="tab-pane" id="sessions">
                <div class="container">
                    <div class="row">

                        <div class="col-lg-12">
                            <div class="panel panel-body">
                                <table class="table table-striped table-hover">
                                    <episerver:property runat="server" id="SessionsContentArea">
                                    <rendersettings tag="ListView" />
                                </episerver:property>
                                </table>
                                <br />
                                <div class="">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <asp:Label AssociatedControlID="SessionName" translate="/attend/edit/sessionname" CssClass="input-group-addon" runat="server"></asp:Label>
                                            <asp:TextBox runat="server" CssClass="form-control" ID="SessionName"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-primary" OnClick="CreateSession_Click" translate="/attend/edit/createsession">
                 
                                                </asp:LinkButton></span>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>


            <div role="tabpanel" class="tab-pane" id="email">
                <div class="container">

                    <attend:eventpageeditscheduledemail runat="server" id="EventPageBaseEditScheduledEmailControl" currenteventpagebase='<%#CurrentPage %>' />
                </div>
            </div>


        </div>
    </div>

</asp:Content>
