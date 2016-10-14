using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using BVNetwork.Attend.Admin.Partials;
using BVNetwork.Attend.Business.API;
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
using EPiServer.XForms;

namespace BVNetwork.Attend.Admin
{
    public partial class ImportText : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            base.OnInit(e);
        }


        protected void ImportBtn_Click(object sender, EventArgs e)
        {
            //PageData parent = EPiServer.DataFactory.Instance.Get<PageData>();
            StatusLiteral.Text = "";
            StatusLiteral.Text += "<h1>Import started</h1><br>";
            EventPageBase ep = ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(RootTextBox.PageLink);
            string participants = ParticipantsTextBox.Text;
            string[] participantStrings = participants.Split('\n');
            if (participantStrings.Count() < 2 )
                return;
            string[] keys = participantStrings[0].Split(';');
            for(int i = 1; i < participantStrings.Count(); i++)
            {
                ImportParticipant(ep, participantStrings[i].Split(';'), keys);
            }

        }

        protected void ImportParticipant(EventPageBase ep, string[] participantFields, string[] keys)
        {
            string email = "";
            Hashtable fieldsHashtable = new Hashtable();
            if(participantFields.Count() == keys.Count())
            for (int i = 0; i < keys.Count(); i++)
            {
                if(!fieldsHashtable.ContainsKey(keys[i]))
                    fieldsHashtable.Add(keys[i].ToLower(), participantFields[i]);
            }

            if (fieldsHashtable.ContainsKey("email"))
                email = fieldsHashtable["email"].ToString();
            else
                return;
            if(string.IsNullOrEmpty(email))
                return;

            XForm xform = ep.RegistrationForm;

            XFormData xFormData = xform.CreateFormData();

            PopulateChildNodeRecursive(fieldsHashtable, xFormData.Data.ChildNodes);
           

            string xformstring = xFormData.Data.InnerXml;
            StatusLiteral.Text += "Adding participant: " + email+"<br/>";
            AttendRegistrationEngine.GenerateParticipation(ep.ContentLink, email, false, xformstring,
                "Imported participant from text");
        }

        private void PopulateChildNodeRecursive(Hashtable data, XmlNodeList parentNodes)
        {
         foreach (XmlNode formNode in parentNodes)
            {
                if (data.ContainsKey(formNode.Name.ToLower()))
                    formNode.InnerText = data[formNode.Name.ToLower()].ToString();
                PopulateChildNodeRecursive(data, formNode.ChildNodes);
            }
        }

        protected void CreateEvent(string eventString, ContentReference parent, string language)
        {
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

    }
}