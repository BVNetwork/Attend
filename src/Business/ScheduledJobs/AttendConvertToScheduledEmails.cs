using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Filters;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using EPiServer.BaseLibrary.Scheduling;

namespace BVNetwork.Attend.Business.ScheduledJobs
{
    [ScheduledPlugIn(DisplayName = "CreateScheduledEmails")]
    public class AttendConvertToScheduledEmails : JobBase
    {
        private bool _stopSignaled;

        public AttendConvertToScheduledEmails()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {


            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));
            PropertyCriteriaCollection criteria = new PropertyCriteriaCollection();
            PropertyCriteria pageTypeCriteria = new PropertyCriteria();
            pageTypeCriteria.Condition = CompareCondition.Equal;
            pageTypeCriteria.Value = ServiceLocator.Current.GetInstance<IContentTypeRepository>().Load<EventPageBase>().ID.ToString();
            pageTypeCriteria.Type = PropertyDataType.PageType;
            pageTypeCriteria.Name = "PageTypeID";
            pageTypeCriteria.Required = true;
            criteria.Add(pageTypeCriteria);
            PageDataCollection allEvents = new PageDataCollection();

            PageDataCollection allLanguages = DataFactory.Instance.GetLanguageBranches(ContentReference.StartPage);

            foreach (PageData pageData in allLanguages)
            {
                allEvents.Add(DataFactory.Instance.FindPagesWithCriteria(ContentReference.StartPage, criteria, pageData.LanguageBranch));

            }
            int cnt = 0;
            foreach (var EventPageBase in allEvents)
            {
                cnt += UpdateEvent(EventPageBase as EventPageBase);
            }

            return "Found " + allEvents.Count + " pages, converted " + cnt + "e-mail templates!";

            //Add implementation

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return "Change to message that describes outcome of execution";
        }

        public static bool ContainsStatusMail(IEnumerable<ScheduledEmailBlock> emails, AttendStatus status)
        {
            foreach (var scheduledEmailBlock in emails)
            {
                if (scheduledEmailBlock.EmailSendOptions == SendOptions.Action &&
                    scheduledEmailBlock.SendOnStatus == status)
                    return true;
            }
            return false;

        }

        public static int UpdateEvent(EventPageBase EventPageBase)
        {
            var scheduledEmails =
                Attend.Business.API.AttendScheduledEmailEngine.GetScheduledEmails(EventPageBase.ContentLink);
            int cnt = 0;
            if (!ContainsStatusMail(scheduledEmails, AttendStatus.Submitted))
            {
                cnt++;
                CreateScheduledEmail(EventPageBase, AttendStatus.Submitted, EventPageBase["SubmitMailTemplate"] as EmailTemplateBlock, EventPageBase["SubmitMailTemplateBlock"] as ContentReference, "Submit mail template");
            }

            if (!ContainsStatusMail(scheduledEmails, AttendStatus.Confirmed))
            {
                cnt++;
                CreateScheduledEmail(EventPageBase, AttendStatus.Confirmed, EventPageBase["ConfirmMailTemplate"] as EmailTemplateBlock, EventPageBase["ConfirmMailTemplateBlock"] as ContentReference, "Confirm mail template");
            }

            if (!ContainsStatusMail(scheduledEmails, AttendStatus.Cancelled))
            {
                cnt++;
                CreateScheduledEmail(EventPageBase, AttendStatus.Cancelled, EventPageBase["CancelMailTemplate"] as EmailTemplateBlock, EventPageBase["CancelMailTemplateBlock"] as ContentReference, "Cancel mail template");
            }

            return cnt;

        }

        public static void CreateScheduledEmail(EventPageBase EventPageBase, AttendStatus status, EmailTemplateBlock emailTemplate, ContentReference emailTemplateContentReference, string name)
        {
            ScheduledEmailBlock emailBlock =
                API.AttendScheduledEmailEngine.GenerateScheduledEmailBlock(EventPageBase.ContentLink).CreateWritableClone() as ScheduledEmailBlock;
            emailBlock.EmailSendOptions = SendOptions.Action;
            emailBlock.SendOnStatus = status;
            emailBlock.EmailTemplate.BCC = emailTemplate.BCC;
            emailBlock.EmailTemplate.CC = emailTemplate.CC;
            emailBlock.EmailTemplate.From = emailTemplate.From;
            emailBlock.EmailTemplate.To = emailTemplate.To;
            emailBlock.EmailTemplate.Subject = emailTemplate.Subject;
            emailBlock.EmailTemplate.MainBody = emailTemplate.MainBody;
            emailBlock.EmailTemplate.MainTextBody = emailTemplate.MainTextBody;

            emailBlock.EmailTemplateContentReference = emailTemplateContentReference;

            (emailBlock as IContent).Name = name;

            DataFactory.Instance.Save(emailBlock as IContent, SaveAction.Publish);

        }
    }
}
