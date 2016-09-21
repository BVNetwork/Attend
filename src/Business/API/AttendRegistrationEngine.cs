using System.Drawing;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Web.Services.Description;
using System.Web.UI;
using BVNetwork.Attend.Business.Core;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Log;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Events.Clients;
using EPiServer.Framework.Localization;
using EPiServer.Personalization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.XForms;
using EPiServer.XForms.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace BVNetwork.Attend.Business.API
{
    public class AttendRegistrationEngine
    {

        public delegate void RegistrationEventHandler(object o, RegistrationEventArgs e);


        public static event RegistrationEventHandler BeforeRegisterParticipant;
        public static event RegistrationEventHandler AfterRegisterParticipant;
        
        protected static void BeforeRegisterParticipantHandler(RegistrationEventArgs e)
        {
            if (BeforeRegisterParticipant != null)
                BeforeRegisterParticipant(new object(), e);
        }

        protected static void AfterRegisterParticipantHandler(RegistrationEventArgs e)
        {
            if (AfterRegisterParticipant != null)
                AfterRegisterParticipant(new object(), e);
        }

        public static IParticipant GenerateParticipation(ContentReference EventPageBase, string email, bool sendMail, string xform, string logText)
        {
            if (Business.Email.Validation.IsEmail(email))
                return ParticipantProviderManager.Provider.GenerateParticipant(EventPageBase, email, sendMail, xform, logText);
            else
                throw new InvalidEmailException("Attend participant cannot be created without a valid e-mail address.");

        }

        public static IParticipant GenerateParticipation(ContentReference EventPageBase, string email, string xform)
        {
            return GenerateParticipation(EventPageBase, email, false, xform, "Generated participant from admin");
        }


        public static IParticipant GetParticipant(string email, string code, ContentReference EventPageBase)
        {
            return (from p in (GetParticipants(EventPageBase)) where p.Email == email && p.Code == code select p).First<IParticipant>();
        }

        public static IParticipant GetParticipant(string code)
        {
            return ParticipantProviderManager.Provider.GetParticipant(code);
        }

        public static IParticipant GetParticipant(string email, string code)
        {
            return ParticipantProviderManager.Provider.GetParticipant(email, code);
        }


        public static bool SendStatusMail(IParticipant participant)
        {
            return ParticipantProviderManager.Provider.SendStatusMail(participant);
        }

        public static bool RegistrationOpen(ContentReference EventPageBase)
        {
            EventPageBase currentEvent = (ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(EventPageBase));
            if (currentEvent.EventDetails.RegistrationOpen == DateTime.MinValue)
                return true;
            if (currentEvent.EventDetails.RegistrationOpen < DateTime.Now && (currentEvent.EventDetails.RegistrationClose > DateTime.Now || currentEvent.EventDetails.RegistrationClose == DateTime.MinValue))
                return true;
            return false;
        }

        public static IEnumerable<IParticipant> GetParticipants(ContentReference EventPageBase, AttendStatus status)
        {
            return (from p in GetParticipants(EventPageBase) where p.AttendStatus == status.ToString() select p);
        }

        public static IParticipant GetParticipant(int ParticipantID)
        {
            return ServiceLocator.Current.GetInstance<IContentRepository>().Get<ParticipantBlock>(new ContentReference(ParticipantID));
        }

        public static int GetNumberOfSeats(ContentReference EventPageBase)
        {
            return ParticipantProviderManager.Provider.GetNumberOfSeats(EventPageBase);
        }

        public static int GetNumberOfParticipants(ContentReference EventPageBase)
        {
            return ParticipantProviderManager.Provider.GetNumberOfParticipants(EventPageBase);
        }

        public static int GetTotalIncome(ContentReference EventPageBase)
        {
            return (from p in GetParticipants(EventPageBase) where p.AttendStatus == AttendStatus.Confirmed.ToString() select p.Price).Sum();
        }

        public static int GetAvailableSeats(ContentReference EventPageBase)
        {
            return ParticipantProviderManager.Provider.GetAvailableSeats(EventPageBase);
        }

        public static IEnumerable<IParticipant> GetParticipants(ContentReference EventPageBase)
        {
            return ParticipantProviderManager.Provider.GetParticipants(EventPageBase);
        }



        private static ContentFolder GetOrCreateParticipantsFolder(ContentReference EventPageBase)
        {

            return Folders.GetOrCreateEventFolder(EventPageBase, "Participants");

        }

        public static NameValueCollection GetFormData(IParticipant participant)
        {
            NameValueCollection _formControls = new NameValueCollection();
            if (participant.XForm != null)
            {
                SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                xmlDoc.LoadXml(participant.XForm);
                XFormData data = new XFormData();
                data.Data = xmlDoc;
                NameValueCollection formControls = data.GetValues();
                _formControls = formControls;

            }
            return _formControls;
        }

        public static string GetParticipantInfo(IParticipant participant, string propertyname)
        {
            if (participant == null)
                return string.Empty;
            {
                if (String.IsNullOrEmpty(propertyname))
                    return String.Empty;

                propertyname = propertyname.ToLower();

                switch (propertyname)
                {
                    case "editurl":
                        var internalUrl = UrlResolver.Current.GetUrl(participant.EventPage);

                        UrlBuilder relativeUrl = new UrlBuilder(internalUrl);
                        Global.UrlRewriteProvider.ConvertToExternal(relativeUrl, null, System.Text.Encoding.UTF8);

                        string url = UriSupport.AbsoluteUrlBySettings(relativeUrl.ToString());
                        url = EPiServer.UriSupport.AddQueryString(url, "code",
                            participant.Code);
                        url = EPiServer.UriSupport.AddQueryString(url, "email",
                            participant.Email);
                        return url;

                    case "username":
                        return participant.Username;

                    case "registrationcode":
                        return participant.Code;

                    case "email":
                        return participant.Email;

                    case "price":
                        return participant.Price.ToString();

                    case "eventname":
                    case "coursename":
                        return DataFactory.Instance.Get<EventPageBase>(participant.EventPage).Name;

                    case "submitted":
                        return participant.DateSubmitted.ToString("yyyy-MM-dd HH:mm");

                    case "status":
                        return participant.AttendStatus.ToString();

                    case "fullname":
                        return GetParticipantInfo(participant, "FirstName") + " " +
                               GetParticipantInfo(participant, "LastName");

                    case "datestring":
                        return GetEventDates(participant.EventPage);

                    case "coursedatestring":
                        return string.Format(LocalizationService.Current.GetString("/attend/diploma/datetext"), GetEventDates(participant.EventPage));

                    default:
                        {
                            try
                            {
                                SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                                xmlDoc.LoadXml(participant.XForm);

                                foreach (XmlNode formNode in xmlDoc.SelectNodes("instance/*"))
                                {
                                    if (propertyname == formNode.Name.ToLower())
                                        return formNode.InnerText;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        break;
                }

                return String.Empty;
            }
        }

        public static EventPageBase GetEventPageBase(IParticipant participant)
        {
            if (participant.EventPage != null)
            {
                EventPageBase EventPage =
                    ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(participant.EventPage);
                return EventPage;
            }
            return null;
        }

        public static string GetEventDates(ContentReference currentEvent)
        {
            EventPageBase EventPageBase =
            ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(currentEvent);
            if (EventPageBase != null)
            {
                // One day event
                if (EventPageBase.EventDetails.EventEnd.ToShortDateString() ==
                    EventPageBase.EventDetails.EventStart.ToShortDateString())
                    return EventPageBase.EventDetails.EventStart.ToString("dd. MMMM yyyy");
                // Multiple days, same month
                if (EventPageBase.EventDetails.EventStart.Month == EventPageBase.EventDetails.EventEnd.Month)
                    return EventPageBase.EventDetails.EventStart.ToString("d. - ") + EventPageBase.EventDetails.EventEnd.ToString("d. MMMM yyyy");
                if (EventPageBase.EventDetails.EventStart.Month == EventPageBase.EventDetails.EventEnd.Month)
                    return EventPageBase.EventDetails.EventStart.ToString("d. MMMM - ") + EventPageBase.EventDetails.EventEnd.ToString("d. MMMM yyyy");
            }
            return string.Empty;
        }


        public static List<IParticipant> GetOrCreateParticipantsClipboard()
        {
            List<IParticipant> participants;
            if (HttpContext.Current.Session["Participants"] == null)
                HttpContext.Current.Session["Participants"] = new List<IParticipant>();
            participants = HttpContext.Current.Session["Participants"] as List<IParticipant>;
            return participants;
        }

        public static bool IsChecked(string Code)
        {
            return (from x in GetOrCreateParticipantsClipboard() where (x as IParticipant).Code == Code select x)
                .Any();
        }

        public static void CopyParticipants(IParticipant p)
        {
            List<IParticipant> participants = GetOrCreateParticipantsClipboard();
            participants.Add(p);
            HttpContext.Current.Session["Participants"] = participants;
        }

        public static void CopyParticipantsRemove(IParticipant p)
        {
            List<IParticipant> participants = GetOrCreateParticipantsClipboard();
            participants = (from x in participants where (x as IParticipant).Code != (p as IParticipant).Code select x).ToList();
            HttpContext.Current.Session["Participants"] = participants;

        }

        public static void CopyParticipantsRemoveAll()
        {
            HttpContext.Current.Session["Participants"] = null;
        }

        public static void CopyParticipantsChangeStatus(string newStatus, bool sendMail)
        {
            List<IParticipant> participants = GetOrCreateParticipantsClipboard();
            foreach (IParticipant participantBlock in participants)
            {
                var participant = (participantBlock as ParticipantBlock).CreateWritableClone() as ParticipantBlock;
                participant.AttendStatus = newStatus;
                ServiceLocator.Current.GetInstance<IContentRepository>().Save(participant as IContent, SaveAction.Publish);
                if (sendMail)
                    SendStatusMail(participant);
            }
        }


        public static void PasteParticipantsCopy(EventPageBase destination)
        {
            ///TODO: Rewrite
            /*
            foreach (ParticipantBlock participantBlock in GetOrCreateParticipantsClipboard())
            {

                var newParticipant = ServiceLocator.Current.GetInstance<IContentRepository>()
                    .Copy((participantBlock as IContent).ContentLink, GetOrCreateParticipantsFolder(destination.ContentLink).ContentLink, AccessLevel.NoAccess, AccessLevel.NoAccess, true);
                var participant = ServiceLocator.Current.GetInstance<IContentRepository>().Get<ParticipantBlock>(newParticipant).CreateWritableClone() as ParticipantBlock;
                participant.Code = GenerateCode();
                participant.Comment = participant.Comment + "Copied from " + participantBlock.EventPageBase.ID;
                participant.EventPageBase = (destination as IContent).ContentLink.ToPageReference();
                Log.ParticipantLog.AddLogText("Copy", "Copied from " + participantBlock.EventPageBase.ID + " to " + destination.ContentLink.ID, participant);
                ServiceLocator.Current.GetInstance<IContentRepository>()
                    .Save(participant as IContent, SaveAction.Publish);
            }
            CopyParticipantsRemoveAll();*/
        }

        public static void PasteParticipantsMove(EventPageBase destination)
        {
            foreach (IParticipant participantBlock in GetOrCreateParticipantsClipboard())
            {
                var participant = (participantBlock as ParticipantBlock).CreateWritableClone() as ParticipantBlock;
                participant.EventPage = (destination as IContent).ContentLink.ToPageReference();
                ServiceLocator.Current.GetInstance<IContentRepository>()
                    .Save(participant as IContent, SaveAction.Publish);
                Log.ParticipantLog.AddLogText("Move", "Moved from " + participantBlock.EventPage.ID + " to " + destination.ContentLink.ID, participant);

                ServiceLocator.Current.GetInstance<IContentRepository>()
                    .Move((participantBlock as IContent).ContentLink, GetOrCreateParticipantsFolder(destination.ContentLink).ContentLink, AccessLevel.NoAccess, AccessLevel.NoAccess);
            }
        }

        public static string GetAvailableSeatsText(EventPageBase EventPageBase)
        {
            int available = GetAvailableSeats(EventPageBase.ContentLink);
            if (available > 3 && !string.IsNullOrEmpty(EventPageBase.AvailableSeatsText.ManyAvailableSeats))
                return string.Format(EventPageBase.AvailableSeatsText.ManyAvailableSeats, available);
            if (available == 3)
                return EventPageBase.AvailableSeatsText.ThreeSeats;
            if (available == 2)
                return EventPageBase.AvailableSeatsText.TwoSeats;
            if (available == 1)
                return EventPageBase.AvailableSeatsText.OneSeat;
            return string.Empty;
        }

        public static void SaveParticipant(IParticipant participant) {
            ParticipantProviderManager.Provider.SaveParticipant(participant);
        }

        public static bool UseForms { get { return BVNetwork.Attend.Business.Settings.Settings.GetSetting("UseEpiserverForms").ToString() == true.ToString(); } }
    }
}