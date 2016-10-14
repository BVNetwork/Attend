using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace BVNetwork.Attend.Admin.Partials
{
    public partial class ParticipantList : System.Web.UI.UserControl
    {
        private int _invoice;
        private int _count;
        protected int TotalIncome { get; set; }
        protected int TotalCount { get; set; }

        public PageDataCollection EventList
        {
            get
            {
                return (EventsDetailsRepeater.DataSource as PageDataCollection);
            }
            set
            {
                EventsDetailsRepeater.DataSource = value;
                EventsOverviewRepeater.DataSource = value;
            }
        }

        public void SetEventPageBaseList(IEnumerable<PageData> events)
        {
            EventsDetailsRepeater.DataSource = events;
            EventsOverviewRepeater.DataSource = events;
            DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string GetStatus(PageData EventPageBase)
        {
            return ServiceLocator.Current.GetInstance<LocalizationService>().GetString("/attend/eventstatus/" + GetClass(EventPageBase));

        }

        protected string GetClass(PageData EventPageBase)
        {
            if ((EventPageBase as EventPageBase).EventDetails.Cancelled == true)
                return "event-cancelled";
            if ((EventPageBase as EventPageBase).EventDetails.Private)
                return "event-private";
            int participants = AttendRegistrationEngine.GetNumberOfParticipants(EventPageBase.ContentLink);
            if (participants == 0)
                return "event-empty";
            int seats = AttendRegistrationEngine.GetNumberOfSeats(EventPageBase.ContentLink);
            if (seats > 0)
            {
                if (participants >= seats)
                    return "event-full";
                if (((double)participants / (double)seats) > 0.7)
                    return "event-many";
                if (((double)participants / (double)seats) > 0.4)
                    return "event-some";
                if (((double)participants / (double)seats) > 0)
                    return "event-few";

            }
            return "event-unknown";
        }

        private void SetupPreviewPropertyControl(Property propertyControl, IContent contents)
        {
            var contentArea = new ContentArea();
           
                contentArea.Items.Add(new ContentAreaItem { ContentLink = (contents).ContentLink });
           
            var previewProperty = new PropertyContentArea { Value = contentArea, Name = "PreviewPropertyData", IsLanguageSpecific = true };

            propertyControl.InnerProperty = previewProperty;
            propertyControl.DataBind();

        }

        protected void previewRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            var propertyControl = e.Item.FindControl("Participants") as Repeater;
           
            EventPageBase EventPageBase = e.Item.DataItem as EventPageBase;
            if (EventPageBase != null) { 
                propertyControl.DataSource = AttendRegistrationEngine.GetParticipants(EventPageBase.ContentLink).ToList();
                propertyControl.DataBind();

                //var eventDetailsControl = e.Item.FindControl("EventInfoProperty") as Property;

                //SetupPreviewPropertyControl(eventDetailsControl, EventPageBase);
            }

           

            //SetupPreviewPropertyControl(propertyControl, AttendRegistrationEngine.GetParticipants(EventPageBase.ContentLink).ToList());
        }

        protected int CalculateIncome(PageReference EventPageBase)
        {
            int income = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetTotalIncome(EventPageBase);
            TotalIncome += income;
            return income;
        }

        protected int CalculateParticipants(PageReference EventPageBase)
        {
            int count = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfParticipants(EventPageBase);
            TotalCount += count;
            return count;
        }

        protected string GetProgressBar(EventPageBase EventPageBase)
        {
            if (EventPageBase.EventDetails.Cancelled == true)
                return "<div class='progress-bar progress-bar-danger' style='width:100%;'></div>";

            int numberOfSeats =
                BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfSeats(EventPageBase.PageLink);
            int availableSeats = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetAvailableSeats(EventPageBase.PageLink);

            if (availableSeats == 0)
                return "<div class='progress-bar progress-bar-info' style='width:100%;'></div>";

            return
                string.Format("<div class='progress-bar' style='width:{0}%;'><div class='pull-right'>{1}&nbsp;</span></div>", Math.Round(((double)(numberOfSeats - availableSeats) / (double)numberOfSeats) * 100), numberOfSeats - availableSeats);

        }

        protected void ExportParticipantsButton_OnClick(object sender, CommandEventArgs e)
        {
            EventPageBase EventPageBase = DataFactory.Instance.Get<EventPageBase>(new ContentReference(e.CommandArgument.ToString()));
             
            ParticipantExport.Export(AttendRegistrationEngine.GetParticipants(EventPageBase.ContentLink).ToList());
        }
    }
}