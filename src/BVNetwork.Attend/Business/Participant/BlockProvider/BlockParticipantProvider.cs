using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BVNetwork.Attend.Business.Core;
using BVNetwork.Attend.Business.Log;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor.TinyMCE;
using EPiServer.Filters;
using EPiServer.Personalization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace BVNetwork.Attend.Business.Participant.BlockProvider
{
    public class BlockParticipantProvider : ParticipantProviderBase
    {
        public override IParticipant GenerateParticipant(ContentReference EventPageBase, string email, bool sendMail, string xform, string logText)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            EventPageBase EventPageBaseData = contentRepository.Get<EventPageBase>(EventPageBase);

            ContentFolder participantsFolder = GetOrCreateParticipantsFolder(EventPageBase);

            ParticipantBlock newParticipant = contentRepository.GetDefault<ParticipantBlock>(participantsFolder.ContentLink);
            (newParticipant as IContent).Name = email;
            newParticipant.Code = GenerateCode();
            newParticipant.XForm = xform;
            newParticipant.EventPage = EventPageBase as PageReference;
            newParticipant.Email = email;
            newParticipant.AttendStatus = (GetAvailableSeats(EventPageBase) > 0) ? AttendStatus.Confirmed.ToString() : AttendStatus.Submitted.ToString();
            newParticipant.Price = EventPageBaseData.EventDetails.Price;
            newParticipant.Username = EPiServerProfile.Current.UserName;
            newParticipant.DateSubmitted = DateTime.Now;

            ParticipantEventArgs e1 = new ParticipantEventArgs();
            e1.CurrentParticipant = newParticipant;
            e1.CancelEvent = false;
            e1.SendMail = sendMail;
            RaiseOnAddingParticipant(e1);

            if (e1.CancelEvent == true)
                return null;

            contentRepository.Save(newParticipant as IContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            newParticipant = ParticipantLog.AddLogTextAndSave("Generated", logText, newParticipant as ParticipantBlock) as ParticipantBlock;
            newParticipant = ParticipantLog.AddLogTextAndSave("Status", "Status set to " + newParticipant.AttendStatus, newParticipant as IParticipant) as ParticipantBlock;

            sendMail = e1.SendMail;

            ParticipantEventArgs e2 = new ParticipantEventArgs();
            e2.CurrentParticipant = newParticipant;
            e2.CancelEvent = false;
            e2.SendMail = sendMail;
            RaiseOnAddedParticipant(e2);

            sendMail = e1.SendMail;

            if (sendMail)
                SendStatusMail(newParticipant);




            return newParticipant;
        }

        private static ContentFolder GetOrCreateParticipantsFolder(ContentReference EventPageBase)
        {

            return Folders.GetOrCreateEventFolder(EventPageBase, "Participants");

        }

        private static string GenerateCode()
        {
            Random rnd = new Random();
            StringBuilder newCode = new StringBuilder();
            char[] allowedCharacters = new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'R', 'S', 'T', 'V', 'W', 'X', 'Z', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 10; i++)
            {
                newCode.Append(allowedCharacters[rnd.Next(0, 25)]);
            }
            return newCode.ToString();
        }


        public override IEnumerable<IParticipant> GetParticipants(ContentReference EventPageBase)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            return contentRepository.GetChildren<ParticipantBlock>(GetOrCreateParticipantsFolder(EventPageBase).ContentLink, new CultureInfo(Localization.LanguageHelper.MasterLanguage(EventPageBase)));
        }

        public override List<EventPageBase> GetEventPages()
        {
            PropertyCriteriaCollection criteria = new PropertyCriteriaCollection();

            var pageTypes = ServiceLocator.Current.GetInstance<IContentTypeRepository>().List();
            foreach (var pageType in pageTypes)
            {
                if (pageType.ModelType != null && typeof(EventPageBase).IsAssignableFrom(pageType.ModelType))
                {
                    PropertyCriteria pageTypeCriteria = new PropertyCriteria();
                    pageTypeCriteria.Condition = CompareCondition.Equal;
                    pageTypeCriteria.Value = pageType.ID.ToString();
                    pageTypeCriteria.Type = PropertyDataType.PageType;
                    pageTypeCriteria.Name = "PageTypeID";
                    pageTypeCriteria.Required = false;
                    criteria.Add(pageTypeCriteria);
                }
            }

            List<EventPageBase> eventList = new List<EventPageBase>();
            var sites = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.SiteDefinitionRepository>().List();
            foreach (SiteDefinition siteDefinition in sites)
            {

                PageDataCollection allEvents = new PageDataCollection();

                var allLanguages = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>().ListEnabled();

                foreach (LanguageBranch languageBranch in allLanguages)
                {
                    allEvents.Add(DataFactory.Instance.FindPagesWithCriteria(siteDefinition.StartPage.ToPageReference(), criteria, languageBranch.LanguageID));
                }

                foreach (PageData currentEvent in allEvents)
                {
                    if (currentEvent as EventPageBase != null)
                        eventList.Add(currentEvent as EventPageBase);
                }
            }
            return eventList;

        }

        public override string GetEditUrl(IParticipant participant)
        {
            return EPiServer.Editor.PageEditing.GetEditUrl((participant as EPiServer.Core.IContent).ContentLink);
        }

        public override void SaveParticipant(IParticipant participant)
        {
            ServiceLocator.Current.GetInstance<IContentRepository>().Save(participant as EPiServer.Core.IContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }

        public override IParticipant GetParticipant(string code)
        {
            /// TODO: Must be rewritten. Loops all participantblocks to find participant.
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();
            var myblockType = contentTypeRepository.Load<ParticipantBlock>();
            List<ContentReference> myblockTypeReferences = contentModelUsage.ListContentOfContentType(myblockType).Select(x => x.ContentLink.ToReferenceWithoutVersion()).ToList();
            foreach (ContentReference cref in myblockTypeReferences)
            {
                ParticipantBlock participant;
                repository.TryGet<ParticipantBlock>(cref, out participant);
                if (participant != null)
                    if (participant.Code == code)
                        return participant.CreateWritableClone() as IParticipant;
            }
            return null;
        }

        public override IParticipant GetParticipant(string email, string code)
        {
            /// TODO: Must be rewritten. Loops all participantblocks to find participant.
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();
            var myblockType = contentTypeRepository.Load<ParticipantBlock>();
            List<ContentReference> myblockTypeReferences = contentModelUsage.ListContentOfContentType(myblockType).Select(x => x.ContentLink.ToReferenceWithoutVersion()).ToList();
            foreach (ContentReference cref in myblockTypeReferences)
            {
                ParticipantBlock participant;
                repository.TryGet<ParticipantBlock>(cref, out participant);
                if (participant != null)
                    if (participant.Code == code && participant.Email == email)
                        return participant.CreateWritableClone() as IParticipant;
            }
            return null;
        }

    }
}