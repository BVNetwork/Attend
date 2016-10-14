using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Framework.DataAnnotations;

namespace BVNetwork.Attend.Views.Blocks.Static
{

    [TemplateDescriptor(AvailableWithoutTag = true, Default = false,  Path = "~/Modules/BVNetwork.Attend/Views/Blocks/Static/EmailTemplateBlockControl.ascx")]
    public partial class EmailTemplateBlockControl : BlockControlBase<EmailTemplateBlock>
    {
        protected void Page_Load(object sender, EventArgs e)
        
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();

            (this.Page as PageBase).EditHints.Add("SendAsSms");
            SendAsSMSProperty.ApplyEditAttributes<EmailTemplateBlock>(p => p.SendAsSms);
            DataBind();
        }
    }
}