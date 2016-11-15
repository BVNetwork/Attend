using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.WebControls;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Framework.DataAnnotations;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms.WebControls;
using EPiServer.XForms.Util;
using EPiServer.XForms;
using System.Text;
using EPiServer.Framework.Web;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.API;
using EPiServer.ServiceLocation;
using BVNetwork.Attend.Business.Participant;
using System.Collections.Specialized;
using System.Xml;

namespace BVNetwork.Attend.Views.Blocks
{
     [TemplateDescriptor(Inherited = false, Default = true, AvailableWithoutTag=false, Tags = new[] { RenderingTags.Preview, RenderingTags.Edit }, Path = "~/Modules/BVNetwork.Attend/Views/Blocks/ParticipantBlockPreviewControl.ascx")]
    public partial class ParticipantBlockPreviewControl : BlockControlBase<ParticipantBlock>
    {
        protected string FormDataXml { get; set; }
        protected NameValueCollection FormData { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            bool UseForms = BVNetwork.Attend.Business.API.AttendRegistrationEngine.UseForms;
            var eventPage = Locate.ContentRepository().Get<EventPageBase>(CurrentBlock.EventPage);
            if (UseForms == false)
            {
                DetailsXFormControl.FormDefinition = XForm.CreateInstance(new Guid(eventPage.RegistrationForm.Id.ToString()));
                PopulateForm();
                DetailsXFormControl.DataBind();
            }
            else {
                FormData = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetFormData(CurrentBlock);
                FormElementsRepeater.DataSource = FormData;
                FormElementsRepeater.DataBind();
            }
            XFormContainer.Visible = !UseForms;
            FormContainer.Visible = UseForms;
            SessionList.Controls.Add(AttendSessionEngine.GetSessionsControl(CurrentData.EventPage, CurrentData));
        }

        protected string GetFormElementValue(string key)
        {
            var value = FormData[key];
            if (!string.IsNullOrEmpty(value))
                return value;
            return string.Empty;
        }

        protected void UpdateParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                bool UseForms = BVNetwork.Attend.Business.API.AttendRegistrationEngine.UseForms;

                ParticipantBlock current = (CurrentBlock.CreateWritableClone() as ParticipantBlock);
                if (UseForms)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlNode rootNode = doc.CreateElement("FormData");
                    doc.AppendChild(rootNode);
                    foreach (RepeaterItem item in FormElementsRepeater.Items)
                    {
                        if (item.ItemType == ListItemType.Item
                            || item.ItemType == ListItemType.AlternatingItem)
                        {
                            System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)item.FindControl("FormLabel");
                            TextBox textBox = (TextBox)item.FindControl("FormTextBox");

                            XmlNode formElementNode = doc.CreateElement(label.Text);
                            formElementNode.InnerText = textBox.Text;
                            rootNode.AppendChild(formElementNode);
                        }
                    }
                    current.XForm = doc.InnerXml;
                }
                else
                    current.XForm = DetailsXFormControl.Data.Data.OuterXml;
                Locate.ContentRepository().Save(current as IContent, EPiServer.DataAccess.SaveAction.Publish);
            }
            catch (Exception ex) {
                StatusLiteral.Text = ex.Message;
            }
        }

        protected void UpdateSessions_Click(object sender, EventArgs e)
        {
            ParticipantBlock current = (CurrentBlock.CreateWritableClone() as ParticipantBlock);
            current.Sessions = new ContentArea();
            current.Sessions.Items.Clear();
            foreach (ContentReference item in GetChosenSessions())
            {
                current.Sessions.Items.Add(new ContentAreaItem() { ContentLink = item });
            }
            Locate.ContentRepository().Save(current as IContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }



        private ICollection<ContentReference> GetChosenSessions()
        {
            ICollection<ContentReference> items = new List<ContentReference>();
            foreach (Control c in SessionList.Controls[0].Controls)
            {
                if (c.GetOriginalType() == typeof(System.Web.UI.WebControls.RadioButton)) { 
                    RadioButton rb = (c as RadioButton);
                    if (rb != null && rb.InputAttributes["value"] != null && rb.Checked == true)
                        items.Add(new ContentReference(rb.InputAttributes["value"]));
                }
                if (c.GetOriginalType() == typeof (System.Web.UI.WebControls.CheckBox))
                {
                    CheckBox cb = (c as CheckBox);
                    if (cb != null && cb.InputAttributes["value"] != null && cb.Checked == true)
                        items.Add(new ContentReference(cb.InputAttributes["value"]));
                }
            }
            return items;
        }
         
        protected void SendMail_Click(object sender, EventArgs e)
        {
            UpdateParticipant_Click(sender, e);
            AttendRegistrationEngine.SendStatusMail(Locate.ContentRepository().Get<ParticipantBlock>((CurrentBlock as IContent).ContentLink));
        }

         

        protected void PopulateForm()
        {
            if (!string.IsNullOrEmpty(CurrentData.XForm))
            {
                SerializableXmlDocument xmlDoc = new SerializableXmlDocument();
                xmlDoc.LoadXml(CurrentData.XForm);
                DetailsXFormControl.Data.Data = xmlDoc;
            }
        }

        protected string EventUrl { get { return EPiServer.Editor.PageEditing.GetEditUrl((Locate.ContentRepository().Get<EventPageBase>(CurrentBlock.EventPage) as IContent).ContentLink); } }

        protected string GetLogText
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentBlock.Log))
                    return "No log text";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table class='table table-striped table-hover'>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Date</td>");
                sb.AppendLine("<td>Type</td>");
                sb.AppendLine("<td>Logtext</td>");
                sb.AppendLine("</tr>");
                foreach (string s in CurrentBlock.Log.Split(';'))
                {
                    sb.AppendLine("<tr>");
                    foreach (string t in s.Split('|'))
                        sb.AppendLine("<td>" + t + "</td>");
                    sb.AppendLine("</tr>");

                }
                sb.AppendLine("</table>");
                return sb.ToString();
            }
        }
    }
}