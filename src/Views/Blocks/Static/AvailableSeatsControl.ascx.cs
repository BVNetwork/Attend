using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Views.Blocks.Static
{
    [TemplateDescriptor(AvailableWithoutTag = true, Default = false, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/Static/AvailableSeatsControl.ascx")]
    public partial class AvailableSeatsControl : BlockControlBase<AvailableSeatsBlock>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetAvailableSeatsText() {
            int available = AttendRegistrationEngine.GetAvailableSeats(CurrentPage.ContentLink);
            if (available > 3 && !string.IsNullOrEmpty(CurrentData.ManyAvailableSeats))
                return string.Format(CurrentData.ManyAvailableSeats, available);
            if (available == 3)
                return CurrentData.ThreeSeats;
            if (available == 2)
                return CurrentData.TwoSeats;
            if (available == 1)
                return CurrentData.OneSeat;
            return string.Empty;
        }
    }
}