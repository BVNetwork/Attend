using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Extensions
{
    public static class AttendExtensions
    {
        public static EventPage EventPageData(this ParticipantBlock participantBlock)
        {
            var contentLoader = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentLoader>();
            return contentLoader.Get<EventPage>(participantBlock.EventPage);
        }

        public static string StatusCssClass(this ParticipantBlock participantBlock)
        {
            if (participantBlock.AttendStatus == AttendStatus.Confirmed.ToString())
                return "panel-success";
            if (participantBlock.AttendStatus == AttendStatus.Submitted.ToString() || participantBlock.AttendStatus == AttendStatus.Standby.ToString())
                return "panel-warning";
            if (participantBlock.AttendStatus == AttendStatus.Cancelled.ToString() || participantBlock.AttendStatus == AttendStatus.Deleted.ToString())
                return "panel-danger";
            return "";
        }

        public static string FriendlyDateOfEvent(this EventPage eventPage)
        {
            if (eventPage.EventDetails.EventStart.Date == eventPage.EventDetails.EventEnd.Date)
                return string.Format("{0:d. MMMM}", eventPage.EventDetails.EventStart);
            return eventPage.EventDetails.EventStart.ToString("M") + " - " + eventPage.EventDetails.EventEnd.ToString("M").Replace(" ", "\u00a0");
        }


    }
}