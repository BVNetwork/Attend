using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Log;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Personalization;
using EPiServer.ServiceLocation;

namespace BVNetwork.Attend.Business.Participant
{
    public abstract class ParticipantProviderBase : ProviderBase
    {

        protected string _name;

        public override string Name
        {
            get { return ProviderName; }
        }

        public string ProviderName { get; set; }

        protected string _description;

        public override string Description
        {
            get { return _description; }
        }

        public delegate void ParticipantEventHandler(object sender, ParticipantEventArgs e);

        public static event ParticipantEventHandler OnDeletedParticipant;
        public static event ParticipantEventHandler OnDeletingParticipant;
        public static event ParticipantEventHandler OnAddingParticipant;
        public static event ParticipantEventHandler OnAddedParticipant;

        public void RaiseOnDeletedParticipant(ParticipantEventArgs e)
        {
            if (OnDeletedParticipant != null)
                OnDeletedParticipant(null, e);
        }

        public void RaiseOnDeletingParticipant(ParticipantEventArgs e)
        {
            if (OnDeletingParticipant != null)
                OnDeletingParticipant(null, e);
        }

        public void RaiseOnAddingParticipant(ParticipantEventArgs e)
        {
            if (OnAddingParticipant != null)
                OnAddingParticipant(null, e);
        }

        public void RaiseOnAddedParticipant(ParticipantEventArgs e)
        {
            if (OnAddedParticipant != null)
                OnAddedParticipant(null, e);
        }

        public bool SendStatusMail(IParticipant participant)
        {
            AttendStatus status = AttendStatus.Undefined;
            EventPageBase CurrentEvent = EPiServer.DataFactory.Instance.Get<EventPageBase>(participant.EventPage);
            Enum.TryParse<AttendStatus>(participant.AttendStatus, out status);
            foreach (ScheduledEmailBlock scheduledEmailBlock in AttendScheduledEmailEngine.GetScheduledEmails(CurrentEvent.ContentLink, SendOptions.Action, status))
            {
                AttendScheduledEmailEngine.SendScheduledEmail(scheduledEmailBlock, participant);
            }
            return true;
        }

        public int GetNumberOfSeats(ContentReference EventPage)
        {
            return (ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(EventPage)).EventDetails.NumberOfSeats;
        }

        public int GetNumberOfParticipants(ContentReference EventPage)
        {
            return (from p in GetParticipants(EventPage) where p.AttendStatus == AttendStatus.Confirmed.ToString() || p.AttendStatus == AttendStatus.Participated.ToString()  select p).Count<IParticipant>();
        }

        public int GetAvailableSeats(ContentReference EventPageBase)
        {
            int available = GetNumberOfSeats(EventPageBase) - GetNumberOfParticipants(EventPageBase);
            if (available < 0)
                available = 0;
            return available;
        }



        public abstract IParticipant GenerateParticipant(ContentReference EventPageBase, string email, bool sendMail,
            string xform, string logText);

        public abstract IEnumerable<IParticipant> GetParticipants(ContentReference EventPageBase);

        public abstract IParticipant GetParticipant(string code);

        public abstract IParticipant GetParticipant(string email, string code);

        public abstract List<IParticipant> GetParticipantByEmail(string email);

        public abstract List<EventPageBase> GetEventPages();

        public abstract string GetEditUrl(IParticipant participant);

        public abstract void SaveParticipant(IParticipant participant);

    }
}