<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ Control Language="C#" Inherits="ViewUserControl<BVNetwork.Attend.Forms.Models.Forms.AttendSubmitButton>" %>

<%
    var formElement = Model.FormElement;
    var buttonText = Model.Label;

    var buttonDisableState = Model.GetFormSubmittableStatus(ViewContext.Controller.ControllerContext.HttpContext);
%>

<input type="hidden" class="Form__Element Form__SystemElement FormHidden AttendFormButton" name="__AttendEvent" value="<%: Model.AttendPage %>" />
<input type="hidden" class="Form__Element Form__SystemElement FormHidden AttendFormButton" name="__AttendParticipantEmail" value="<%: Model.ParticipantEmail %>" />
<input type="hidden" class="Form__Element Form__SystemElement FormHidden AttendFormButton" name="__AttendParticipantCode" value="<%: Model.ParticipantCode %>" />


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

    <% if(Model.PredefinedValues != null && !string.IsNullOrEmpty(Request.QueryString["code"])) { %>
        <script type="text/javascript">
            function getInputsByValue(type, name, value) {
                return document.querySelectorAll(type+'[value="' + value + '"][name="' + name + '"]');
            }

            function getInputsByName(type, name) {
                return document.querySelectorAll(type+'[name="' + name + '"]');
            }



            document.addEventListener("DOMContentLoaded", function (event) {
                <% foreach(var element in Model.PredefinedValues) { %>
                <% if(element.Key.Split(';')[0].ToLower().StartsWith("choice")) { %>
                <% foreach(var choice in element.Value.Split(',')) 
                    { 
                    %>
                var checkboxes = getInputsByValue('input','<%=element.Key.Split(';')[1] %>', '<%=choice%>');
                if (checkboxes != null && checkboxes.length > 0) {
                    for (var i = 0; i < checkboxes.length; i++) {
                        checkboxes[i].checked = true;
                    }
                }
                    <%
                    }
                    %>
                <% 
                }
                else if(element.Key.Split(';')[0].ToLower().StartsWith("select")) 
                { 
                %>
                var dropdowns = getInputsByName('select', '<%=element.Key.Split(';')[1] %>');
                if (dropdowns != null) {
                    for (var i = 0; i < dropdowns.length; i++) {
                        dropdowns[i].value = '<%=element.Value %>';
                    }
                }
                <% 
                }
                else if(element.Key.Split(';')[0].ToLower().StartsWith("textarea")) 
                { 
                %>
                var textareas = getInputsByName('textarea', '<%=element.Key.Split(';')[1] %>');
                if (textareas != null) {
                    for (var i = 0; i < textareas.length; i++) {
                        textareas[i].value = '<%=element.Value %>';
                    }
                }
                <% 
                }
                else 
                {
                 %>
                var inputs = getInputsByName('input', '<%=element.Key.Split(';')[1] %>');
                if (inputs != null) {
                    for (var i = 0; i < inputs.length; i++) {
                        inputs[i].value = '<%=element.Value %>';
                    }
                }
                <% 
                }
            } %>
            });


        </script>
    <% 
        }
        Model.PredefinedValues = null;
        Model.ParticipantEmail = null;
        Model.ParticipantCode = null;
    %>
