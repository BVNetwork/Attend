using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using EPiServer.XForms;
using EPiServer.XForms.Util;

namespace BVNetwork.Attend.Views.Pages.Partials
{
    public partial class EventPageEditParticipants : EPiServer.UserControlBase
    {
        protected EventPage CurrentEventPage { get; set; }
        protected List<IParticipant> Participants { get; set; }
        protected List<SessionBlock> Sessions { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            Participants = AttendRegistrationEngine.GetParticipants(CurrentPage.ContentLink).ToList();
            NoParticipants.Visible = !(Participants.Count<IParticipant>() > 0);
            Sessions = AttendSessionEngine.GetSessions(CurrentPage.ContentLink).ToList();

            PopulateStatusDropDown();
            PopulateEMailDropDown();
            PopulateSessionDropDown();

            DeleteParticipantsCopy.OnClientClick = "return confirm('" + DeleteConfirmation() + "');";

            NoParticipants.DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateParticipants();

        }

        protected override void OnPreRender(EventArgs e)
        {
            CopyMovePlaceHolder.Visible = AttendRegistrationEngine.GetOrCreateParticipantsClipboard().Count > 0;
        }

        protected override void OnDataBinding(EventArgs e)
        {
            int numberOfCopied = AttendRegistrationEngine.GetOrCreateParticipantsClipboard().Count;
            CopyMovePlaceHolder.Visible = numberOfCopied > 0;
            CopyMovePlaceHolder.DataBind();
            SelectedRepeater.DataSource = AttendRegistrationEngine.GetOrCreateParticipantsClipboard();
            SelectedRepeater.DataBind();
            base.OnDataBinding(e);
        }



        protected void PopulateSessionDropDown()
        {
            SessionDropDownList.Items.Add(new ListItem(Locate.LocalizationService().GetString("/attend/edit/sessions"), ""));
            List<IParticipant> contents = Participants;
            contents = FilterStatus(contents);
            contents = FilterSearch(contents);
            contents = FilterEmail(contents);

            foreach (SessionBlock sessionBlock in Sessions)
            {
                var currentSession =
                    contents.Where(
                        x =>
                            (x as IParticipant).Sessions != null && (x as IParticipant).Sessions.Items.Where(s => s.ContentLink.ID == (sessionBlock as IContent).ContentLink.ID).Any());

                SessionDropDownList.Items.Add(new ListItem((sessionBlock as IContent).Name + " (" + currentSession.Count() + ")", (sessionBlock as IContent).ContentLink.ID.ToString()));
            }
        }

        protected void PopulateEMailDropDown()
        {
            List<IParticipant> contents = Participants;
            contents = FilterStatus(contents);
            contents = FilterSearch(contents);
            contents = FilterSessions(contents);

            EMailDropDownList.Items.Add(new ListItem(Locate.LocalizationService().GetString("/attend/edit/email"), ""));
            var emails = (from IParticipant p in contents where p.Email.Split('@').Length > 1 select p.Email.Split('@')[1]).OrderBy(p => p).Distinct();
            foreach (var email in emails)
            {
                EMailDropDownList.Items.Add(new ListItem(email + " (" + (from p in contents where (p as IParticipant).Email.EndsWith(email) select p).Count() + ")", email));
            }
        }

        protected void PopulateStatusDropDown()
        {
            Array itemValues = System.Enum.GetValues(typeof(AttendStatus));
            Array itemNames = System.Enum.GetNames(typeof(AttendStatus));
            StatusDropDown.Items.Add(new ListItem(ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/selectstatus"), ""));
            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                ListItem item = new ListItem(ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/attendstatus/" + itemValues.GetValue(i).ToString()), itemValues.GetValue(i).ToString());
                StatusDropDown.Items.Add(item);
            }

            StatusFilterDropDown.Items.Add(new ListItem(Locate.LocalizationService().GetString("/attend/edit/status"), ""));
            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                string text = GetStatusNameAndNumber(itemNames.GetValue(i).ToString());
                ListItem item = new ListItem(text, itemValues.GetValue(i).ToString());
                item.Selected = Request.Form["StatusFilterDropDown"] == item.Value;
                StatusFilterDropDown.Items.Add(item);
            }

            StatusFilterDropDown.SelectedValue = Request.Form["StatusFilterDropDown"];
            //StatusDropDown.DataBind();
            //StatusFilterDropDown.DataBind();
        }

        private string GetStatusNameAndNumber(string p)
        {
            int cnt = Participants.Where(x => x.AttendStatus == p).Count();
            string text = Locate.LocalizationService().GetString("/attend/attendstatus/" + p);
            return text + " (" + cnt.ToString() + ")";
        }

        private List<IParticipant> FilterStatus(List<IParticipant> contents)
        {
            if (StatusFilterDropDown.SelectedIndex > 0)
                contents =
                    contents.Where(x => (x as IParticipant).AttendStatus == StatusFilterDropDown.SelectedValue).ToList<IParticipant>();
            return contents;
        }

        private List<IParticipant> FilterEmail(List<IParticipant> contents)
        {
            if (EMailDropDownList.SelectedIndex > 0)
                contents =
                    contents.Where(x => (x as IParticipant).Email.Trim().EndsWith(EMailDropDownList.SelectedValue)).ToList<IParticipant>();


            return contents;
        }

        private List<IParticipant> FilterSessions(List<IParticipant> contents)
        {

            if (SessionDropDownList.SelectedIndex > 0)
            {
                var session = Locate.ContentRepository().Get<IContent>(new ContentReference(int.Parse(SessionDropDownList.SelectedValue)));
                contents =
                    contents.Where(
                        x =>
                            (x as IParticipant).Sessions != null && (x as IParticipant).Sessions.Items.Where(s => s.ContentLink.ID.ToString() == SessionDropDownList.SelectedValue).Any()).ToList<IParticipant>();
            }

            return contents;
        }


        private List<IParticipant> FilterSearch(List<IParticipant> contents)
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                contents = contents.Where(x => (x as IParticipant).XForm != null && (x as IParticipant).XForm.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList<IParticipant>();

            return contents;
        }

        private void PopulateParticipants()
        {
            int total = Participants.Count;

            var contents = Participants;

            contents = FilterStatus(contents);
            contents = FilterEmail(contents);
            contents = FilterSessions(contents);
            contents = FilterSearch(contents);

            int totalFiltered = contents.Count;

            NumberOfParticipantsLiteral.Text = string.Format(Locate.LocalizationService().GetString("/attend/edit/findresult"), totalFiltered, total);

            ParticipantsRepeater.DataSource = contents;
            ParticipantsRepeater.DataBind();

        }


        private int GetIntSafe(string text)
        {
            int i = 0;
            int.TryParse(text, out i);
            return i;
        }

        protected void ExportButton_OnClick(object sender, EventArgs e)
        {
            List<IParticipant> contents = Participants;
            contents = FilterStatus(contents);
            contents = FilterEmail(contents);
            contents = FilterSessions(contents);
            contents = FilterSearch(contents);

            ParticipantExport.Export((from x in contents select x as IParticipant).ToList());

        }

        protected void RemoveFiltersLinkButton_OnClick(object sender, EventArgs e)
        {
            EMailDropDownList.SelectedIndex = 0;
            SessionDropDownList.SelectedIndex = 0;
            StatusFilterDropDown.SelectedIndex = 0;
            SearchTextBox.Text = string.Empty;

        }

        protected void DeleteParticipantsCopy_OnClick(object sender, EventArgs e)
        {
            foreach (IParticipant IParticipant in AttendRegistrationEngine.GetOrCreateParticipantsClipboard())
            {
                ServiceLocator.Current.GetInstance<IContentRepository>().Delete((IParticipant as IContent).ContentLink, true, AccessLevel.NoAccess);
            }

            AttendRegistrationEngine.CopyParticipantsRemoveAll();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentPage as IContent).ContentLink) + "'", true);

        }


        protected string DeleteConfirmation()
        {
            return ServiceLocator.Current.GetInstance<LocalizationService>()
                .GetString("/attend/edit/deleteconfirmation");
        }

        protected void CheckAll_OnClick(object sender, EventArgs e)
        {
            int numberOfCopied = AttendRegistrationEngine.GetOrCreateParticipantsClipboard().Count;
            if (numberOfCopied > 0)
                AttendRegistrationEngine.CopyParticipantsRemoveAll();
            if (numberOfCopied == 0)
            {

                List<IParticipant> contents = Participants;
                contents = FilterStatus(contents);
                contents = FilterEmail(contents);
                contents = FilterSessions(contents);
                contents = FilterSearch(contents);

                foreach (var IParticipant in contents)
                {
                    AttendRegistrationEngine.CopyParticipants(IParticipant as IParticipant);
                }
            }
        }

        protected void PasteParticipantsMove_OnClick(object sender, EventArgs e)
        {
            AttendRegistrationEngine.PasteParticipantsMove(CurrentPage as EventPage);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentPage as IContent).ContentLink) + "'", true);

        }

        protected void PasteParticipantsCopy_OnClick(object sender, EventArgs e)
        {
            AttendRegistrationEngine.PasteParticipantsCopy(CurrentPage as EventPage);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentPage as IContent).ContentLink) + "'", true);

        }

        protected void PasteParticipantsRemove_OnClick(object sender, EventArgs e)
        {
            AttendRegistrationEngine.CopyParticipantsRemoveAll();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentPage as IContent).ContentLink) + "'", true);

        }


        protected void PasteParticipantsExport_OnClick(object o, EventArgs e)
        {
            ParticipantExport.Export(AttendRegistrationEngine.GetOrCreateParticipantsClipboard());
        }

        protected void ChangeStatus_Click(object sender, EventArgs e)
        {
            if (StatusDropDown.SelectedIndex > 0)
                AttendRegistrationEngine.CopyParticipantsChangeStatus(StatusDropDown.SelectedValue, StatusMailCheckBox.Checked);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentPage as IContent).ContentLink) + "'", true);
        }

        protected void CreateParticipant_Click(object sender, EventArgs e)
        {
            IParticipant newParticipant = null;
            EmailPlaceHolder.Visible = false;
            if (Business.Email.Validation.IsEmail(EMailTextBox.Text) == false) { 
                EmailPlaceHolder.Visible = true;
            }
            else { 
                newParticipant = AttendRegistrationEngine.GenerateParticipation(CurrentPage.ContentLink, EMailTextBox.Text, string.Empty);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((newParticipant as IContent).ContentLink) + "'", true);
            }
        }


        public bool SendStatusMail(IParticipant participant, EmailTemplateBlock et)
        {
            EmailSender email;
            email = new EmailSender(participant, et);

            email.Send();

            return true;
        }


        protected void StatusFilterDropDown_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}