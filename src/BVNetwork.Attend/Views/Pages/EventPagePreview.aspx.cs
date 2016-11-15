using System.Diagnostics;
using System.Reflection;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Business.Log;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using BVNetwork.Attend.Views.Blocks;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Editor;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Web;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.UI.Admin;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using EPiServer.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Views.Pages
{
    [TemplateDescriptor(Inherited = true, Default = true, AvailableWithoutTag = false, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Pages/EventPagePreview.aspx")]
    public partial class EventPagePreview : PreviewPage, IRenderTemplate<EventPageBase>
    {
        protected List<SessionBlock> Sessions { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            Sessions = AttendSessionEngine.GetSessions(CurrentPage.ContentLink).ToList();

            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();

            //ParticipantsContentArea.DataBind();
            //SessionsContentArea.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool UseForms = BVNetwork.Attend.Business.API.AttendRegistrationEngine.UseForms;

            SetupGenericPreviewPropertyControl(SessionsContentArea, Sessions);


            //if(SubmitMailTemplateBlock != null)
            //SubmitMailTemplate. = Locate.ContentRepository().Get<EmailTemplateBlock>((CurrentPage as EventPageBase).ConfirmMailTemplateBlock) as PropertyData;

            FormsPlaceHolder.DataBind();

            DetailsContentXhtmlWrapper.Visible = UseForms;
            DetailsContentRepeaterWrapper.Visible = !UseForms;

            ClosedContentXhtmlWrapper.Visible = UseForms;
            ClosedContentRepeaterWrapper.Visible = !UseForms;

            NoSeatsXhtmlWrapper.Visible = UseForms;
            NoSeatsContentWrapper.Visible = !UseForms;


            ConfirmedXhtmlWrapper.Visible = UseForms;
            ConfirmedContentWrapper.Visible = !UseForms;

            SubmittedXhtmlWrapper.Visible = UseForms;
            SubmittedContentWrapper.Visible = !UseForms;


            EventPageBaseEditScheduledEmailControl.DataBind();

            if ((CurrentPage as EventPageBase).DetailsContent != null)
            DetailsContentRepeater.DataSource = (CurrentPage as EventPageBase).DetailsContent.FilteredItems;
            DetailsContentRepeater.DataBind();
            DetailsContentRepeaterWrapper.ApplyEditAttributes<EventPage>(p => p.DetailsContent);
            (this as PageBase).EditHints.Add("DetailsContent");

            if ((CurrentPage as EventPageBase).RegistrationFormContainer != null)
                RegistrationFormContainerRepeater.DataSource = (CurrentPage as EventPageBase).RegistrationFormContainer.FilteredItems;
            RegistrationFormContainerRepeater.DataBind();
            FormsPlaceHolderContainer.ApplyEditAttributes<EventPage>(p => p.RegistrationFormContainer);
            (this as PageBase).EditHints.Add("RegistrationFormContainer");


            if ((CurrentPage as EventPageBase).ClosedContent != null)
                ClosedContentRepeater.DataSource = (CurrentPage as EventPageBase).ClosedContent.FilteredItems;
            ClosedContentRepeater.DataBind();
            ClosedContentRepeaterWrapper.ApplyEditAttributes<EventPage>(p => p.ClosedContent);
            (this as PageBase).EditHints.Add("ClosedContent");

            if ((CurrentPage as EventPageBase).NoSeatsContent != null)
                NoSeatsContentRepeater.DataSource = (CurrentPage as EventPageBase).NoSeatsContent.FilteredItems;
            NoSeatsContentRepeater.DataBind();
            NoSeatsContentWrapper.ApplyEditAttributes<EventPage>(p => p.NoSeatsContent);
            (this as PageBase).EditHints.Add("NoSeatsContent");




            if ((CurrentPage as EventPageBase).CompleteContent != null)
            ConfirmedContentRepeater.DataSource = (CurrentPage as EventPageBase).CompleteContent.FilteredItems;
            ConfirmedContentRepeater.DataBind();
            ConfirmedContentWrapper.ApplyEditAttributes<EventPage>(p => p.CompleteContent);
            (this as PageBase).EditHints.Add("CompleteContent");

            if ((CurrentPage as EventPageBase).SubmittedContent != null)
            SubmittedContentRepeater.DataSource = (CurrentPage as EventPageBase).SubmittedContent.FilteredItems;
            SubmittedContentRepeater.DataBind();
            SubmittedContentWrapper.ApplyEditAttributes<EventPage>(p => p.SubmittedContent);
            (this as PageBase).EditHints.Add("SubmittedContent");




        }


        protected void CreateSession_Click(object sender, EventArgs e)
        {
            string sessionName = string.IsNullOrEmpty(SessionName.Text) ? "New Session" : SessionName.Text;
            SessionBlock session = AttendSessionEngine.GenerateSession(CurrentPage as EventPageBase, sessionName, (CurrentPage as EventPageBase).EventDetails.EventStart, (CurrentPage as EventPageBase).EventDetails.EventEnd);
            ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((session as IContent).ContentLink) + "'", true);
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

        protected string GetBlockName(object contentAreaItem) {
            return (contentAreaItem as ContentAreaItem).GetContent().Name;
        }






        protected void ConvertLocalSubmitBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("SubmitMailTemplateBlock", "SubmitMailTemplate");
        }

        protected void ConvertLocalCancelBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("CancelMailTemplateBlock", "CancelMailTemplate");
        }

        protected void ConvertLocalNotificationBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("NotificationMailTemplateBlock", "NotificationMailTemplate");
        }

        protected void ConvertLocalConfirmBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("ConfirmMailTemplateBlock", "ConfirmMailTemplate");
        }

        private void ConvertEmailTemplateToLocal(string sharedBlock, string localBlock)
        {
            EventPageBase currentEventPageBase = CurrentPage.CreateWritableClone() as EventPageBase;
            EmailTemplateBlock sharedBlockData = Get<EmailTemplateBlock>((CurrentPage as EventPageBase)[sharedBlock] as ContentReference) as EmailTemplateBlock;
            EmailTemplateBlock localBlockData = currentEventPageBase[localBlock] as EmailTemplateBlock;
            localBlockData.BCC = sharedBlockData.BCC;
            localBlockData.CC = sharedBlockData.CC;
            localBlockData.From = sharedBlockData.From;
            localBlockData.MainBody = sharedBlockData.MainBody;
            localBlockData.MainTextBody = sharedBlockData.MainTextBody;
            localBlockData.Subject = sharedBlockData.Subject;
            localBlockData.To = sharedBlockData.To;
            currentEventPageBase[sharedBlock] = null;
            Locate.ContentRepository().Save(currentEventPageBase, AttendScheduledEmailEngine.GetForcedSaveActionFor(currentEventPageBase));
            ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentData as IContent).ContentLink) + "'", true);
        }


        public bool SendStatusMail(ParticipantBlock participant, EmailTemplateBlock et)
        {
            EmailSender email;
            email = new EmailSender(participant, et);
            
            email.Send();

            return true;
        }


        private int GetIntSafe(string text)
        {
            int i = 0;
            int.TryParse(text, out i);
            return i;
        }

    }
}