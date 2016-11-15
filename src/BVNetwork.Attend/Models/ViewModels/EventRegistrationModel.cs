using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Core;
using EPiServer.XForms;
using EPiServer.XForms.Util;
using System.Web.Mvc;

namespace BVNetwork.Attend.Models.ViewModels
{
    public class EventRegistrationModel : AttendPageViewModel<EventPageBase>
    {
        public EventRegistrationModel(EventPageBase currentPage)
            : base(currentPage)
        {

        }
        public EventPageBase EventPageBase { get; set; }
        public XFormPostedData PostedData { get; set; }
        public ViewDataDictionary ViewData { get; set; }
        public string Controller { get; set; }
        public string ViewDataKey { get; set; }
        public List<AttendSessionEngine.Session> Sessions { get; set; }
        public int AvailableSeats { get; set; }
        public string PriceText { get; set; }
        public string EventCategory { get; set; }
        public bool Submitted { get; set; }
        public ParticipantBlock CurrentParticipant { get; set; }
        public PageData HostPageData { get; set; }
        public string ActionUrl { get; set; }
        public List<string> Messages { get; set; }
    }
}