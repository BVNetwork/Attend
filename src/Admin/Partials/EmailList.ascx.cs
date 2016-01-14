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
        protected void Page_Load(object sender, EventArgs e)
        {
            ScheduledEmailsRepeater.DataSource = AttendScheduledEmailEngine.GetEventPageBases();
            ScheduledEmailsRepeater.DataBind();
        }


        protected IEnumerable<ScheduledEmailBlock> GetScheduledEmailBlocks(EventPageBase EventPageBase)
        {
            return AttendScheduledEmailEngine.GetScheduledEmailsToSend(EventPageBase);

        }

    }
}