using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Business.Log;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Localization.XmlResources;
using EPiServer.Web.Hosting;
using System.IO;
using System.Collections.Specialized;

namespace BVNetwork.Attend.Business.InitializableModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class AttendLogInitializer : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            EPiServer.DataFactory.Instance.PublishingContent += Instance_PublishingContent;
        }

        public void Uninitialize(InitializationEngine context)
        {
            EPiServer.DataFactory.Instance.PublishingContent -= Instance_PublishingContent;
        }

        void Instance_PublishingContent(object sender, EPiServer.ContentEventArgs e)
        {
            if (e.Content as ParticipantBlock != null) {
                ParticipantBlock participant = (e.Content as ParticipantBlock);
                ParticipantLog.AddLogText("Saved", "Participant saved with e-mail "+participant.Email+" and status "+participant.AttendStatus, participant);
            }
        }

        public void Preload(string[] parameters)
        {
        }

    }
}