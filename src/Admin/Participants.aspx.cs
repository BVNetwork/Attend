using System.Collections.Specialized;
using System.Globalization;
using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.XForms;
using EPiServer.XForms.WebControls;

namespace BVNetwork.Attend.Admin
{
    public partial class Participants : System.Web.UI.Page
    {
        protected string SelectedEmail { get; set; }

        protected List<IParticipant> ParticipantsList { get; set; }
        protected List<string> FieldsList { get; set; }

        protected DateTime ToDateTime
        {
            get
            { return DateTime.Parse(TextBoxToDate.Text); }
            set
            {
                TextBoxToDate.Text = value.ToShortDateString();
                TextBoxToDate.DataBind();
            }
        }

        protected DateTime FromDateTime
        {
            get { return DateTime.Parse(TextBoxFromDate.Text); }
            set
            {
                TextBoxFromDate.Text = value.ToShortDateString();
                TextBoxFromDate.DataBind();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            base.OnPreInit(e);
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["todate"]) || string.IsNullOrEmpty(Request.QueryString["fromdate"]))
                Response.Redirect("Participants.aspx?fromdate=" + DateTime.Now.ToShortDateString() + "&todate=" + DateTime.Now.AddMonths(6).ToShortDateString());

            if (string.IsNullOrEmpty(TextBoxToDate.Text))
                ToDateTime = DateTime.Parse(Request.QueryString["todate"]);
            if (string.IsNullOrEmpty(TextBoxFromDate.Text))
                FromDateTime = DateTime.Parse(Request.QueryString["fromdate"]);

            if (string.IsNullOrEmpty(PagingPage.Text))
            {
                PagingPage.Text = "1";
                PagingPage.DataBind();
            }

            PopulateParticipants();

            FormFieldsCheckBoxList.DataSource = FieldsList;
            FormFieldsCheckBoxList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupPreviewPropertyControl(ParticipantsList);
        }

        private void PopulateParticipants()
        {
            List<IParticipant> participants = EPiServer.CacheManager.Get("participants-" + FromDateTime.ToString() + ToDateTime.ToString()) as List<IParticipant>;
            FieldsList = EPiServer.CacheManager.Get("fieldnames-" + FromDateTime.ToString() + ToDateTime.ToString()) as List<String>;
            if (participants == null)
            {
                FieldsList = new List<string>();

                var events = ParticipantProviderManager.Provider.GetEventPages();
                participants = new List<IParticipant>();
                foreach (PageData EventPageBaseData in (events))
                {
                    if ((EventPageBaseData as EventPageBase).EventDetails.EventEnd <= ToDateTime &&
                        (EventPageBaseData as EventPageBase).EventDetails.EventStart >= FromDateTime)
                    {
                        ExtractFieldNames(EventPageBaseData as EventPageBase);
                        foreach (var participant in AttendRegistrationEngine.GetParticipants(EventPageBaseData.ContentLink))
                        {
                            participants.Add(participant as IParticipant);
                        }
                    }
                }
                EPiServer.CacheManager.Insert("fieldnames-" + FromDateTime.ToString() + ToDateTime.ToString(), FieldsList);
                EPiServer.CacheManager.Insert("participants-" + FromDateTime.ToString() + ToDateTime.ToString(), participants);
            }
            participants = (from p in participants orderby p.DateSubmitted descending select p).ToList();
            ParticipantsList = participants;
            SetupPreviewPropertyControl(ParticipantsList);



        }

        private void ExtractFieldNames(EventPageBase EventPageBase)
        {
            if (EventPageBase.RegistrationForm == null)
                return;

            XForm xform = XForm.CreateInstance(new Guid(EventPageBase.RegistrationForm.Id.ToString()));
            NameValueCollection formControls = xform.CreateFormData().GetValues();
            foreach (string data in formControls)
            {
                if (!FieldsList.Contains(data))
                    FieldsList.Add(data);
            }
        }

        protected void PopulateEMailDropDown()
        {
            IEnumerable<IParticipant> contents = ParticipantsList;
            contents = FilterStatus(contents);
            contents = FilterSearch(contents);
            contents = FilterEvent(contents);
            string selected = EMailDropDownList.SelectedValue;
            EMailDropDownList.Items.Clear();

            EMailDropDownList.Items.Add(new ListItem(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/email"), ""));
            var emails = (from IParticipant p in contents where p.Email.Split('@').Length > 1 select p.Email.Split('@')[1]).OrderBy(p => p).Distinct();
            foreach (var email in emails)
            {
                ListItem item =
                    new ListItem(
                        email + " (" +
                        (from p in contents where (p as ParticipantBlock).Email.EndsWith(email) select p).Count() + ")",
                        email);
                item.Selected = (selected == item.Value);
                EMailDropDownList.Items.Add(item);
            }
            EMailDropDownList.DataBind();
        }

        protected void PopulateEventDropDown()
        {
            IEnumerable<IParticipant> contents = ParticipantsList;
            contents = FilterStatus(contents);
            contents = FilterSearch(contents);
            contents = FilterEmail(contents);
            string selected = EventsDropDownList.SelectedValue;
            EventsDropDownList.Items.Clear();

            EventsDropDownList.Items.Add(new ListItem(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/event"), ""));
            var events = (from IParticipant p in contents select p.EventPage).OrderBy(p => p).Distinct();
            foreach (var eventid in events)
            {
                ListItem item = new ListItem(
                        DataFactory.Instance.Get<EventPageBase>(eventid).Name + " (" +
                        (from p in contents where (p as IParticipant).EventPage.ID == eventid.ID select p).Count() +
                        ")", eventid.ID.ToString());
                item.Selected = (selected == item.Value);
                EventsDropDownList.Items.Add(item);
            }
            EventsDropDownList.DataBind();
        }


        private void CheckDates()
        {
            if (DatePeriod.SelectedIndex > -1)
            {
                DateTime from = DateTime.Now;
                DateTime to = DateTime.Now.AddMonths(12);
                switch (DatePeriod.SelectedValue)
                {
                    case "thisyear":
                        FromDateTime = new DateTime(DateTime.Now.Year, 1, 1);
                        ToDateTime = new DateTime(DateTime.Now.Year, 12, 31);
                        break;

                    case "lastyear":
                        FromDateTime = new DateTime(DateTime.Now.Year - 1, 1, 1);
                        ToDateTime = new DateTime(DateTime.Now.Year - 1, 12, 31);
                        break;
                    case "nextyear":
                        FromDateTime = new DateTime(DateTime.Now.Year + 1, 1, 1);
                        ToDateTime = new DateTime(DateTime.Now.Year + 1, 12, 31);
                        break;

                    case "lastmonth":
                        FromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                        ToDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                        break;

                    case "nextmonth":
                        FromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
                        ToDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1);
                        break;

                    case "thismonth":
                        FromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        ToDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
                        break;
                }
                Response.Redirect("Participants.aspx?fromdate=" + FromDateTime.ToShortDateString() + "&todate=" + ToDateTime.ToShortDateString());
                DatePeriod.SelectedIndex = 0;
                //DatePeriod.DataBind();
            }

        }

        protected void PopulateStatusDropDown()
        {
            IEnumerable<IParticipant> contents = ParticipantsList;
            contents = FilterEvent(contents);
            contents = FilterSearch(contents);
            contents = FilterEmail(contents);

            string selected = StatusFilterDropDown.SelectedValue;

            StatusFilterDropDown.Items.Clear();
            Array itemValues = System.Enum.GetValues(typeof(AttendStatus));
            Array itemNames = System.Enum.GetNames(typeof(AttendStatus));
            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                ListItem item = new ListItem(ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/attendstatus/" + itemValues.GetValue(i).ToString()), itemValues.GetValue(i).ToString());
            }

            StatusFilterDropDown.Items.Add(new ListItem(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/status"), ""));
            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                string status = itemNames.GetValue(i).ToString();
                int cnt = contents.Where(x => (x as IParticipant).AttendStatusText == status).Count();
                string text = ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/attendstatus/" + status);
                text = text + " (" + cnt.ToString() + ")";
                ListItem item = new ListItem(text, itemValues.GetValue(i).ToString());
                item.Selected = selected == item.Value;
                StatusFilterDropDown.Items.Add(item);

            }

            //StatusDropDown.DataBind();
            //StatusFilterDropDown.DataBind();

            StatusFilterDropDown.DataBind();
        }




        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

        }

        private void SetupPreviewPropertyControl(IEnumerable<IParticipant> contents)
        {
            int itemsPrPage = int.Parse(PagingPrPaging.SelectedValue);
            itemsPrPage = itemsPrPage == 0 ? contents.Count() : itemsPrPage;
            SetupPreviewPropertyControl(contents, GetIntSafe(PagingPage.Text), itemsPrPage, false);
        }


        protected void RemoveFiltersLinkButton_OnClick(object sender, EventArgs e)
        {
            EMailDropDownList.SelectedIndex = 0;
            StatusFilterDropDown.SelectedIndex = 0;
            SearchTextBox.Text = string.Empty;
            SetupPreviewPropertyControl(ParticipantsList, 1, int.Parse(PagingPrPaging.SelectedValue), true);


        }


        private void SetupPreviewPropertyControl(IEnumerable<IParticipant> contents, int page, int itemsPrPage, bool databind)
        {
            int total = contents.Count();

            contents = FilterStatus(contents);
            contents = FilterEmail(contents);
            contents = FilterSearch(contents);
            contents = FilterEvent(contents);

            int totalFiltered = contents.Count();
            int last = itemsPrPage * (page);
            if (last > totalFiltered)
                last = totalFiltered;
            int first = itemsPrPage * (page - 1) + 1;
            var filteredContent = contents.Skip(first - 1).Take(itemsPrPage);


            NumberOfParticipantsLiteral.Text = string.Format(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/findresult"), totalFiltered, total);

            PageResultLiteral.Text = string.Format(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/edit/pageresult"), page, first, last, totalFiltered);
            PageResultLiteral2.Text = PageResultLiteral.Text;

            ParticipantsRepeater.DataSource = filteredContent;

            //if (databind)
            ParticipantsRepeater.DataBind();

            PopulateEMailDropDown();
            PopulateEventDropDown();
            PopulateStatusDropDown();

        }

        protected List<string> GetFormFields(IParticipant participant)
        {
            List<string> fieldsList = new List<string>();
            foreach (ListItem checkBox in FormFieldsCheckBoxList.Items)
            {
                if (checkBox.Selected)
                    fieldsList.Add(AttendRegistrationEngine.GetParticipantInfo(participant, checkBox.Value) ?? "&nbsp;");

            }
            return fieldsList;
        }

        private IEnumerable<IParticipant> FilterStatus(IEnumerable<IParticipant> contents)
        {
            if (StatusFilterDropDown.SelectedIndex > 0)
                contents =
                    contents.Where(x => (x as IParticipant).AttendStatus == StatusFilterDropDown.SelectedValue);
            return contents;
        }

        private IEnumerable<IParticipant> FilterEmail(IEnumerable<IParticipant> contents)
        {
            if (EMailDropDownList.SelectedIndex > 0)
                contents =
                    contents.Where(x => (x as IParticipant).Email.Trim().EndsWith(EMailDropDownList.SelectedValue));


            return contents;
        }

        private IEnumerable<IParticipant> FilterEvent(IEnumerable<IParticipant> contents)
        {
            if (EventsDropDownList.SelectedIndex > 0)
                contents =
                    contents.Where(x => (x as IParticipant).EventPage.ID.ToString() == EventsDropDownList.SelectedValue);


            return contents;
        }

        private IEnumerable<IParticipant> FilterSearch(IEnumerable<IParticipant> contents)
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                contents = contents.Where(x => (x as IParticipant).XForm != null && (x as IParticipant).XForm.ToLower().Contains(SearchTextBox.Text.ToLower()));

            return contents;
        }

        protected void PagingPrevious_OnClick(object sender, EventArgs e)
        {
            int i = GetIntSafe(PagingPage.Text);
            if (i > 1)
                i = (i - 1);
            PagingPage.Text = i.ToString();
            SetupPreviewPropertyControl(ParticipantsList, i, int.Parse(PagingPrPaging.SelectedValue), true);
        }

        protected void PagingNext_OnClick(object sender, EventArgs e)
        {
            int i = GetIntSafe(PagingPage.Text);
            if ((double)i < (double)ParticipantsList.Count / double.Parse(PagingPrPaging.SelectedValue))
                i = (i + 1);
            PagingPage.Text = i.ToString();
            PagingPage.DataBind();
            SetupPreviewPropertyControl(ParticipantsList, i, int.Parse(PagingPrPaging.SelectedValue), true);
        }

        private int GetIntSafe(string text)
        {
            int i = 0;
            int.TryParse(text, out i);
            return i;
        }

        protected void PagingPrPaging_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PagingPage.Text = "1";
            //SetupPreviewPropertyControl(ParticipantsContentArea, Participants, int.Parse(PagingPage.Text), int.Parse(PagingPrPaging.SelectedValue), true);
        }

        protected void ExportButton_OnClick(object sender, EventArgs e)
        {
            IEnumerable<IParticipant> contents = ParticipantsList;
            contents = FilterStatus(contents);
            contents = FilterEmail(contents);
            contents = FilterSearch(contents);
            contents = FilterEvent(contents);

            List<string> fieldsList = "email;registrationcode;status;".Split(';').ToList<string>();
            foreach (ListItem checkBox in FormFieldsCheckBoxList.Items)
            {
                if (checkBox.Selected)
                    fieldsList.Add(checkBox.Value);

            }

            ParticipantExport.Export((from x in contents select x as IParticipant).ToList(), fieldsList);

        }


        protected void DatePeriod_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDates();
            //Response.Redirect("AllEvents.aspx?from="+from.ToString()+"&to="+to.ToString());

        }

        protected void ChangeDate_OnClick(object sender, EventArgs e)
        {
            FromDateTime = DateTime.Parse(TextBoxFromDate.Text);
            ToDateTime = DateTime.Parse(TextBoxToDate.Text);
            PopulateParticipants();

        }
        /*
        protected void EMailDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateEventDropDown();
            PopulateStatusDropDown();
            PopulateParticipants();
        }*/

        protected void EMailDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedEmail = (sender as DropDownList).SelectedValue;
        }

        /*
        protected void FormFieldsCheckBoxList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Participants.aspx");
        }*/
    }
}