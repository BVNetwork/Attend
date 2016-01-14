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
using EPiServer.Editor;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms;
using System.Collections.Specialized;
using EPiServer.XForms.WebControls;
using EPiServer.XForms.Util;
using System.Collections;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = true, Tags = new[] { "ListView" }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/SessionBlockListViewControl.ascx")]
    public partial class SessionBlockListViewControl : BlockControlBase<SessionBlock>
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}