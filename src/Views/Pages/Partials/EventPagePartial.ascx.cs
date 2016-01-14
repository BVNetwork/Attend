using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVNetwork.Attend.Business.Participant;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Web;
using EPiServer.XForms;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Business.Text;
using EPiServer.XForms.Util;

namespace BVNetwork.Attend.Views.Pages.Partials
{
     [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag = true, Path = "~/Modules/BVNetwork.Attend/Views/Pages/Partials/EventPagePartial.ascx")]
    public partial class EventPagePartial : ContentControlBase<EventPage>
    {

        protected override void OnInit(EventArgs e)
        {
            DetailsXFormControl.FormDefinition = XForm.CreateInstance(new Guid((CurrentData as EventPageBase).RegistrationForm.Id.ToString()));

            string email = Request.QueryString["email"];
            string code = Request.QueryString["code"];

            HiddenCode.Value = code;
            HiddenEmail.Value = email;


            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(code))
            {
                IParticipant participant = AttendRegistrationEngine.GetParticipant(email, code);
                if (participant != null)
                {
                    SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                    xmlDoc.LoadXml(participant.XForm);
                    DetailsXFormControl.Data.Data = xmlDoc;
                }
            }

            SessionsPanel.Controls.Add(AttendSessionEngine.GetSessionsControl(CurrentData.ContentLink, null));
            SessionsPanel.DataBind();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string propertyName = "DetailsContent";
            if (AttendRegistrationEngine.GetNumberOfSeats(CurrentData.ContentLink) < 1)
                propertyName = "NoSeatsContent";
            if (AttendRegistrationEngine.RegistrationOpen(CurrentData.ContentLink) == false)
            {
                propertyName = "ClosedContent";
            }
            ContentProperty.PropertyName = propertyName;

           
        }


        protected void AttendButton_Click(object sender, EventArgs e)
        {
            IParticipant participant;
            if (!string.IsNullOrEmpty(HiddenEmail.Value))
            {
                participant = AttendRegistrationEngine.GetParticipant(HiddenEmail.Value, HiddenCode.Value);
                if (participant != null)
                    participant = (participant as ParticipantBlock).CreateWritableClone() as ParticipantBlock;
            }

            else {
                string participantEmail = "";
                foreach (var fragment in DetailsXFormControl.ExtractXFormControls())
                {
                    if (fragment.ID == "epost" || fragment.ID == "email")
                    {
                        participantEmail = fragment.Value;
                    }
                }


                if (string.IsNullOrEmpty(participantEmail)) {

                    return;
                }
                    
                participant = AttendRegistrationEngine.GenerateParticipation(CurrentData.ContentLink, participantEmail, true, DetailsXFormControl.Data.Data.OuterXml, "Participant submitted form");
            }
            if (participant != null) { 
            participant.XForm = DetailsXFormControl.Data.Data.OuterXml;
            participant.Sessions = new ContentArea();
            foreach (ContentReference item in GetChosenSessions()) {
                participant.Sessions.Items.Add(new ContentAreaItem() { ContentLink=item });
            }
            Locate.ContentRepository().Save(participant as IContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            string propertyName = "";
            if (participant.AttendStatus == AttendStatus.Confirmed.ToString())
                propertyName = "CompleteContent";
            if (participant.AttendStatus == AttendStatus.Submitted.ToString())
                propertyName = "SubmittedContent";
            ContentProperty.PropertyName = propertyName;
            ContentProperty.DataBind();
            DetailsXFormControl.Visible = false;
            AttendButton.Visible = false;
            }
        }

        private ICollection<ContentReference> GetChosenSessions() {
            ICollection<ContentReference> items = new List<ContentReference>();
            foreach (Control c in SessionsPanel.Controls[0].Controls) {
                RadioButton rb = (c as RadioButton);
                if (rb != null && rb.InputAttributes["value"] != null && rb.Checked == true)
                    items.Add(new ContentReference(rb.InputAttributes["value"]));
                CheckBox cb = (c as CheckBox);
                if (cb != null && cb.InputAttributes["value"] != null && cb.Checked == true)
                    items.Add(new ContentReference(cb.InputAttributes["value"]));
                
            }
            return items;
        }

    }
}