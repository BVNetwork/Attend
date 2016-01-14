using System;
using System.Collections.Generic;
using BVNetwork.Attend.Models.Blocks;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.WebControls;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag = false, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/EMailTemplatePreview.aspx")]
    public partial class EMailTemplatePreview : PreviewPage, IRenderTemplate<EmailTemplateBlock>
    {

        protected List<ParticipantBlock> Participants { get; set; }
        protected List<SessionBlock> Sessions { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetupPreviewPropertyControl(propertyControl, new[] { CurrentData });


        }

        private void SetupPreviewPropertyControl(Property propertyControl, IEnumerable<IContent> contents)
        {
            // Define a content area
            var contentArea = new ContentArea();

            // Add the blocks to preview
            foreach (var content in contents)
            {
                contentArea.Items.Add(new ContentAreaItem { ContentLink = content.ContentLink });
            }

            // Create a temporary property for the content area
            var previewProperty = new PropertyContentArea { Value = contentArea, Name = "PreviewPropertyData", IsLanguageSpecific = true };

            // Render the temporary property using the Property control in the web form
            propertyControl.InnerProperty = previewProperty;
        }

    }
}