using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.WebControls;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag = false, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ScheduledEmailBlockPreview.aspx")]
    public partial class ScheduledEmailBlockPreview : PreviewPage, IRenderTemplate<ScheduledEmailBlock>
    {

        protected IEnumerable<PreviewArea> PreviewAreas { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SendOptionsControl.ApplyEditAttributes<ScheduledEmailBlock>(p => p.EmailSendOptions);
            BlockName.ApplyEditAttributes<ScheduledEmailBlock>(p => (p as IContent).Name);
            SendOnStatusControl.ApplyEditAttributes<ScheduledEmailBlock>(p => p.SendOnStatus);
            ScheduledRelativeAmountControl.ApplyEditAttributes<ScheduledEmailBlock>(p => p.ScheduledRelativeAmount);
            ScheduledRelativeToControl.ApplyEditAttributes<ScheduledEmailBlock>(p => p.ScheduledRelativeTo);
            ScheduledRelativeUnitControl.ApplyEditAttributes<ScheduledEmailBlock>(p => p.ScheduledRelativeUnit);
            (this as PageBase).EditHints.Add("ScheduledRelativeTo");
            (this as PageBase).EditHints.Add("ScheduledRelativeAmount");
            (this as PageBase).EditHints.Add("ScheduledRelativeUnit");
            (this as PageBase).EditHints.Add("SendOnStatus");
            (this as PageBase).EditHints.Add("SendOptions");

            if ((CurrentData as ScheduledEmailBlock).EmailTemplateContentReference != null &&
                (CurrentData as ScheduledEmailBlock).EmailTemplateContentReference != ContentReference.EmptyReference)
                SetupPreviewPropertyControl(MailTemplateBlockPreview,
                    new[]
                    {
                        Locate.ContentRepository()
                            .Get<IContent>((CurrentData as ScheduledEmailBlock).EmailTemplateContentReference)
                    });
            else
                ConfirmMailTemplateBlockPreviewPlaceHolder.Visible = false;

            MailTemplateBlockPreview.RenderSettings.Tag = "edit";
            MailTemplateBlockPreview.DataBind();

            this.DataBind();
            
        }

        protected string EventUrl
        {
            get
            {
                if((CurrentData as ScheduledEmailBlock).EventPage != null)
                return
                    EPiServer.Editor.PageEditing.GetEditUrl(
                        (Locate.ContentRepository().Get<EventPageBase>((CurrentData as ScheduledEmailBlock).EventPage)
                            as IContent).ContentLink);
                return string.Empty;
            }
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

        protected void ConvertLocalConfirmBlock_OnClick(object sender, EventArgs e)
        {
            ConvertEmailTemplateToLocal("ConfirmMailTemplateBlock", "ConfirmMailTemplate");
        }

        private void ConvertEmailTemplateToLocal(string sharedBlock, string localBlock)
        {
            ScheduledEmailBlock currentScheduledEmailBlock = ((CurrentData as ScheduledEmailBlock).CreateWritableClone()  as ScheduledEmailBlock);
            EmailTemplateBlock sharedBlockData = Get<EmailTemplateBlock>((CurrentData as ScheduledEmailBlock).EmailTemplateContentReference as ContentReference) as EmailTemplateBlock;
            EmailTemplateBlock localBlockData = currentScheduledEmailBlock.EmailTemplate as EmailTemplateBlock;
            localBlockData.BCC = sharedBlockData.BCC;
            localBlockData.CC = sharedBlockData.CC;
            localBlockData.From = sharedBlockData.From;
            localBlockData.MainBody = sharedBlockData.MainBody;
            localBlockData.MainTextBody = sharedBlockData.MainTextBody;
            localBlockData.Subject = sharedBlockData.Subject;
            localBlockData.To = sharedBlockData.To;
            currentScheduledEmailBlock.EmailTemplateContentReference = null;
            Locate.ContentRepository().Save(currentScheduledEmailBlock as IContent, SaveAction.Save | SaveAction.ForceCurrentVersion);
            ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentData as IContent).ContentLink) + "'", true);
        }

        protected void EditConfirmMailTemplate_OnClick(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "scriptid", "window.parent.location.href='" + EPiServer.Editor.PageEditing.GetEditUrl((CurrentData as ScheduledEmailBlock).EmailTemplateContentReference) + "'", true);
        }


        protected void previewRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            var propertyControl = e.Item.FindControl("propertyControl") as Property;
            var previewArea = (PreviewArea)e.Item.DataItem;

            propertyControl.RenderSettings.Tag = previewArea.AreaTag;
            propertyControl.Visible = previewArea.Supported;

            if (previewArea.Supported)
            {
                SetupPreviewPropertyControl(propertyControl, new[] { CurrentData });
            }
        }

        protected class PreviewArea
        {
            public bool Supported { get; set; }
            public string AreaName { get; set; }
            public string AreaTag { get; set; }
        }

        protected void Reset_OnClick(object sender, EventArgs e)
        {
            ScheduledEmailBlock current = (CurrentData as ScheduledEmailBlock).CreateWritableClone() as ScheduledEmailBlock;
            current.DateSent = new DateTime(1800,01,01);
            DataFactory.Instance.Save(current as IContent, SaveAction.Publish | SaveAction.ForceCurrentVersion);

        }
    }
}