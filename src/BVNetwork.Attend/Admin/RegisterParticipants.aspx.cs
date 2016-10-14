using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Export;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Settings;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor.TinyMCE.Plugins;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVNetwork.Attend.Admin
{
    public partial class RegisterParticipants : System.Web.UI.Page
    {
        EventPageBase currentEvent;
        public EventPageBase CurrentEvent
        {
            get
            {
                if (currentEvent == null)
                {
                    string EventID = Request.QueryString["eventid"];
                    if (!string.IsNullOrEmpty(EventID))
                        currentEvent = ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(new ContentReference(EventID));
                }
                return currentEvent;
            }
            set { currentEvent = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            DataBindTables();
        }

        protected void DataBindTables() {
            RegisteredRepeater.DataSource = AttendRegistrationEngine.GetParticipants(CurrentEvent.ContentLink, Business.Text.AttendStatus.Participated);
            NotRegisteredRepeater.DataSource = AttendRegistrationEngine.GetParticipants(CurrentEvent.ContentLink, Business.Text.AttendStatus.Confirmed);
            DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected string GetProgressBar(EventPageBase eventPageBase)
        {
            if (eventPageBase.EventDetails.Cancelled == true)
                return "<div class='progress-bar progress-bar-danger' style='width:100%;'></div>";

          
            if (ConfirmedParticipants() == 0)
                return "<div class='progress-bar progress-bar-info' style='width:100%;'></div>";

            return
                string.Format("<div class='progress-bar progress-bar-success' style='width:{0}%;'><div class='pull-right'>{1}&nbsp;</span></div></div>", Math.Round(((double)(ParticipatedParticipants()) / (double)ExpectedParticipants() * 100)), ParticipatedParticipants());

        }

        private int _numberConfirmed;
        private int _numberParticipated;
        private int _numberExpected;

        private void PopulateParticipantNumbers() {
            EventPageBase eventPageBase = CurrentEvent;
             _numberConfirmed = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipants(eventPageBase.ContentLink, Business.Text.AttendStatus.Confirmed).Count<IParticipant>();
             _numberParticipated = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipants(eventPageBase.ContentLink, Business.Text.AttendStatus.Participated).Count<IParticipant>();
             _numberExpected = _numberConfirmed + _numberParticipated;
        }

        protected int ExpectedParticipants()
        {
            if (_numberExpected == 0)
                PopulateParticipantNumbers();
            return _numberExpected;
        }

        protected int ConfirmedParticipants() {
            if (_numberExpected == 0)
                PopulateParticipantNumbers();
            return _numberConfirmed;
        }

        protected int ParticipatedParticipants() {
            if (_numberExpected == 0)
                PopulateParticipantNumbers();
            return _numberParticipated;
        }

        protected void AddStatusText(string statusText) {
            StatusPlaceHolder.Visible = true;
            StatusLiteral.Text = statusText;
        }

        protected void RegisterParticipantButton_Command(object sender, CommandEventArgs e)
        {
            string[] participantInfo = e.CommandArgument.ToString().Split(',');
            IParticipant participant = AttendRegistrationEngine.GetParticipant(participantInfo[0], participantInfo[1]);
            RegisterParticipant(participant);
        }


        protected void UnRegisterParticipantButton_Command(object sender, CommandEventArgs e)
        {
            string[] participantInfo = e.CommandArgument.ToString().Split(',');
            IParticipant participant = AttendRegistrationEngine.GetParticipant(participantInfo[0], participantInfo[1]);
            RegisterParticipant(participant, true);
        }

        private void RegisterParticipant(IParticipant participant)
        {
            RegisterParticipant(participant, NoshowCheckBox.Checked);
        }
        private void RegisterParticipant(IParticipant participant, bool unregister) {
            if (participant != null)
            {
                if(unregister)
                    participant.AttendStatus = Attend.Business.Text.AttendStatus.Confirmed.ToString();
                else
                    participant.AttendStatus = Attend.Business.Text.AttendStatus.Participated.ToString();

                AttendRegistrationEngine.SaveParticipant(participant);
                AddStatusText(string.Format("Participant {0} with code {1} is now unregistered.", participant.Email, participant.Code));
            }
            else
                AddStatusText(string.Format("ERROR REGISTERING PARTICIPANT"));
            DataBindTables();

        }

        protected void RegisterByCodeButton_Click(object sender, EventArgs e)
        {
            string code = TextBoxBarCode.Text;
            IParticipant participant = AttendRegistrationEngine.GetParticipant(code);
            RegisterParticipant(participant);
            TextBoxBarCode.Text = "";
            TextBoxBarCode.Focus();
        }

        protected void RegisterAllParticipantsButton_Click(object sender, EventArgs e)
        {
            IEnumerable<IParticipant> allConfirmed = AttendRegistrationEngine.GetParticipants(CurrentEvent.ContentLink, Business.Text.AttendStatus.Confirmed);
            foreach (IParticipant participant in allConfirmed)
                RegisterParticipant(AttendRegistrationEngine.GetParticipant(participant.Code), false);
            AddStatusText("All participants registered!");
        }
    }
}