<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduledEmailBlockListViewControl.ascx.cs" Inherits="BVNetwork.Attend.Views.Blocks.ScheduledEmailBlockListViewControl" %>
<%@ Import Namespace="BVNetwork.Attend.Models.Blocks" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>

<tr >
    
    <td><%=GetStatus() %></td>

    <td>
        <%=(CurrentBlock as EPiServer.Core.IContent).Name %>
    </td>
    
    
    <td><%=GetScheduledDate()  %></td>

    <td><%=((CurrentBlock) as ScheduledEmailBlock).AttendStatusFilter %></td>

    <td><%=(((CurrentBlock) as ScheduledEmailBlock).GetEmailTemplateBlock().SendAsSms) ? "SMS" : "E-mail" %></td>
    
    <td><div class="btn-group">
                <button type="button" class="btn btn-default btn-sm" data-toggle="modal" data-target="#modal-<%=(CurrentData as EPiServer.Core.IContent).ContentLink.ID %>">
        <span class="glyphicon glyphicon-modal-window"></span>
           
        </button>



        <a class="btn btn-default btn-sm" target="_top" href='<%=EPiServer.Editor.PageEditing.GetEditUrl((CurrentBlock as EPiServer.Core.IContent).ContentLink) %>'>
        <span class="glyphicon glyphicon-pencil"></span>
       
    </a>
             <asp:LinkButton runat="server" ID="DeleteScheduledEmail" CssClass="btn btn-default btn-sm" Text="<span class='glyphicon glyphicon-trash'></span>" OnClientClick="return confirm('Are you sure you want to delete this e-mail?');" OnClick="DeleteScheduledEmail_OnClick"></asp:LinkButton>
                   </div>
    </td>


</tr>
        <div class="modal" id="modal-<%=(CurrentData as EPiServer.Core.IContent).ContentLink.ID %>" tabindex="-1000000" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">X</span></button>
                        <h4 class="modal-title" id="myModalLabel">Details</h4>
                    </div>
                    <div class="modal-body">

                    </div>
                </div>
            </div>
        </div>
