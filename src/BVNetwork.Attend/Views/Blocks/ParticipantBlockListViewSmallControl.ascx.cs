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
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = true, Tags = new[] { "ListViewSmall" }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ParticipantBlockListViewSmallControl.ascx")]
    public partial class ParticipantBlockListViewSmallControl : BlockControlBase<ParticipantBlock>
    {
        private NameValueCollection _formControls;
        private Hashtable _hiddenControls;

        protected NameValueCollection FormControls
        {
            get
            {
                //EventPageBase CurrentEvent = Locate.ContentRepository().Get<EventPageBase>(CurrentBlock.EventPageBase);


                if (_formControls == null)
                {
                    _formControls = AttendRegistrationEngine.GetFormData(CurrentBlock);

                }
                return _formControls;
            }
        }


        protected string GetFormControlValue(string key) {
            return "<td>"+FormControls.Get(key)+"</td>";
         }

        protected void Page_Load(object sender, EventArgs e)
        {

            FormFieldsRepeater.DataSource = FormControls.AllKeys;
            FormFieldsRepeater.DataBind();
        }
    }
}