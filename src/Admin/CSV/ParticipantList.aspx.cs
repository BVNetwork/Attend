using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Admin.CSV
{
    public partial class ParticipantList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            IEnumerable<IParticipant> participants = AttendRegistrationEngine.GetParticipants(new EPiServer.Core.ContentReference(HttpContext.Current.Request.QueryString["EventPageBase"]));

            string attachment = "attachment; filename=ParticipantList.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Attend", "public");

            var sb = new StringBuilder();
            foreach (var participant in participants)
                sb.AppendLine(GetParticipantData(participant));

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        private string GetParticipantData(IParticipant participant)
        {
            string data = participant.Email + ";" + participant.Code + ";" + participant.AttendStatus+";";
            NameValueCollection formfields = AttendRegistrationEngine.GetFormData(participant);
            foreach (var key in formfields.AllKeys)
                data += formfields.Get(key)+";";
            return data;
        }
    }
}