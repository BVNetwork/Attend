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