using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.Events.Clients;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Editor;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms;
using System.Collections.Specialized;
using EPiServer.XForms.WebControls;
using EPiServer.XForms.Util;
using System.Collections;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = true, Tags = new[] { "ListView" }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ScheduledEmailBlockListViewControl.ascx")]
    public partial class ScheduledEmailBlockListViewControl : BlockControlBase<ScheduledEmailBlock>
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected string GetScheduledDate()
        {
            if (CurrentBlock.EmailSendOptions == SendOptions.Specific)
                return CurrentBlock.SpecificDateScheduled.ToString("g");
            if (CurrentBlock.EmailSendOptions == SendOptions.Action)
            {
                return CurrentBlock.SendOnStatus.ToString();
            }
            if (CurrentBlock.EventPage == null)
                return string.Empty;
            if (CurrentBlock.EmailSendOptions == SendOptions.Relative)
                return CurrentBlock.ScheduledText() + ": " + AttendScheduledEmailEngine.GetSendDate(CurrentBlock, DataFactory.Instance.Get<EventPageBase>(CurrentBlock.EventPage)).ToShortDateString();
            return string.Empty;
        }

        private void SetupPreviewPropertyControl(Property propertyControl, IEnumerable<IContent> contents)
        {
            var contentArea = new ContentArea();

            foreach (var content in contents)
            {
                contentArea.Items.Add(new ContentAreaItem { ContentLink = content.ContentLink });
            }

            var previewProperty = new PropertyContentArea { Value = contentArea, Name = "PreviewPropertyData", IsLanguageSpecific = true };

            propertyControl.InnerProperty = previewProperty;
        }

        protected void DeleteScheduledEmail_OnClick(object sender, EventArgs e)
        {
            Locate.ContentRepository().Delete((CurrentBlock as IContent).ContentLink, true, AccessLevel.NoAccess);
            this.Visible = false;
        }

        protected string GetClass()
        {
            if (CurrentBlock.EmailSendOptions == SendOptions.Action)
                return "success";
            else
            {
                if (
                    AttendScheduledEmailEngine.GetSendDate(CurrentBlock,
                        ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(CurrentBlock.EventPage)) >
                    DateTime.Now)
                    return "success";
            }
            return "";
        }

        protected string GetStatus()
        {
            if (CurrentBlock.EmailSendOptions == SendOptions.Action)
                return "<span class='label label-success'>When " + CurrentBlock.SendOnStatus.ToString() + "</span>";
            if (CurrentBlock.EventPage == null)
                return string.Empty;
            DateTime sendDateTime = AttendScheduledEmailEngine.GetSendDate(CurrentBlock,
                ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(CurrentBlock.EventPage));
            if (sendDateTime >
                    DateTime.Now)
                return "<span class='label label-success'>In " + sendDateTime.Subtract(DateTime.Now).TotalDays + " days</span>";
            else
            {
                if (DateTime.Now.Subtract(CurrentBlock.DateSent).TotalDays > 60000)
                    return "<span class='label label-warning'>Sending...</span>";
                else
                {
                    return "<span class='label label-primary'>Sent " + (int)DateTime.Now.Subtract(CurrentBlock.DateSent).TotalDays + " days ago</span>";
                }
            }
        }

    }
}