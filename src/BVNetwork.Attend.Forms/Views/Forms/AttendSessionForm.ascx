<%@ import namespace="System.Web.Mvc" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ Control Language="C#" Inherits="ViewUserControl<BVNetwork.Attend.Forms.Models.Forms.AttendSessionForm>" %>



<% if (Model.Sessions.Count > 0)
{
    var formElement = Model.FormElement; 
    var labelText = Model.Label;
%>

<div style="display:none;" class="Form__Element FormTextbox <%: Model.GetValidationCssClasses() %>" data-epiforms-element-name="<%: formElement.ElementName %>">
    <label for="<%: formElement.Guid %>" class="Form__Element__Caption"><%: labelText %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid %>" type="text" class="FormTextbox__Input SessionsFormTextBox"
        placeholder="<%: Model.PlaceHolder %>" value="<%: Model.GetDefaultValue() %>" <%: Html.Raw(Model.AttributesString) %> />

    <span data-epiforms-linked-name="<%: formElement.ElementName %>" class="Form__Element__ValidationError" style="display: none;">*</span>
    <%= Model.RenderDataList() %>
</div>
    <div class="well-sessions" id="sessions">
        <div class="form-group">
            <h4><%=Html.Translate("/eventRegistrationPage/sessionsIntroText")%> <%=Model.EventName %></h4>
            <% foreach (var session in Model.Sessions)
            {
                
                if (session.NewGroup)
                {
                %>
                    <h5><%=session.Header%></h5>
                <%}

                if (BVNetwork.Attend.Business.API.AttendSessionEngine.HasParallelSessions(session, Model.Sessions) == false)
                { %>
                    <div class="checkbox" @(session.Enabled ? "" : "disabled")>
                        <label>
                            <input type="checkbox" class="sessionCheckBox" <%=(session.Selected ? "checked='' " : "") %> name="<%=session.Group %>" value="<%=session.ContentID %>" <%=(session.Enabled ? "" : "disabled")%>>
                             <%=session.Name %>
                        </label>
                    </div>
                <%}
                if (BVNetwork.Attend.Business.API.AttendSessionEngine.HasParallelSessions(session, Model.Sessions))
                { %>
                    <label class="radio-inline">
                        <input type="radio" class="sessionCheckBox" @(session.Selected ? "checked=''" : "") name="<%=session.Group %>" value="<%=session.ContentID %>" <%= (session.Enabled ? "" : "disabled") %>> <%=session.Name %>
                    </label>
               <% }
            } %>
        </div>
    </div>
<script type="text/javascript">
    $('.sessionCheckBox').change(function () {
        console.log('Session Chosen!');
        var fields = $('.sessionCheckBox');
        var sessionsSelected = '';
        jQuery.each(fields, function (i, field) {
            if(field.checked)
                sessionsSelected = sessionsSelected + field.value + ",";
        });
        $('.SessionsFormTextBox').val(sessionsSelected);
        console.log(sessionsSelected);
    });

    /*
    $(document).ready(function () {
        $(":checkbox").on('click', function () {
            if ($(':checkbox:checked')) {
                var fields = $(":checkbox").val();
                jQuery.each(fields, function (i, field) {
                    $('#field_results').val($('#field_results').val() + field.value + " ");
                });
            }
        });
    });*/

    function UpdateSessions(){
        ($.FormSessionTextBox).value;
    }
</script>
<% }
    %>
