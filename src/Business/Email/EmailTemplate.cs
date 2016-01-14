using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Mail;
using BVNetwork.Attend.Business.Participant;
using EPiServer.Editor.TinyMCE.Plugins;
using log4net;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Business.Email
{
    public delegate string EmailTemplateDataBindDelegate(string objectName, string propertyName);

    //[Serializable()]
    public class EmailTemplate
    {
        private static ILog _log = LogManager.GetLogger(typeof(EmailTemplate));
        protected static Regex databindPattern = new Regex(@"[[]([a-zA-Z0-9~_+-]+)[.]([a-z0-9.~ _+-]+)[]]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected static Regex xmlSafe = new Regex("[<>&]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //protected static XmlSerializer serializer = new XmlSerializer(typeof(EmailTemplate));

        public string From;
        public string To;
        public string Cc;
        public string Bcc;
        public string Subject;
        public string Body;
        public string HtmlBody;
        public string VCal;
        public bool SendAsSms;
        public string Badge;
        public IParticipant Participant;

        private System.IO.MemoryStream BadgeStream;
        private System.IO.MemoryStream VCalStream;

        private Attachment[] Attachments;
        private int AttachmentCount;

        public EmailTemplate()
        {
            To = "[CurrentRegistration.Email]";
            Attachments = new Attachment[10];
            AttachmentCount = 0;
        }

        public void AddAttachment(Attachment a)
        {
            if (AttachmentCount < 10)
                Attachments[AttachmentCount] = a;
            AttachmentCount++;
        }

        public bool Send()
        {
            if (!string.IsNullOrEmpty(Badge))
            {

                BadgeStream = new System.IO.MemoryStream();
                BadgeStream.Seek(0L, SeekOrigin.Begin);

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType("image/gif");
                Attachment attachment = new Attachment(BadgeStream, contentType);
                attachment.ContentDisposition.FileName = "badge.gif";
                AddAttachment(attachment);
            }
            if (!string.IsNullOrEmpty(VCal))
            {

                VCalStream = new System.IO.MemoryStream();

                byte[] contentAsBytes = Encoding.UTF8.GetBytes(VCal);
                VCalStream.Write(contentAsBytes, 0, contentAsBytes.Length);
                VCalStream.Seek(0, SeekOrigin.Begin);

                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType("text/calendar");
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                contentType.Name = "event.vcal";
                Attachment attachment = new Attachment(VCalStream, contentType);
                attachment.ContentDisposition.FileName = "event.ics";
                AddAttachment(attachment);
            }

            try
            {
                if (SendAsSms)
                {
                    _log.Debug(string.Format("Attend: Sending SMS from {0} to {1}", From, To));
                    SmsSender.SendSMS(To, From, Body);
                    if (Participant != null)
                        Log.ParticipantLog.AddLogTextAndSave("SMS",
                            "SMS to " + To + " with message " + Body + " sent successfully.", Participant);
                }
                else
                {
                    MailMessage mm;

                    if (string.IsNullOrEmpty(Body) == false)
                    {
                        mm = new MailMessage(From, To, Subject, Body);
                    }
                    else
                    {
                        mm = new MailMessage(From, To, Subject, HtmlBody);
                        mm.IsBodyHtml = true;
                    }

                    if (!String.IsNullOrEmpty(Cc))
                        mm.CC.Add(Cc);
                    if (!String.IsNullOrEmpty(Bcc))
                        mm.Bcc.Add(Bcc);

                    if (mm.IsBodyHtml == false)
                    {
                        // We only add an alternative view if we have got both
                        // a text-only AND html version of the content.
                        mm.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(HtmlBody, Encoding.UTF8,
                            "text/html"));
                    }

                    for (int i = 0; i < AttachmentCount; i++)
                        mm.Attachments.Add(Attachments[i]);
                    _log.Debug(string.Format("Attend: Sending e-mail from {0} to {1}", mm.From, mm.To));
                    new SmtpClient().Send(mm);
                    if (Participant != null)
                        Log.ParticipantLog.AddLogTextAndSave("Mail",
                            "E-mail to " + mm.To + " with subject " + mm.Subject + " sent successfully.", Participant);
                }
            }
            catch (Exception e)
            {
                if (Participant != null)
                    Log.ParticipantLog.AddLogTextAndSave("Mail", "Error sending e-mail: " + e.Message, Participant);
                _log.Error(string.Format("Attend: Error sending e-mail - {0}", e.Message), e);
                return false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(VCal))
                    VCalStream.Close();
                if (!string.IsNullOrEmpty(Badge))
                    BadgeStream.Close();
            }
            return true;
        }

        protected static string XmlSafe(Match m)
        {
            if (m.Value == ">")
                return "&gt;";
            else if (m.Value == "<")
                return "&lt;";
            else if (m.Value == "&")
                return "&amp;";
            return "";
        }

        public static EmailTemplate Load(EmailTemplateBlock template, EventPageBase EventPageBase, IParticipant participant)
        {
            EmailTemplate email = new EmailTemplate();
            email.HtmlBody = PopulatePropertyValues(template.MainBody != null ? template.MainBody.ToString() : string.Empty, EventPageBase, participant);
            email.Body = PopulatePropertyValues(template.MainTextBody, EventPageBase, participant);
            email.To = PopulatePropertyValues(template.To, EventPageBase, participant);
            email.From = PopulatePropertyValues(template.From, EventPageBase, participant);
            email.Cc = PopulatePropertyValues(template.CC, EventPageBase, participant);
            email.Bcc = PopulatePropertyValues(template.BCC, EventPageBase, participant);
            email.Subject = PopulatePropertyValues(template.Subject, EventPageBase, participant);
            email.Participant = participant;
            email.SendAsSms = template.SendAsSms;
            return email;
        }


        public static string PopulatePropertyValues(string template, EventPageBase EventPageBase, IParticipant participant)
        {
            if (template != null)
            {
                template = EmailTemplate.databindPattern.Replace(template, delegate(Match m)
                {

                    string value = GetPropertyValue(m.Groups[1].Value, m.Groups[2].Value, EventPageBase, participant);

                    if (!String.IsNullOrEmpty(value))
                        value = EmailTemplate.xmlSafe.Replace(value, new MatchEvaluator(XmlSafe));

                    return value;
                });
            }
            return template;
        }

        protected static string GetPropertyValue(string objectName, string propertyName, EventPageBase CurrentEvent, IParticipant CurrentParticipant)
        {
            object value = null;

            if (0 == String.Compare("CurrentPage", objectName, true))
            {
                if (0 == String.Compare(propertyName, "PageLinkUrl", true))
                {
                    string linkUrl = CurrentEvent.LinkURL;
                    UrlBuilder ub = new UrlBuilder(linkUrl);

                    Global.UrlRewriteProvider.ConvertToExternal(ub, CurrentEvent.PageLink, System.Text.Encoding.UTF8);

                    value = ub.ToString();
                }
                else
                {
                    value = CurrentEvent.Property[propertyName];
                }
            }
            else if (0 == String.Compare("CurrentRegistration", objectName, true))
            {
                value = AttendRegistrationEngine.GetParticipantInfo(CurrentParticipant, propertyName);
            }


            if (null == value)
                return "";
            else
                return value.ToString();
        }


    }
}
