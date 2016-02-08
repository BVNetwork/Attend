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
using System.Linq;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Views.Blocks
{
    [TemplateDescriptor(Inherited = true, Tags = new[] {"ListView"},
        Path = "~/Modules/BVNetwork.Attend/Views/Blocks/SessionBlockListViewControl.ascx")]
    public partial class SessionBlockListViewControl : BlockControlBase<SessionBlock>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string NumberOfParticipants
        {
            get
            {
                int numberOfParticipants = 0;
                int numberOfSeats = CurrentBlock.NumberOfSeats;
                var allParticipants =
                    AttendSessionEngine.GetParticipants(CurrentBlock)
                        .Where(x => x.AttendStatus == AttendStatus.Confirmed.ToString());
                numberOfParticipants = allParticipants.Count();
                return string.Format("{0} / {1}", numberOfParticipants, numberOfSeats);
            }
        }
    }
}