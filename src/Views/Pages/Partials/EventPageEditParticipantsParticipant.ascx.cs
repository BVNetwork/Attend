using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.XForms;
using EPiServer.XForms.Util;

namespace BVNetwork.Attend.Views.Pages.Partials
{
    public partial class EventPageEditParticipantsParticipant : EPiServer.UserControlBase
    {

        public IParticipant CurrentData { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //SetupPreviewPropertyControl(ParticipantsContentArea, Participants);
            if (FormControls != null && FormControls.Count > 0)
            {

                FormFieldsRepeater.DataSource = FormControls.AllKeys.Take(5);
                FormFieldsRepeater.DataBind();
                FormFieldsModalRepeater.DataSource = FormControls.AllKeys;
                FormFieldsModalRepeater.DataBind();
            }
            //SelectCheckBox.Checked = IsChecked();
            //SelectCheckBox.DataBind();
            Copy.DataBind();


        }


        private NameValueCollection _formControls;
        private Hashtable _hiddenControls;

        protected NameValueCollection FormControls
        {
            get
            {
                EventPageBase CurrentEvent = Locate.ContentRepository().Get<EventPageBase>(CurrentData.EventPage);


                if (CurrentData.XForm == null)
                {
                    _formControls = new NameValueCollection();
                }
                else if (_formControls == null && !string.IsNullOrEmpty(CurrentData.XForm))
                {

                    SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                    xmlDoc.LoadXml(CurrentData.XForm);
                    XFormData data = new XFormData();
                    data.Data = xmlDoc;
                    NameValueCollection formControls = data.GetValues();
                    _formControls = formControls;

                }
                return _formControls;
            }
        }


        protected string GetFormControlValue(string key)
        {
            return "<td>" + FormControls.Get(key) + "</td>";
        }

        protected string GetFormControlDetailsValue(string key)
        {
            return "<tr><td>" + key + "</td><td>" + FormControls.Get(key) + "</td></tr>";
        }

        protected string GetStatusClass()
        {
            switch ((CurrentData as IParticipant).AttendStatus.ToLower())
            {
                case "participated":
                    return "label label-success";
                case "confirmed":
                    return "label label-primary";
                case "submitted":
                    return "label label-default";
            }
            return "label label-danger";
        }

        protected string GetSessions()
        {
            string sessions = "";
            if (CurrentData.Sessions != null)
            {
                int sessionsCount = CurrentData.Sessions.Count;
                foreach (var session in CurrentData.Sessions.Items)
                {
                    sessions += Locate.ContentRepository().Get<IContent>(session.ContentLink).Name + ", ";
                }
                if (sessions.Length > 1)
                    sessions = sessions.Substring(0, sessions.Length - 2);
                return string.Format("<span title='{0}'>{1}</span>", sessions, sessionsCount);

            }
            return sessions;
        }


        protected string GetPdfUrl()
        {
            if (Locate.ContentRepository().Get<EventPageBase>(CurrentData.EventPage).EventDetails.PdfTemplate != null)
                return String.Format("<a class=\"btn btn-default btn-sm\" href=\"{0}?participant={1}&code={2}&email={3}\" target=\"_blank\">PDF</a>", UriSupport.ResolveUrlBySettings("~/Modules/BVNetwork.Attend/Views/Pages/Pdf.aspx"), CurrentData.Code, CurrentData.Code, CurrentData.Email);
            return string.Empty;
        }


        protected void Copy_OnClick(object sender, EventArgs e)
        {
            if (IsChecked())
                AttendRegistrationEngine.CopyParticipantsRemove(CurrentData);
            else
            {
                AttendRegistrationEngine.CopyParticipants(CurrentData);
            }

            Copy.DataBind();
        }

        protected string GetClass()
        {
            if (IsChecked())
                return "success";

            switch ((CurrentData as IParticipant).AttendStatus.ToLower())
            {
                case "cancelled":
                    return "danger";
                case "submitted":
                    return "warning";
            }
            return "";
        }

        protected bool IsChecked()
        {
            return AttendRegistrationEngine.IsChecked(CurrentData.Code);
        }

        protected string CheckedText()
        {
            return IsChecked()
                ? "<span class='glyphicon glyphicon-check'></span> " //+
                //LocalizationService.GetString("/attend/edit/deselect")
                : "<span class='glyphicon glyphicon-unchecked'></span> "; // +
            //LocalizationService.GetString("/attend/edit/select");
        }

        protected void SelectCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
                AttendRegistrationEngine.CopyParticipants(CurrentData);
        }




    }
}