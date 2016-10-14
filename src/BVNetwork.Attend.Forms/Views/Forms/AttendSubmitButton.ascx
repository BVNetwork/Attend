<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ Control Language="C#" Inherits="ViewUserControl<BVNetwork.Attend.Forms.Models.Forms.AttendSubmitButton>" %>

<%
    var formElement = Model.FormElement;
    var buttonText = Model.Label;

    var buttonDisableState = Model.GetFormSubmittableStatus(ViewContext.Controller.ControllerContext.HttpContext);
%>

<input type="hidden" class="Form__Element Form__SystemElement FormHidden AttendFormButton" name="__AttendEvent" value="<%: Model.AttendPage %>" />


<button id="<%: formElement.Guid %>" name="submit" type="submit" value="<%: formElement.Guid %>" data-epiforms-is-finalized="<%: Model.FinalizeForm.ToString().ToLower() %>"
    data-epiforms-is-progressive-submit="true"
    <%= Model.AttributesString %> <%: buttonDisableState %>
    <% if (Model.Image == null) 
    { %>
        class="Form__Element FormExcludeDataRebind FormSubmitButton">
        <%: buttonText %>
    <% } else { %>
        class="Form__Element FormExcludeDataRebind FormSubmitButton FormImageSubmitButton">
        <img src="<%: Model.Image.Path %>" data-epiforms-is-progressive-submit="true" data-epiforms-is-finalized="<%: Model.FinalizeForm.ToString().ToLower() %>" />
    <% } %>
</button>
