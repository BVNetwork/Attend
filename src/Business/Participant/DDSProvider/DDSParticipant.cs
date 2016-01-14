using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace BVNetwork.Attend.Business.Participant.DDSProvider
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class DDSParticipant : IParticipant
    {
        public DDSParticipant()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }

        public Identity Id { get; set; }

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
            get { return new PageReference(EventPageID); }
            set { EventPageID = value.ID; }
        }

        public int EventPageID { get; set; }

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