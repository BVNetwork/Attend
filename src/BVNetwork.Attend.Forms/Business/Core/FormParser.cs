using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Forms.Models.Forms;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core.Models;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.ServiceLocation;
using EPiServer.XForms;
using EPiServer.XForms.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace BVNetwork.Attend.Forms.Business.Core
{
    public class FormParser
    {
        private const string __AttendEvent = "__AttendEvent";
        private const string __AttendEmail = "__AttendEmail";
        private const string __AttendSessions = "__AttendSessions";

        public static void ProcessForm(NameValueCollection rawFormData, FormContainerBlock formBlock, Submission submissionData) {

            string eventPageId = rawFormData[__AttendEvent];
            if (string.IsNullOrEmpty(eventPageId)) // Not an Attend form - exit form processing.
                return;
            SetPrivatePropertyValue<PropertyData>(false, "IsReadOnly", formBlock.Property["SubmitSuccessMessage"]);

            NameValueCollection nvc = FormParser.ParseForm(submissionData, formBlock);
            ContentReference eventPage = new ContentReference(eventPageId).ToPageReference();
            IParticipant participant = FormParser.GenerateParticipation(eventPage, nvc);
            string message = null;
            EventPageBase eventPageBase = ServiceLocator.Current.GetInstance<IContentRepository>().Get<EventPageBase>(eventPage);
            if (participant.AttendStatus == AttendStatus.Confirmed.ToString())
            {
                if (eventPageBase.CompleteContentXhtml != null)
                    message = eventPageBase.CompleteContentXhtml.ToHtmlString();
                else
                    message = EPiServer.Framework.Localization.LocalizationService.Current.GetString("/eventRegistrationPage/confirmed");
            }
            if (participant.AttendStatus == AttendStatus.Submitted.ToString())
                if (eventPageBase.SubmittedContentXhtml != null)
                    message = eventPageBase.SubmittedContentXhtml.ToHtmlString();
                else
                    message = EPiServer.Framework.Localization.LocalizationService.Current.GetString("/eventRegistrationPage/submitted");
            if (message == null)
                message = EPiServer.Framework.Localization.LocalizationService.Current.GetString("/eventRegistrationPage/error");
            formBlock.SubmitSuccessMessage = new XhtmlString(message);

        }


        public static NameValueCollection GetFormData(IParticipant participant) {
            NameValueCollection _formControls = new NameValueCollection();
            return _formControls;
        }


        public static string SerializeForm(NameValueCollection values) {
            var allValues = new XElement("FormData",
                                        values.AllKeys.Select(o => new XElement(o, values[o]))
                                     );
            var result = new XElement("FormData", (from element in allValues.Elements() where element.Name.ToString().StartsWith("__") == false select element));
            return result.ToString();
        }

        public static NameValueCollection ParseForm(Submission submission, FormContainerBlock formContainer) {
            NameValueCollection formData = new NameValueCollection();
            IContentRepository rep = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
            string email = string.Empty;
            foreach (var element in formContainer.ElementsArea.Items)
            {
                bool skip = false;
                var control = rep.Get<IContent>(element.ContentLink);
                string value = string.Empty;
                string key = "__field_" + control.ContentLink.ID.ToString();
                if(submission.Data.ContainsKey(key)) {
                    var elementObject = submission.Data[key];
                    if (elementObject != null) { 
                        value = elementObject.ToString();
                    if (new [] { "email", "e-mail", "epost", "e-post" }.Contains(control.Name.ToLower() ))
                        { 
                        formData.Add(__AttendEmail, value);
                        skip = true;
                    }
                    if (control as AttendSessionForm != null) { 
                            formData.Add(__AttendSessions, value);
                            skip = true;
                        }
                    }
                    if(!skip)
                    formData.Add(control.Name, value);
                }
            }
            return formData;

        }

        public static IParticipant GenerateParticipation(ContentReference eventPage, NameValueCollection nvc) {
            string email = nvc.AllKeys.Contains(__AttendEmail) ? nvc[__AttendEmail] : "";
            IParticipant participant = null;
            if (!string.IsNullOrEmpty(email)) { 
                participant = Attend.Business.API.AttendRegistrationEngine.GenerateParticipation(eventPage, email, FormParser.SerializeForm(nvc));
                string sessions = nvc.AllKeys.Contains(__AttendSessions) ? nvc[__AttendSessions] : "";
                participant.Sessions = parseSessionsToContentArea(parseSessionsToStringArray(sessions));
            }
            Attend.Business.API.AttendRegistrationEngine.SaveParticipant(participant);
            Attend.Business.API.AttendRegistrationEngine.SendStatusMail(participant);

            return participant;
        }

        private static string[] parseSessionsToStringArray(string sessions) {
            string[] result = sessions.Replace(" ","").Split(',');
            result = result.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return result;
        }

        private static ContentArea parseSessionsToContentArea(string[] sessions) {
            var sessionsContentArea = new ContentArea();
            foreach (var session in sessions)
            {
                var sessionContentReference = new ContentReference(session);
                sessionsContentArea.Items.Add(new ContentAreaItem() { ContentLink = sessionContentReference });
            }
            return sessionsContentArea;
        }


        public static void SetPrivatePropertyValue<T>(object obj, string propName, T val)
        {
            Type t = val.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, val, new object[] { obj });
        }



        public static string GetEmail(NameValueCollection formData) {
            if (formData["__AttendEmail"] != null)
                return formData["__AttendEmail"];
            return string.Empty;
        }


    }
}