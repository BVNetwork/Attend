<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventPageEditParticipantsParticipant.ascx.cs" Inherits="BVNetwork.Attend.Views.Pages.Partials.EventPageEditParticipantsParticipant" %>
<%@ Import Namespace="BVNetwork.Attend.Business.Participant" %>
<tr class="<%=GetClass() %>">

                            <td>
                                <asp:LinkButton runat="server" CssClass="btn btn-default btn-sm" ID="Copy" OnClick="Copy_OnClick" Text='<%#CheckedText()%>'></asp:LinkButton>
                            </td>

                            <td>
                                <div class="<%=GetStatusClass() %>"><%=CurrentData.AttendStatusText%></div>
                            </td>

                            <td>
                                <%=CurrentData.Email %>
                            </td>


                            <td>
                                <%=GetSessions() %>
                            </td>


                            <td>
                                <%=CurrentData.Price%>
                            </td>

                            <asp:Repeater runat="server" ID="FormFieldsRepeater" >
                                <ItemTemplate>
                                    <%#GetFormControlValue((Container.DataItem).ToString()) %>
                                </ItemTemplate>
                            </asp:Repeater>

                            <td>
                                <button type="button" class="btn btn-default btn-sm btn-block" data-toggle="modal" data-target="#modal-<%=(CurrentData as IParticipant).Code %>">
                                    ...
                                </button>

                                <div class="modal" id="modal-<%=(CurrentData as IParticipant).Code %>" tabindex="-1000000" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">X</span></button>
                                                <h4 class="modal-title" id="myModalLabel">Details</h4>
                                            </div>
                                            <div class="modal-body">
                                                <table class="table">
                                                    <asp:Repeater runat="server" ID="FormFieldsModalRepeater">
                                                        <ItemTemplate>
                                                            <%#GetFormControlDetailsValue((Container.DataItem).ToString()) %>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>



                            </td>

                            <td><a class="btn btn-default btn-sm" target="_top" href='<%=ParticipantProviderManager.Provider.GetEditUrl(CurrentData) %>'>
                                <span class="glyphicon glyphicon-pencil"></span>

                            </a></td>

                            <td>
                                <%=GetPdfUrl()%>
                            </td>

                        </tr>
