using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Participant.TestProvider
{
    public class TestParticipant : IParticipant
    {
        public DateTime DateSubmitted { get; set; }

        public string Code
        {
            get;
            set;
        }

        public string Email { get; set; }

        public string XForm
        {
            get;
            set;
        }

        public EPiServer.Core.PageReference EventPage
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string SessionsString
        {
            get;
            set;
        }

        public string AttendStatus
        {
            get;
            set;
        }

        public string AttendStatusText
        {
            get { return "Statustext"; }
        }

        public string Log
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public EPiServer.Core.ContentArea Sessions
        {
            get;
            set;
        }
    }
}