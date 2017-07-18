using System.Collections;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BVNetwork.Attend.Business.API
{
    public class AttendScheduledEmailEngine
    {

        public static List<EventPageBase> GetEventPageBases()
        {
            return ParticipantProviderManager.Provider.GetEventPages();
        }

        private static ContentFolder GetOrCreateEmailFolder(ContentReference EventPageBase)
        {
            return Core.Folders.GetOrCreateEventFolder(EventPageBase, "emails");
        }

        public static IEnumerable<ScheduledEmailBlock> GetAllEmails(ContentReference EventPageBase)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            return contentRepository.GetChildren<ScheduledEmailBlock>(GetOrCreateEmailFolder(EventPageBase).ContentLink, new CultureInfo(Localization.LanguageHelper.MasterLanguage(EventPageBase)));
        }

        public static IEnumerable<ScheduledEmailBlock> GetScheduledEmails(ContentReference EventPageBase, SendOptions option, AttendStatus status)
        {
            return (from e in GetAllEmails(EventPageBase) where e.SendOnStatus == status && e.EmailSendOptions == option select e);
        }

        public static ScheduledEmailBlock GenerateScheduledEmailBlock(ContentReference EventPageBase)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            ScheduledEmailBlock emailBlock = contentRepository.GetDefault<ScheduledEmailBlock>(GetOrCreateEmailFolder(EventPageBase).ContentLink, new CultureInfo(Localization.LanguageHelper.MasterLanguage(EventPageBase)));
            (emailBlock as IContent).Name = "New message";
            emailBlock.EventPage = EventPageBase as PageReference;
            contentRepository.Save(emailBlock as IContent, SaveAction.Publish);
            return emailBlock;
        }

        public static void MarkAsSent(ScheduledEmailBlock scheduledEmailBlock)
        {
            scheduledEmailBlock = scheduledEmailBlock.CreateWritableClone() as ScheduledEmailBlock;
            scheduledEmailBlock.DateSent = DateTime.Now;
            DataFactory.Instance.Save(scheduledEmailBlock as IContent,
                SaveAction.Publish | SaveAction.ForceCurrentVersion, EPiServer.Security.AccessLevel.NoAccess);
        }



        public static List<IParticipant> GetParticipantsForScheduledEmail(ScheduledEmailBlock scheduledEmailBlock)
        {
            List<IParticipant> participants = new List<IParticipant>();
            Hashtable statuses = new Hashtable();
            if (scheduledEmailBlock.AttendStatusFilter != null)
                foreach (string status in scheduledEmailBlock.AttendStatusFilter.Split(','))
                {
                    statuses.Add(status, status);
                }
            if (scheduledEmailBlock.EventPage != null)
                foreach (var participant in AttendRegistrationEngine.GetParticipants(scheduledEmailBlock.EventPage))
                {
                    if (scheduledEmailBlock.AttendStatusFilter == null || statuses.Contains(participant.AttendStatus))
                        participants.Add(participant);
                }
            return participants;
        }


        public static void SendScheduledEmail(ScheduledEmailBlock scheduledEmailBlock, IParticipant participant)
        {
            EmailTemplateBlock et;
            EmailSender email;

            if (scheduledEmailBlock != null)
            {
                if (scheduledEmailBlock.EmailTemplateContentReference == null)
                    et = (scheduledEmailBlock.EmailTemplate);
                else
                    et = DataFactory.Instance.Get<EmailTemplateBlock>(scheduledEmailBlock.EmailTemplateContentReference);

                if (et != null)
                {
                    email = new EmailSender(participant, et);
                    email.Send();
                }
            }
        }

        public static IEnumerable<ScheduledEmailBlock> GetScheduledEmails(EventPageBase EventPageBase)
        {
            return (from e in GetAllEmails(EventPageBase.ContentLink) where (e.EmailSendOptions == SendOptions.Specific || e.EmailSendOptions == SendOptions.Relative) select e);
        }

        public static IEnumerable<ScheduledEmailBlock> GetScheduledEmailsToSend(EventPageBase eventPageBase, DateTime plannedAfterDate, DateTime plannedBeforeDate)
        {
            var scheduledEmailBlocks = GetScheduledEmails(eventPageBase);
            var scheduledEmailBlocksToSend = new List<ScheduledEmailBlock>();
            scheduledEmailBlocksToSend.AddRange((from x in scheduledEmailBlocks where x.SendDateTime < plannedBeforeDate && x.SendDateTime > plannedAfterDate && x.DateSent < new DateTime(1801, 01, 01) select x));
            return scheduledEmailBlocksToSend;
        }

        public static IEnumerable<ScheduledEmailBlock> GetScheduledEmailsToSend(EventPageBase eventPageBase)
        {
            return GetScheduledEmailsToSend(eventPageBase, DateTime.MinValue, DateTime.Now);
        }


        public static List<ScheduledEmailBlock> GetScheduledEmailsToSend(List<EventPageBase> eventPages)
        {
            List<ScheduledEmailBlock> emailsToSend = new List<ScheduledEmailBlock>();
            foreach (EventPageBase eventPageBase in eventPages)
            {
                foreach (ScheduledEmailBlock scheduledEmailBlock in AttendScheduledEmailEngine.GetScheduledEmailsToSend(eventPageBase))
                {
                    emailsToSend.Add(scheduledEmailBlock);
                }
            }
            return emailsToSend;
        }


        public IEnumerable<ScheduledEmailBlock> Sort(IEnumerable<ScheduledEmailBlock> scheduledEmailBlocks)
        {
            return scheduledEmailBlocks.OrderBy(s => s.SendDateTime);
        }


        public static DateTime GetSendDate(ScheduledEmailBlock scheduledEmailBlock, EventPageBase EventPageBase)
        {
            DateTime dateToSend = DateTime.Now;
            bool subtract = false;
            if (scheduledEmailBlock.EmailSendOptions == SendOptions.Specific)
                return scheduledEmailBlock.SpecificDateScheduled;
            if (scheduledEmailBlock.EmailSendOptions == SendOptions.Relative)
            {
                switch (scheduledEmailBlock.ScheduledRelativeTo)
                {
                    case RelativeTo.AfterEventStart:
                        subtract = false;
                        dateToSend = EventPageBase.EventDetails.EventStart;
                        break;

                    case RelativeTo.BeforeEventStart:
                        subtract = true;
                        dateToSend = EventPageBase.EventDetails.EventStart;
                        break;

                    case RelativeTo.AfterStartPublish:
                        subtract = false;
                        dateToSend = EventPageBase.StartPublish ?? DateTime.Now;
                        break;

                    case RelativeTo.BeforeStartPublish:
                        subtract = true;
                        dateToSend = EventPageBase.StartPublish ?? DateTime.Now;
                        break;

                }
                int amount = scheduledEmailBlock.ScheduledRelativeAmount;
                if (subtract)
                    amount = amount * -1;

                switch (scheduledEmailBlock.ScheduledRelativeUnit)
                {
                    case RelativeUnit.Days:
                        dateToSend = dateToSend.AddDays(amount);
                        break;
                    case RelativeUnit.Hours:
                        dateToSend = dateToSend.AddHours(amount);
                        break;
                    case RelativeUnit.Minutes:
                        dateToSend = dateToSend.AddMinutes(amount);
                        break;
                    case RelativeUnit.Months:
                        dateToSend = dateToSend.AddMonths(amount);
                        break;
                }
                return dateToSend;
            }
            return DateTime.MaxValue;
        }

        public static string GetStatus(ScheduledEmailBlock scheduledEmailBlock)
        {
            if (scheduledEmailBlock.EmailSendOptions == SendOptions.Action)
                return "<span class='label label-success'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/attendstatus/" + scheduledEmailBlock.SendOnStatus.ToString()) + "</span>";
            if (scheduledEmailBlock.EventPage == null)
                return string.Empty;
            DateTime sendDateTime = AttendScheduledEmailEngine.GetSendDate(scheduledEmailBlock,
                ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(scheduledEmailBlock.EventPage));
            if (sendDateTime >
                    DateTime.Now && ((int)sendDateTime.Subtract(DateTime.Now).TotalDays > 0))
                return "<span class='label label-success'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/in") + " " + (int)sendDateTime.Subtract(DateTime.Now).TotalDays + " " + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/days") + "</span>";
            if (sendDateTime >
                    DateTime.Now && ((int)sendDateTime.Subtract(DateTime.Now).TotalHours > 0))
                return "<span class='label label-success'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/in") + " " + (int)sendDateTime.Subtract(DateTime.Now).TotalHours + " " + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/hours") + "</span>";
            if (sendDateTime >
                    DateTime.Now && ((int)sendDateTime.Subtract(DateTime.Now).TotalHours <= 0))
                return "<span class='label label-success'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/in") + " " + (int)sendDateTime.Subtract(DateTime.Now).TotalMinutes + " " + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/minutes") + "</span>";
            if (DateTime.Now.Subtract(scheduledEmailBlock.DateSent).TotalDays > 60000)
                return "<span class='label label-warning'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/sending") + "...</span>";
            else
            {
                return "<span class='label label-primary'>" + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/sent") + " " + (int)DateTime.Now.Subtract(scheduledEmailBlock.DateSent).TotalDays + " " + EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/admin/daysAgo") + "</span>";
            }
        }

        public static SaveAction GetForcedSaveActionFor(IVersionable page)
        {
            var saveAction = SaveAction.SkipValidation | SaveAction.ForceCurrentVersion;
            switch (page.Status)
            {
                case VersionStatus.Published:
                    saveAction = saveAction | SaveAction.Publish;
                    break;
                case VersionStatus.CheckedIn:
                    saveAction = saveAction | SaveAction.CheckIn;
                    break;
                default:
                    saveAction = saveAction | SaveAction.Save;
                    break;
            }
            return saveAction;
        }


    }
}