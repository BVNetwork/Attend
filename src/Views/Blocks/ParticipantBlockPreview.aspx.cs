using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Views.Blocks
{
     [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag=true, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ParticipantBlockPreview.aspx")]
    public partial class ParticipantBlockPreview : PreviewPage, IRenderTemplate<ParticipantBlock>
    {

        protected IEnumerable<PreviewArea> PreviewAreas { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            propertyControl.RenderSettings.Tag = "edit";
            SetupPreviewPropertyControl(propertyControl, new[] { CurrentData });
            
            this.DataBind();

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
            var previewProperty = new PropertyContentArea { Value = contentArea, Name = "PreviewPropertyData", IsLanguageSpecific = true};

            // Render the temporary property using the Property control in the web form
            propertyControl.InnerProperty = previewProperty;
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

    }
}