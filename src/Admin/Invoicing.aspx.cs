using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Settings;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor.TinyMCE.Plugins;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Admin
{
    public partial class Invoicing : System.Web.UI.Page
    {
        public const string InvoiceFildsSettingName = "InvoiceFilds";
        public List<IParticipant> Participants { get; set; }

        protected DateTime ToDateTime
        {
            get { return DateTime.Parse(TextBoxToDate.Text); }
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

        protected void Page_Init(object sender, EventArgs e)
        {
            InvoiceFieldsTextBox.Text = Settings.GetSetting(InvoiceFildsSettingName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxToDate.Text))
                ToDateTime = DateTime.Now.AddMonths(12);
            if (string.IsNullOrEmpty(TextBoxFromDate.Text))
                FromDateTime = DateTime.Now;

            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            if (!string.IsNullOrEmpty(DatePeriod.SelectedValue))
            {
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
                DatePeriod.SelectedIndex = -1;
            }
            SearchEvents();
        }

        protected void SearchEvents()
        {
            var events = ParticipantProviderManager.Provider.GetEventPages();


            var upcomingEvents = (from PageData p in events orderby ((EventPageBase)p).EventDetails.EventStart where ((EventPageBase)p).EventDetails.EventStart >= FromDateTime && ((EventPageBase)p).EventDetails.EventEnd <= ToDateTime select p);
            List<IParticipant> participants = new List<IParticipant>();
            foreach (PageData EventPageBaseData in upcomingEvents)
            {
                 if ((EventPageBaseData as EventPageBase).EventDetails.EventEnd <= ToDateTime &&
                        (EventPageBaseData as EventPageBase).EventDetails.EventStart >= FromDateTime)
                    {
                        //ExtractFieldNames(EventPageBaseData as EventPageBase);
                        foreach (var participant in AttendRegistrationEngine.GetParticipants(EventPageBaseData.ContentLink))
                        {
                            participants.Add(participant as IParticipant);
                        }
                    }
            }
            Participants = participants;
            (AttendInvoiceList as Attend.Admin.Partials.InvoiceList).ParticipantList = participants;
            AttendInvoiceList.DataBind();

        }



        protected void ChangeDate_OnClick(object sender, EventArgs e)
        {
            SearchEvents();
        }


        protected void SaveSettingsButton_OnClick(object sender, EventArgs e)
        {
            Settings.AddSetting(InvoiceFildsSettingName,InvoiceFieldsTextBox.Text);
            SearchEvents();
        }

        protected void ExportButton_OnClick(object sender, EventArgs e)
        {
            ParticipantExport.Export(Participants, Settings.GetSetting(InvoiceFildsSettingName).Split(';').ToList<string>());
        }
    }
}