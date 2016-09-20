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
    public partial class ScheduledEmails : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<EventPageBase> AllEvents = AttendScheduledEmailEngine.GetEventPageBases();

            List<ScheduledEmailBlock> upcoming = new List<ScheduledEmailBlock>();
            List<ScheduledEmailBlock> sendNow = new List<ScheduledEmailBlock>();

            foreach (EventPageBase eventPage in AllEvents)
            {
                upcoming.AddRange(AttendScheduledEmailEngine.GetScheduledEmailsToSend(eventPage, DateTime.Now, DateTime.MaxValue));
                sendNow.AddRange(AttendScheduledEmailEngine.GetScheduledEmailsToSend(eventPage, DateTime.MinValue, DateTime.Now));
            }
            (UpcomingControl as EmailList).Messages = (from x in upcoming orderby x.SendDateTime ascending select x).ToList<ScheduledEmailBlock>();
            (SendNowControl as EmailList).Messages = (from x in sendNow orderby x.SendDateTime ascending select x).ToList<ScheduledEmailBlock>();
            UpcomingControl.DataBind();
            SendNowControl.DataBind();

        }



    }
}