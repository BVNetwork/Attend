using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Settings;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Admin.Partials
{
    public partial class EmailList : System.Web.UI.UserControl
    {
        public List<ScheduledEmailBlock> Messages { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected IEnumerable<ScheduledEmailBlock> GetScheduledEmailBlocks()
        {
            return Messages;
        }

        protected string GetPageName(ScheduledEmailBlock email) {
            if (email.EventPage != null)
            {
                var eventPage = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(email.EventPage);
                if (eventPage != null)
                    return eventPage.Name;
            }
            return "Page not found";
        }

    }
}