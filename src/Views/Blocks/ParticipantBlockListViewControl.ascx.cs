using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Editor;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms;
using System.Collections.Specialized;
using EPiServer.XForms.WebControls;
using EPiServer.XForms.Util;
using System.Collections;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = true, Tags = new[] { "ListView" }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ParticipantBlockListViewControl.ascx")]
    public partial class ParticipantBlockListViewControl : BlockControlBase<ParticipantBlock>
    {
        private NameValueCollection _formControls;
        private Hashtable _hiddenControls;

        protected NameValueCollection FormControls
        {
            get
            {
                EventPage CurrentEvent = Locate.ContentRepository().Get<EventPage>(CurrentBlock.EventPage);


                if (CurrentBlock.XForm == null)
                {
                    _formControls = new NameValueCollection();
                }
                else if (_formControls == null)
                {

                    SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                    xmlDoc.LoadXml(CurrentBlock.XForm);
                    XFormData data = new XFormData();
                    data.Data = xmlDoc;
                    NameValueCollection formControls = data.GetValues();
                    _formControls = formControls;

                }
                return _formControls;
            }
        }

        protected Hashtable HiddenControls
        {
            get
            {
                if (_hiddenControls == null)
                {
                    _hiddenControls = new Hashtable();
                    EventPage eventPage = Locate.ContentRepository().Get<EventPage>(CurrentBlock.EventPage);
                    if (!string.IsNullOrEmpty(eventPage.RemoveFieldsFromEdit))
                    {
                        string[] fieldsToRemove = eventPage.RemoveFieldsFromEdit.Split(',');
                        foreach (string s in fieldsToRemove)
                            _hiddenControls.Add(s, s);
                    }
                }
                return _hiddenControls;
            }

        }

        protected string GetFormControlValue(string key)
        {
            if (HiddenControls.ContainsKey(key))
                return string.Empty;
            return "<td>" + FormControls.Get(key) + "</td>";
        }

        protected string GetFormControlDetailsValue(string key)
        {
            if (HiddenControls.ContainsKey(key))
                return string.Empty;
            return "<tr><td>" + key + "</td><td>" + FormControls.Get(key) + "</td></tr>";
        }

        protected string GetStatusClass()
        {
            switch ((CurrentBlock as ParticipantBlock).AttendStatus.ToLower())
            {
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
            if (CurrentBlock.Sessions != null)
            {
                int sessionsCount = CurrentBlock.Sessions.Count;
                foreach (var session in CurrentBlock.Sessions.Items)
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
            if (Locate.ContentRepository().Get<EventPage>(CurrentBlock.EventPage).EventDetails.PdfTemplate != null)
                return String.Format("<a class=\"btn btn-default btn-sm\" href=\"{0}?participant={1}&code={2}&email={3}\" target=\"_blank\">PDF</a>", UriSupport.ResolveUrlBySettings("~/Modules/BVNetwork.Attend/Views/Pages/Pdf.aspx"), (CurrentData as IContent).ContentLink.ID ,CurrentData.Code, CurrentData.Email);
            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            FormFieldsRepeater.DataSource = FormControls.AllKeys.Take(5);
            FormFieldsRepeater.DataBind();
            FormFieldsModalRepeater.DataSource = FormControls.AllKeys;
            FormFieldsModalRepeater.DataBind();
            //SelectCheckBox.Checked = IsChecked();
            //SelectCheckBox.DataBind();
            Copy.DataBind();
        }

        protected void Copy_OnClick(object sender, EventArgs e)
        {
            if(IsChecked())
                AttendRegistrationEngine.CopyParticipantsRemove(CurrentBlock);
            else
            {
                AttendRegistrationEngine.CopyParticipants(CurrentBlock);
            }
                
            Copy.DataBind();
        }

        protected string GetClass()
        {
            if (IsChecked())
                return "success";
            
            switch ((CurrentBlock as ParticipantBlock).AttendStatus.ToLower())
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
            return AttendRegistrationEngine.IsChecked((CurrentBlock as IContent).ContentLink.ID);
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
            if((sender as CheckBox).Checked)
                AttendRegistrationEngine.CopyParticipants(CurrentBlock);
        }
    }
}