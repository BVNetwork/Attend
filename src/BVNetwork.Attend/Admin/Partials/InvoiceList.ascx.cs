﻿using BVNetwork.Attend.Business.API;
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
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Admin.Partials
{
    public partial class InvoiceList : System.Web.UI.UserControl
    {
        private int _invoice;
        private int _count;
        protected int TotalIncome { get; set; }
        protected int TotalCount { get; set; }

        protected string Income(IParticipant participant)
        {
            if(participant.AttendStatus == AttendStatus.Confirmed.ToString() || participant.AttendStatus == AttendStatus.Participated.ToString()) { 
                TotalIncome += participant.Price;
                TotalCount++;
            }
            return string.Empty;
        }

        public string[] GetInvoiceFields()
        {
            string InvoiceFildsSettingName = "InvoiceFilds";
            string storedFields = Settings.GetSetting(InvoiceFildsSettingName);
            ///TODO: Add settings
            if (string.IsNullOrEmpty(storedFields))
            {
                storedFields = "status;company;invoice_recipient;invoice_email;invoice_address;comment;eventname;price";
                Settings.AddSetting(InvoiceFildsSettingName, storedFields);
            }
            string[] fields = storedFields.Split(';');
            return fields;
        }

        protected string[] GetInvoiceValues(ParticipantBlock participant)
        {
            string[] values = GetInvoiceFields();
            for (int i = 0; i < values.Length; i++)
            {

                if (values[i] == "eventname")
                {
                    values[i] = GetCourseName(participant.EventPage) + " for " + GetFormData(participant, "firstname") + " " + GetFormData(participant, "lastname");
                }
                else
                values[i] = GetFormData(participant, values[i]);
            }
            return values;
        }

        public List<IParticipant> ParticipantList
        {
            get
            {
                return (ParticipantsRepeater.DataSource as List<IParticipant>);
            }
            set
            {
                ParticipantsRepeater.DataSource = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string GetCourseName(ContentReference EventPageBase)
        {
            var currentEvent = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>().Get<BVNetwork.Attend.Models.Pages.EventPageBase>(EventPageBase);
            if (currentEvent == null)
                return string.Empty;
            return currentEvent.Name;
        }

        protected string GetFormData(IParticipant participant, string fieldname)
        {
            return AttendRegistrationEngine.GetParticipantInfo(participant, fieldname);
        }


        protected int CalculateIncome(PageReference EventPageBase)
        {
            int income = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetTotalIncome(EventPageBase);
            TotalIncome += income;
            return income;
        }

        protected int CalculateParticipants(PageReference EventPageBase)
        {
            int count = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetNumberOfParticipants(EventPageBase);
            TotalCount += count;
            return count;
        }
    }
}