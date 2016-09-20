using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;

namespace BVNetwork.Attend.Views.Pages.Partials
{
    public partial class EventPageEditScheduledEmail : EPiServer.UserControlBase
    {
        public EventPageBase CurrentEventPageBase { get; set; }
        protected List<ScheduledEmailBlock> ScheduledEmails { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScheduledEmails = AttendScheduledEmailEngine.GetAllEmails(CurrentPage.ContentLink).ToList();
            SetupGenericPreviewPropertyControl(ScheduledEmailContentArea, ScheduledEmails);

        }

        private void SetupGenericPreviewPropertyControl(Property propertyControl, BlockData contents)
        {
            List<BlockData> contentList = new List<BlockData>();
            contentList.Add(contents);
            SetupGenericPreviewPropertyControl(propertyControl, contentList);
        }

        private void SetupGenericPreviewPropertyControl(Property propertyControl, IEnumerable<BlockData> contents)
        {
            var contentArea = new ContentArea();
            foreach (var content in contents)
            {
                contentArea.Items.Add(new ContentAreaItem { ContentLink = (content as IContent).ContentLink });
            }

            var previewProperty = new PropertyContentArea { Value = contentArea, Name = "PreviewPropertyData", IsLanguageSpecific = true };

            propertyControl.InnerProperty = previewProperty;
            propertyControl.DataBind();

        }


        protected void ConvertLocalConfirmBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("ConfirmMailTemplateBlock", "ConfirmMailTemplate");
        }


        private void ConvertEmailTemplateToLocal(string sharedBlock, string localBlock)
        {
            EventPageBase currentEventPageBase = CurrentPage.CreateWritableClone() as EventPageBase;
            EmailTemplateBlock sharedBlockData = Locate.ContentRepository().Get<EmailTemplateBlock>((CurrentPage as EventPageBase)[sharedBlock] as ContentReference) as EmailTemplateBlock;
            EmailTemplateBlock localBlockData = currentEventPageBase[localBlock] as EmailTemplateBlock;
            localBlockData.BCC = sharedBlockData.BCC;
            localBlockData.CC = sharedBlockData.CC;
            localBlockData.From = sharedBlockData.From;
            localBlockData.MainBody = sharedBlockData.MainBody;
            localBlockData.MainTextBody = sharedBlockData.MainTextBody;
            localBlockData.Subject = sharedBlockData.Subject;
            localBlockData.To = sharedBlockData.To;
            currentEventPageBase[sharedBlock] = null;
            Locate.ContentRepository().Save(currentEventPageBase, SaveAction.Save | SaveAction.ForceCurrentVersion);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((currentEventPageBase as IContent).ContentLink) + "'", true);
        }

        protected void CreateNew_Click(object sender, EventArgs e)
        {
            ScheduledEmailBlock emailBlock =
                AttendScheduledEmailEngine.GenerateScheduledEmailBlock(CurrentEventPageBase.ContentLink);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptid",
                "window.parent.location.href='" +
                EPiServer.Editor.PageEditing.GetEditUrl((emailBlock as IContent).ContentLink) + "'", true);
        }
    }
}