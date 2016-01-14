using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVNetwork.Attend.Business.Participant;
using log4net;
using EPiServer.Core;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;

namespace BVNetwork.Attend.Business.Email
{
    public class EmailSender
    {
        private IParticipant CurrentParticipant { get; set; }
        private EventPageBase CurrentEvent { get; set; }
        private string PropertyName { get; set; }

        public EmailTemplate Email { get; set; }

        public delegate void MailEventHandler(object o, EmailEventArgs e);

        public static event MailEventHandler BeforeSendingEmail;
        public static event MailEventHandler AfterSendingEmail;


        protected static void BeforeSendingEmailHandler(EmailEventArgs e)
        {
            if (BeforeSendingEmail != null)
                BeforeSendingEmail(new object(), e);
        }

        protected static void AfterSendingEmailHandler(EmailEventArgs e)
        {
            if (AfterSendingEmail != null)
                AfterSendingEmail(new object(), e);
        }

        public EmailSender(EventPageBase EventPageBase, EmailTemplateBlock emailTemplate)
        {
            CurrentEvent = EventPageBase;
            CurrentParticipant = null;
            Email = EmailTemplate.Load(emailTemplate, CurrentEvent, null);
        }

        public EmailSender(IParticipant participant, EmailTemplateBlock emailTemplate)
        {
            CurrentEvent = EPiServer.DataFactory.Instance.Get<EventPageBase>(participant.EventPage);
            CurrentParticipant = participant;
            Email = EmailTemplate.Load(emailTemplate, CurrentEvent, CurrentParticipant);
        }

        public void Send() {
            if (null != Email)
            {
                if (!string.IsNullOrEmpty(Email.HtmlBody + Email.Body))
                {
                    EmailEventArgs e1 = new EmailEventArgs();
                    e1.Email = Email;
                    e1.CancelEmail = false;
                    BeforeSendingEmailHandler(e1);
                    bool success = true;
                    if(e1.CancelEmail == false)
                        success = Email.Send();
                    EmailEventArgs e2 = new EmailEventArgs();
                    e2.Email = Email;
                    e2.Success = success;
                    AfterSendingEmailHandler(e2);
                }
            }
        }



    }
}