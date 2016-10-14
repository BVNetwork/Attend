using System;
using System.Collections.Generic;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Core;
using EPiServer.PlugIn;
using iTextSharp.text;
using EPiServer.BaseLibrary.Scheduling;

namespace BVNetwork.Attend.Business.ScheduledJobs
{
    [ScheduledPlugIn(DisplayName = "AttendSendScheduledEmails")]
    public class AttendSendScheduledEmails : JobBase
    {
        private bool _stopSignaled;

        public AttendSendScheduledEmails()
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
            OnStatusChanged(String.Format("Sending scheduled emails..."));

            List<EventPageBase> allEventPages = AttendScheduledEmailEngine.GetEventPageBases();
            List<ScheduledEmailBlock> emailsToSend = AttendScheduledEmailEngine.GetScheduledEmailsToSend(allEventPages);



            int numberOfMailsSent = 0;
            int numberOfMailsToSend = emailsToSend.Count;

            foreach (ScheduledEmailBlock scheduledEmailBlock in emailsToSend)
            {
                List<IParticipant> participantsToSendEmailTo = AttendScheduledEmailEngine.GetParticipantsForScheduledEmail(scheduledEmailBlock);
                foreach (IParticipant participant in participantsToSendEmailTo)
                {
                    AttendScheduledEmailEngine.SendScheduledEmail(scheduledEmailBlock, participant);
                    numberOfMailsSent++;
                    OnStatusChanged(String.Format("Sent "+numberOfMailsSent+" messages."));

                    if (_stopSignaled)
                    {
                        return "Stop of job was called";
                    }

                }
                AttendScheduledEmailEngine.MarkAsSent(scheduledEmailBlock);
            }
            return "Job completed. Sent "+numberOfMailsSent+" messages through "+numberOfMailsToSend+" templates.";
        }

    }
}
