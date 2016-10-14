using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Framework.DataAnnotations;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms.WebControls;
using EPiServer.XForms.Util;
using EPiServer.XForms;
using System.Text;
using EPiServer.Framework.Web;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Views.Blocks
{
     [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag=false, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/SessionBlockPreviewControl.ascx")]
    public partial class SessionBlockPreviewControl : BlockControlBase<SessionBlock>
    {

        

        protected void Page_Init(object sender, EventArgs e)
        {
            var participants = AttendSessionEngine.GetParticipants(CurrentBlock);

            NoParticipants.Visible = !(participants.Any<ParticipantBlock>());
            NoParticipants.DataBind();

            SetupPreviewPropertyControl(ParticipantsContentArea, participants);

        }

        private void SetupPreviewPropertyControl(Property propertyControl, IEnumerable<BlockData> contents)
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

        protected string EventUrl
        {
            get
            {
                if ((CurrentData as SessionBlock).EventPage != null)
                    return
                        EPiServer.Editor.PageEditing.GetEditUrl(
                            (Locate.ContentRepository().Get<EventPageBase>((CurrentData as SessionBlock).EventPage)
                                as IContent).ContentLink);
                return string.Empty;
            }
        }

    }
}