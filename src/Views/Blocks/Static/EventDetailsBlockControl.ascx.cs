using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;

namespace BVNetwork.Attend.Views.Blocks.Static
{
    [TemplateDescriptor(AvailableWithoutTag = true, Default = false, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/Static/EventDetailsBlockControl.ascx")]
    public partial class EventDetailsBlockControl : BlockControlBase<EventDetailsBlock>
    {
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
            //CurrentEventPageBase = CurrentPage as EventPageBase;

        }

        protected string GetProgressBar(EventPageBase EventPageBase)
        {
            if (EventPageBase != null)
            {
                if (EventPageBase.EventDetails.Cancelled == true)
                    return "<div class='progress-bar progress-bar-danger' style='width:100%;'></div>";

                int numberOfSeats =
                    BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfSeats(EventPageBase.PageLink);
                int availableSeats = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetAvailableSeats(EventPageBase.PageLink);

                if (availableSeats == 0)
                    return "<div class='progress-bar progress-bar-info' style='width:100%;'></div>";

                return
                    string.Format("<div class='progress-bar' style='width:{0}%;'><div class='pull-right'>{1}&nbsp;</div></div>", Math.Round(((double)(numberOfSeats - availableSeats) / (double)numberOfSeats) * 100), numberOfSeats - availableSeats);
            }
            return "<div class='progress-bar' style='width:0;'></div>";
        }
    }
}