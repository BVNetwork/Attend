using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using BVNetwork.Attend.Views.Blocks;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.WebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace BVNetwork.Attend.Admin
{
    public partial class Import : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            XFormInput.XForm = new EPiServer.XForms.XForm();
            XFormInput.PageLink = PageReference.StartPage;
            XFormInput.ParentLink = PageReference.StartPage;
            base.OnInit(e);
        }

        protected void TestBtn_Click(object sender, EventArgs e)
        {
            StatusLiteral.Text = "";
            StatusLiteral.Text += GetUrlContent(RemoteSiteTextBox.Text + "/Export/?test=true&root=" + RemoteRootTextBox.Text) + "<br>";
        }


        protected void ImportBtn_Click(object sender, EventArgs e)
        {
            //PageData parent = EPiServer.DataFactory.Instance.Get<PageData>();
            StatusLiteral.Text = "";
            StatusLiteral.Text += "<h1>Import started</h1><br>";
            string AllEvents = GetUrlContent(RemoteSiteTextBox.Text + "/Export/?root=" + RemoteRootTextBox.Text);
            foreach (string EventString in DelimitContent(AllEvents, "[EVENTDELIMITER]"))
            {
                if (!string.IsNullOrEmpty(EventString))
                    CreateEvent(EventString, RootTextBox.PageLink, (!string.IsNullOrEmpty(LanguageTextBox.Text)) ? LanguageTextBox.Text : "EN");
            }
        }

        protected void CreateEvent(string eventString, ContentReference parent, string language)
        {
            EventPageBase NewEvent = EPiServer.DataFactory.Instance.GetDefault<EventPageBase>(parent, new LanguageSelector(language));
            string ParticipantsString = "";
            string DetailsBody = "";
            string Status = "";
            foreach (string EventData in DelimitContent(eventString, "[EVENTFIELDDELIMITER]"))
            {
                string[] EventDataValues = DelimitContent(EventData, "[DATAVALUEDELIMITER]");
                
                if (EventDataValues.Length > 1)
                {
                    string dataKey = EventDataValues[0];
                    string dataValue = EventDataValues[1];
                    Status += "- " + dataKey +": "+dataValue+"<br>";
                    if (!string.IsNullOrEmpty(dataKey) && !string.IsNullOrEmpty(dataValue))
                        switch (dataKey)
                        {
                            case "Participants":
                                ParticipantsString = dataValue;
                                //StatusLiteral.Text += DelimitContent(EventDataValues[1], "[PARTICIPANTDELIMITER")[0];
                                break;
                            case "EventName":
                                NewEvent.Name = dataValue;
                                break;
                            case "EventStart":
                                NewEvent.EventDetails.EventStart = GetDateSafe(dataValue);
                                break;
                            case "EventEnd":
                                NewEvent.EventDetails.EventEnd = GetDateSafe(dataValue);
                                break;
                            case "RegistrationOpen":
                                NewEvent.EventDetails.RegistrationOpen = GetDateSafe(dataValue);
                                break;
                            case "RegistrationClose":
                                NewEvent.EventDetails.RegistrationClose = GetDateSafe(dataValue);
                                break;
                            case "AutoConfirmThreshold":
                                NewEvent.EventDetails.NumberOfSeats = GetIntSafe(dataValue);
                                break;
                            case "IntroBody":
                                NewEvent.IntroBody = new XhtmlString(dataValue);
                                break;
                            case "DetailsBody":
                                DetailsBody = dataValue;
                                break;
                            case "Price":
                                NewEvent.EventDetails.Price = GetIntSafe(dataValue);
                                break;

                        }

                }

            }
            NewEvent.RemoveFieldsFromView = "";
            NewEvent.RegistrationForm = XFormInput.XForm;
            EPiServer.DataFactory.Instance.Save(NewEvent, EPiServer.DataAccess.SaveAction.Publish);
            StatusLiteral.Text += "<h2>"+NewEvent.Name+"</h2>";
            CreateParticipants(ParticipantsString, NewEvent.ContentLink);
        }

        protected string GetMailString(string mailTemplate, string property)
        {
            XElement element = XElement.Parse(mailTemplate.Substring(1));
            return element.Element(property) != null ? element.Element(property).Value : string.Empty;
        }

        protected int GetIntSafe(string date)
        {
            int safeInt = 0;
            int.TryParse(date, out safeInt);
            return safeInt;
        }
        protected DateTime GetDateSafe(string date)
        {
            DateTime safeDate = DateTime.Now;
            DateTime.TryParseExact(date, "dd'.'MM'.'yyyy' 'HH':'mm':'ss", null, System.Globalization.DateTimeStyles.None, out safeDate);
            return safeDate;
        }

        /*- DetailsBody
        - SessionsBody
        - CompleteBody
        - ClosedBody
        - Teacher
        - MainIntro
        - EventID
        */
        protected void CreateParticipants(string participantsString, ContentReference EventPageBase)
        {
            StatusLiteral.Text += "<h3>Participants:</h3>";
            foreach (string ParticipantString in DelimitContent(participantsString, "[PARTICIPANTDELIMITER]"))
                CreateParticipant(ParticipantString, EventPageBase);
        }

        protected void CreateParticipant(string participantString, ContentReference EventPageBase)
        {
            IParticipant NewParticipant = null;
            foreach (string ParticipantData in DelimitContent(participantString, "[PARTICIPANTFIELDDELIMITER]"))
            {
                string[] ParticipantDataValues = DelimitContent(ParticipantData, "[PARTICIPANTVALUEDELIMITER]");
                switch (ParticipantDataValues[0])
                {
                    case "Email":
                        NewParticipant = AttendRegistrationEngine.GenerateParticipation(EventPageBase, ParticipantDataValues[1], false, string.Empty, "Imported participant");
                        break;
                }
            }
            if (NewParticipant != null)
            {

                StatusLiteral.Text += "<br><ul>";
                foreach (string ParticipantData in DelimitContent(participantString, "[PARTICIPANTFIELDDELIMITER]"))
                {
                    string[] ParticipantDataValues = DelimitContent(ParticipantData, "[PARTICIPANTVALUEDELIMITER]");
                    if (ParticipantDataValues.Length > 1)
                    {
                        string dataKey = ParticipantDataValues[0];
                        string dataValue = ParticipantDataValues[1];

                        StatusLiteral.Text += "<li>" + dataKey + ": " + dataValue + "</li>";
                        switch (ParticipantDataValues[0])
                        {
                            case "Email":
                                break;
                            case "Status":
                                NewParticipant.AttendStatus = dataValue;
                                break;
                            case "XForm":
                                NewParticipant.XForm = dataValue;
                                break;
                            case "Code":
                                NewParticipant.Code = dataValue;
                                break;
                            case "Submitted":
                                NewParticipant.DateSubmitted = GetDateSafe(dataValue);
                                break;
                            case "Username":
                                NewParticipant.Username = dataValue;
                                break;
                        }
                    }
                }

                StatusLiteral.Text += "</ul>";
                EPiServer.DataFactory.Instance.Save(NewParticipant as IContent, EPiServer.DataAccess.SaveAction.Publish);
            }
        }


        private string[] DelimitContent(string content, string delimiter)
        {
            content = content.Replace(delimiter, "§");
            return content.Split('§');
        }





        protected string GetUrlContent(string urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            else
                return response.StatusCode.ToString();
            return data;
        }
    }
}