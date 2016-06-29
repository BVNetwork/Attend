using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.EditorDescriptors;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors.SelectionFactories;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.XForms.WebControls;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Newtonsoft.Json;

namespace BVNetwork.Attend.Models.Blocks
{
    [ContentType(DisplayName = "ScheduledEmailBlock", AvailableInEditMode = false, GUID = "fa4baf30-c34f-4557-a697-5d6b68d37a61", Description = "")]
    public class ScheduledEmailBlock : BlockData
    {
        public virtual EmailTemplateBlock EmailTemplate { get; set; }

        [AllowedTypes(typeof(EmailTemplateBlock))]
        public virtual ContentReference EmailTemplateContentReference { get; set; }


        [EnumAttribute(typeof(SendOptions))]
        [Display(Name = "E-mail send options")]
        public virtual SendOptions EmailSendOptions { get; set; }

        public virtual DateTime SpecificDateScheduled { get; set; }

        [EnumAttribute(typeof(RelativeTo))]
        public virtual RelativeTo ScheduledRelativeTo { get; set; }

        [EnumAttribute(typeof(RelativeUnit))]
        public virtual RelativeUnit ScheduledRelativeUnit { get; set; }

        public virtual int ScheduledRelativeAmount { get; set; }

		[JsonIgnore]
        public DateTime SendDateTime {
            get
            {
                return AttendScheduledEmailEngine.GetSendDate(this,
                    ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(this.EventPage));
            }
        }

        public EmailTemplateBlock GetEmailTemplateBlock()
        {
            if (EmailTemplateContentReference == null)
                return EmailTemplate;
            else
            {
                return
                    ServiceLocator.Current.GetInstance<IContentRepository>()
                        .Get<EmailTemplateBlock>(EmailTemplateContentReference);
            }
        }

        public string ScheduledText()
        {
            switch (EmailSendOptions)
            {
                case SendOptions.Action:
                    return this.SendOnStatus.ToString();
                case SendOptions.Specific:
                    return this.SpecificDateScheduled.ToShortDateString() + " " +
                           this.SpecificDateScheduled.ToShortTimeString();
                case SendOptions.Relative:
                    string relativeAmount = ScheduledRelativeAmount.ToString();
                    string relativeUnit = ScheduledRelativeUnit.ToString();
                    string relativeTo = ScheduledRelativeTo.ToString();
                    return string.Format("{0} {1} {2}", relativeAmount, relativeUnit, relativeTo);
            }
            return string.Empty;
        }

        public virtual DateTime DateSent { get; set; }

        [EnumAttribute(typeof(AttendStatus))]
        public virtual AttendStatus SendOnStatus { get; set; }

        public bool IsScheduled()
        {
            return (EmailSendOptions == SendOptions.Specific || EmailSendOptions == SendOptions.Relative);
        }

        [UIHint(EventRenderTags.AttendMultipleStatus)]
        public virtual string AttendStatusFilter { get; set; }

        public virtual PageReference EventPage { get; set; }



    }

}