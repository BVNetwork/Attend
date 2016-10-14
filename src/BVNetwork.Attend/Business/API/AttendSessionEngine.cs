using System.Collections;
using System.Drawing;
using System.EnterpriseServices.Internal;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sgml;

namespace BVNetwork.Attend.Business.API
{
    public class AttendSessionEngine
    {

        public static SessionBlock GenerateSession(EventPageBase EventPageBase, string name, DateTime start, DateTime end)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            SessionBlock newSession = contentRepository.GetDefault<SessionBlock>(GetOrCreateSessionFolder(EventPageBase.ContentLink).ContentLink);
            newSession.Start = start;
            newSession.End = end;
            newSession.NumberOfSeats = EventPageBase.EventDetails.NumberOfSeats;
            newSession.EventPage = EventPageBase.ContentLink.ToPageReference();
            IContent newSessionContent = newSession as IContent;
            newSessionContent.Name = name;
            contentRepository.Save(newSessionContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            return newSession;
        }

        public static SessionBlock GenerateSession(EventPageBase EventPageBase)
        {
            return GenerateSession(EventPageBase, "New Session", EventPageBase.EventDetails.EventStart, EventPageBase.EventDetails.EventEnd);
        }

        public static List<SessionBlock> GetSessions(ContentReference EventPageBase)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            List<SessionBlock> sessions = contentRepository.GetChildren<SessionBlock>(GetOrCreateSessionFolder(EventPageBase).ContentLink, new NullLanguageSelector()).ToList<SessionBlock>();
            sessions.Sort(delegate(SessionBlock s1, SessionBlock s2) { return s1.Start.CompareTo(s2.Start); });
            return sessions;
        }

        public sealed class Session
        {
            public bool Enabled;
            public bool Selected;
            public string Name;
            public bool NewGroup;
            public string Group;
            public bool CheckBox;
            public int ContentID;
            public string Header;
            public string IntroContent;
            public SessionBlock CurrentSessionBlock;
        }


        public static List<Session> GetSessionsList(ContentReference EventPageBase)
        {
            return GetSessionsList(EventPageBase, "HH.mm - ");
        }

        public enum ControlType
        {
            CheckBox, RadioButton, DropDownList
        }

        public static List<Session> GetSessionsList(ContentReference EventPageBase, string dateFormat)
        {
            var sessionBlocks = GetSessions(EventPageBase);
            var groupedSessions = (from SessionBlock s in sessionBlocks where string.IsNullOrEmpty(s.TrackID) select s).ToList();
            var scheduledSessions = (from SessionBlock s in sessionBlocks where !string.IsNullOrEmpty(s.TrackID) select s).ToList();

            List<Session> sessions = new List<Session>();

            ConvertSessionBlocksToSessions(groupedSessions, sessions);
            ConvertSessionBlocksToSessions(scheduledSessions, sessions);

            return sessions;
        }

        public static void ConvertSessionBlocksToSessions(List<SessionBlock> sessionBlocks, List<Session> sessions)
        {
            string groupName = "";
            string groupID = "";
            for (int i = 0; i < sessionBlocks.Count; i++)
            {
                SessionBlock currentBlock = sessionBlocks[i];

                bool nextSessionIsConcurrent;
                bool previousSessionIsConcurrent;

                if (string.IsNullOrEmpty(currentBlock.TrackID))
                {
                    nextSessionIsConcurrent = (i < sessionBlocks.Count - 1) &&
                                              sessionBlocks[i + 1].Start == sessionBlocks[i].Start;
                    previousSessionIsConcurrent = (i > 0) && sessionBlocks[i - 1].Start == sessionBlocks[i].Start;
                }
                else
                {
                    nextSessionIsConcurrent = (i < sessionBlocks.Count - 1) &&
                                              sessionBlocks[i + 1].TrackID == sessionBlocks[i].TrackID;
                    previousSessionIsConcurrent = (i > 0) && sessionBlocks[i - 1].TrackID == sessionBlocks[i].TrackID;
                }
                if (!previousSessionIsConcurrent || currentBlock.Mandatory)
                {
                    groupID = "Session-" + (currentBlock as IContent).ContentLink.ID;
                    groupName = currentBlock.TrackID;
                }
                sessions.Add(GenerateSession(sessionBlocks[i], groupName, groupID, nextSessionIsConcurrent, previousSessionIsConcurrent));
            }



        }



        public static Session GenerateSession(SessionBlock currentSession, string groupName, string groupID, bool nextSessionIsConcurrent, bool previousSessionIsConcurrent)
        {
            Session newSession = new Session();
            newSession.CurrentSessionBlock = currentSession;

            newSession.Name = (currentSession as IContent).Name;
            newSession.Enabled = currentSession.NumberOfSeats == 0 || (currentSession.NumberOfSeats - GetParticipants(currentSession).Count) > 0;
            newSession.Selected = currentSession.Mandatory;
            newSession.NewGroup = !previousSessionIsConcurrent;

            if (!previousSessionIsConcurrent)
            {
                if (string.IsNullOrEmpty(groupName))
                {
                    newSession.Header = currentSession.Start.ToString("HH:mm");
                }
                else
                {
                    newSession.Header = groupName;
                }
            }
            newSession.Group = groupID;

            if (currentSession.IntroContent != null)
                newSession.IntroContent = currentSession.IntroContent.ToHtmlString();

            newSession.CheckBox = !nextSessionIsConcurrent && !previousSessionIsConcurrent && !currentSession.Mandatory;
            newSession.ContentID = (currentSession as IContent).ContentLink.ID;
            return newSession;

        }

        public static List<ParticipantBlock> GetParticipants(SessionBlock session)
        {
            List<ParticipantBlock> sessionParticipants = new List<ParticipantBlock>();
            int sessionID = (session as IContent).ContentLink.ID;
            var eventParticipants = AttendRegistrationEngine.GetParticipants(session.EventPage);
            foreach (ParticipantBlock participant in eventParticipants)
            {
                if (participant.Sessions != null && participant.Sessions.Items != null && participant.Sessions.Items.Count > 0)
                    if ((from s in participant.Sessions.Items where s.ContentLink.ID == sessionID select s.ContentLink.ID).Any<int>())
                        sessionParticipants.Add(participant);
            }
            return sessionParticipants;
        }

        private static ContentFolder GetOrCreateSessionFolder(ContentReference EventPageBase)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();

            ContentFolder participantsFolder = null;
            var assetsFolder = contentAssetHelper.GetOrCreateAssetFolder(EventPageBase);

            var folders = contentRepository.GetChildren<ContentFolder>(assetsFolder.ContentLink);

            foreach (ContentFolder cf in folders)
            {
                if (cf.Name == "Sessions")
                    participantsFolder = cf;
            }

            if (participantsFolder == null)
            {
                participantsFolder = contentRepository.GetDefault<ContentFolder>(assetsFolder.ContentLink);
                participantsFolder.Name = "Sessions";
                contentRepository.Save(participantsFolder, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            }

            return participantsFolder;
        }

        public static Control GetSessionsControl(ContentReference currentEvent, ParticipantBlock participant)
        {
            var sessions = AttendSessionEngine.GetSessionsList(currentEvent);
            PlaceHolder ph = new PlaceHolder();

            foreach (AttendSessionEngine.Session session in sessions)
            {
                if (session.CheckBox)
                {

                    var cb = new CheckBox() { Enabled = session.Enabled, Checked = (from ContentAreaItem item in participant.Sessions.Items where item.ContentLink.ID == session.ContentID select item).Any() };
                    cb.InputAttributes.Add("value", session.ContentID.ToString());
                    cb.InputAttributes.Add("name", session.Group);
                    ph.Controls.Add(cb);
                    ph.Controls.Add(new LiteralControl(session.Name + "<br/>"));
                }
                else
                {
                    if (session.NewGroup)
                        ph.Controls.Add(new LiteralControl("<br/>"));
                    var li = new RadioButton() { Enabled = session.Enabled, Checked = (from ContentAreaItem item in participant.Sessions.Items where item.ContentLink.ID == session.ContentID select item).Any(), GroupName = session.Group };
                    li.InputAttributes.Add("value", session.ContentID.ToString());
                    li.InputAttributes.Add("name", session.Group);
                    ph.Controls.Add(li);
                    ph.Controls.Add(new LiteralControl(session.Name + "<br/>"));
                }
            }
            return ph;
        }

        public static bool HasParallelSessions(Session session, List<AttendSessionEngine.Session> sessionsList)
        {
            return sessionsList.FindAll(x => x.Group == session.Group).Count > 1;
        }

    }
}