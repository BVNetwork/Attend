using EPiServer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Forms.Core.Events;
using System.IO;
using System.Xml;
using EPiServer.Forms.Core.Models;
using EPiServer.Forms.Helpers.Internal;
using EPiServer.Forms.Implementation.Elements;
using BVNetwork.Attend.Forms.Business.Core;
using System.Collections.Specialized;
using EPiServer.Core;
using BVNetwork.Attend.Business.Participant;
using System.Reflection;
using BVNetwork.Attend.Models.Pages;
using System.Web.Mvc;
using BVNetwork.Attend.Business.Text;
using EPiServer;
using System.Text;
using BVNetwork.Attend.Forms.Models.Forms;

namespace BVNetwork.Attend.Forms.Business.InitializableModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]

    public class FormInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var formsEvents = ServiceLocator.Current.GetInstance<FormsEvents>();
            formsEvents.FormsSubmitting += FormsEvents_FormsSubmitting;
            ParticipantProviderBase.OnAddingParticipant += ParticipantProviderBase_OnAddingParticipant;
        }

        private void ParticipantProviderBase_OnAddingParticipant(object sender, ParticipantEventArgs e)
        {
            if(string.IsNullOrEmpty(e.CurrentParticipant.XForm))
            {
                StringBuilder xform = new StringBuilder();
                xform.AppendLine("<instance>");
                var eventRef = e.CurrentParticipant.EventPage;
                var rep = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
                var eventPage = rep.Get<EventPage>(eventRef);
                if (eventPage != null && eventPage.RegistrationFormContainer != null)
                {
                    foreach (ContentAreaItem contentAreaItem in eventPage.RegistrationFormContainer.FilteredItems)
                        if (contentAreaItem.ContentLink != null) { 
                            var formContainerBlock = rep.Get<FormContainerBlock>(contentAreaItem.ContentLink);
                            if(formContainerBlock != null)
                                foreach (ContentAreaItem formElement in formContainerBlock.ElementsArea.FilteredItems) {
                                    IContent element = rep.Get<IContent>(formElement.ContentLink);
                                    if(element != null && !string.IsNullOrEmpty(element.Name) && (element as AttendSubmitButton == null))
                                        xform.AppendLine("<"+element.Name+">"+ "</" + element.Name + ">");
                                }
                        }
                }
                xform.AppendLine("</instance>");
                e.CurrentParticipant.XForm = xform.ToString();
            }
        }

        private void FormsEvents_FormsSubmitting(object sender, FormsEventArgs e)
        {
            FormsSubmittingEventArgs formsSubmitEvents = e as FormsSubmittingEventArgs;
            NameValueCollection rawFormData = formsSubmitEvents.Data as NameValueCollection;
            FormContainerBlock formBlock = formsSubmitEvents.FormsContent as FormContainerBlock;
            Submission submissionData = formsSubmitEvents.SubmissionData;
            FormParser.ProcessForm(rawFormData, formBlock, submissionData);
        }


        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}