using System.Globalization;
using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor.TinyMCE.Plugins;
using EPiServer.Filters;
using EPiServer.Personalization.VisitorGroups.Criteria;
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
    public partial class Events : System.Web.UI.Page
    {

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

        protected override void OnPreInit(EventArgs e)
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxToDate.Text))
                ToDateTime = DateTime.Now.AddMonths(12);
            if (string.IsNullOrEmpty(TextBoxFromDate.Text))
                FromDateTime = DateTime.Now;

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
                        FromDateTime = new DateTime(DateTime.Now.Year +1, 1, 1);
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
            var allEvents = ParticipantProviderManager.Provider.GetEventPages();

            var upcomingEvents = (from PageData p in allEvents orderby ((EventPageBase)p).EventDetails.EventStart where ((EventPageBase)p).EventDetails.EventStart >= FromDateTime && ((EventPageBase)p).EventDetails.EventEnd <= ToDateTime select p);

            (AttendParticipantList as ParticipantList).SetEventPageBaseList(upcomingEvents);
            //AttendParticipantList.DataBind();

        }

        protected void ChangeDate_OnClick(object sender, EventArgs e)
        {
           SearchEvents();
        }
    }
}