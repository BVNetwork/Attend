using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Log
{
    public class ParticipantLog
    {
        public static IParticipant AddLogTextAndSave(string type, string info, IParticipant participant)
        {
            participant = (participant as ParticipantBlock).CreateWritableClone() as ParticipantBlock;
            AddLogText(type, info, participant);
            ServiceLocator.Current.GetInstance<IContentRepository>().Save(participant as IContent, EPiServer.DataAccess.SaveAction.Publish | EPiServer.DataAccess.SaveAction.ForceCurrentVersion, EPiServer.Security.AccessLevel.NoAccess);
            return participant;
        }

        public static void AddLogText(string type, string info, IParticipant participant)
        {
            participant.Log = participant.Log + DateTime.Now.ToString() + "|" + type + "|" + info + ";";
        }

    }
}