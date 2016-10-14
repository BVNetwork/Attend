<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventDetailsBlockControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.Static.EventDetailsBlockControl" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Pages" %>
<div class="row">
    <asp:Panel runat="server" Visible='<%#CurrentBlock.Cancelled %>'>
        <div class="col-lg-12">
            <div class="alert alert-danger">
                <episerver:translate runat="server" text="/attend/edit/cancelled"></episerver:translate>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" Visible='<%#CurrentBlock.Private && !CurrentBlock.Cancelled %>'>
        <div class="col-lg-12">
            <div class="alert alert-info">
                <episerver:translate runat="server" text="/attend/edit/private"></episerver:translate>
            </div>
        </div>
    </asp:Panel>
    <div class="col-lg-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title"><episerver:translate runat="server" text="/attend/edit/time"/></h3>
            </div>
            <div class="panel-body">
                <table class="table table-striped table-responsive table-striped table-hover">
                    <tr>
                        <td><b>
                            <episerver:translate runat="server" text="/attend/edit/eventstart" />
                        </b></td>


                        <td><%=(CurrentBlock.EventStart != DateTime.MinValue) ? CurrentBlock.EventStart.ToString() : "N/A" %></td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <episerver:translate runat="server" text="/attend/edit/eventend" />
                            </b></td>
                        <td><%=(CurrentBlock.EventEnd != DateTime.MinValue) ? CurrentBlock.EventEnd.ToString() : "N/A" %>

                        </td>
                    </tr>

                    <tr>
                        <td><b>
                            <episerver:translate runat="server" text="/attend/edit/location" />
                        </b></td>
                        <td>
                            <episerver:property runat="server" propertyname="Location"></episerver:property>
                        </td>
                    </tr>
                    <tr>
                        <td><b>
                            <episerver:translate runat="server" text="/attend/edit/contact" />
                        </b></td>
                        <td><%=CurrentBlock.Contact%></td>
                    </tr>

                </table>


            </div>
        </div>
    </div>


    <div class="col-lg-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title"><episerver:translate runat="server" text="/attend/edit/moreinfo"/></h3>
            </div>
            <div class="panel-body">

                <table>
                    <tr>
                        <td><b>
                            <episerver:translate runat="server" text="/attend/edit/numberofseats" />
                        </b>&nbsp;&nbsp;</td>
                        <td><%#CurrentBlock.NumberOfSeats %></td>
                    </tr>
                    <tr>
                        <td><b>
                            <episerver:translate runat="server" text="/attend/edit/price" />
                        </b>&nbsp;&nbsp;</td>
                        <td><%#CurrentBlock.Price %></td>
                    </tr>
                </table>
                <div class="progress" style="background-color: #ccc; margin-top: 5px;">
                    <%#GetProgressBar(CurrentPage as EventPageBase) %>
                </div>
            </div>
        </div>
    </div>


   <asp:Panel runat="server" Visible='<%#CurrentBlock.RegistrationOpen < DateTime.Now && (CurrentBlock.RegistrationClose > DateTime.Now) %>'>
        <div class="col-lg-6">
            <div class="alert alert-success">
                <episerver:translate runat="server" text="/attend/edit/registrationopen"></episerver:translate>
            </div>
        </div>
    </asp:Panel>
    
   <asp:Panel runat="server" Visible='<%#CurrentBlock.RegistrationOpen >= DateTime.Now || CurrentBlock.RegistrationClose < DateTime.Now %>'>
        <div class="col-lg-6">
            <div class="alert alert-danger">
                <episerver:translate runat="server" text="/attend/edit/registrationclosed"></episerver:translate>
            </div>
        </div>
    </asp:Panel>
</div>




