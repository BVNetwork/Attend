using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace BVNetwork.Attend.Business.Participant.DDSProvider
{
    public class DDSParticipantProvider : ParticipantProviderBase
    {
        private DynamicDataStore ParticipantDataStore
        {
            get
            {
                return typeof(DDSParticipant).GetStore();
            }
        }


        public override IParticipant GenerateParticipant(EPiServer.Core.ContentReference EventPageBase, string email, bool sendMail, string xform, string logText)
        {
            DDSParticipant newParticipant = new DDSParticipant { AttendStatus = AttendStatus.Confirmed.ToString(), EventPage = EventPageBase as PageReference, Email = email, XForm = xform, DateSubmitted = DateTime.Now };
            newParticipant.Code = newParticipant.Id.ToString();
            newParticipant.EventPageID = EventPageBase.ID;
            ParticipantDataStore.Save(newParticipant);
            return newParticipant;
        }

        public override IEnumerable<IParticipant> GetParticipants(EPiServer.Core.ContentReference EventPage)
        {
            return
                (from participants in ParticipantDataStore.Items<DDSParticipant>()
                    where participants.EventPageID == EventPage.ID select participants);
        }

        public override List<EventPageBase> GetEventPages()
        {
            return new BlockProvider.BlockParticipantProvider().GetEventPages();
        }

        public override string GetEditUrl(IParticipant participant)
        {
            return "/Modules/BVNetwork.Attend/Views/Pages/EditParticipant.aspx?id=" + participant.Code;
        }

        public override void SaveParticipant(IParticipant participant)
        {
            throw new NotImplementedException();
        }

        public override IParticipant GetParticipant(string code)
        {
            throw new NotImplementedException();
        }

        public override List<IParticipant> GetParticipantByEmail(string email)
        {
            throw new NotImplementedException();
        }


        public override IParticipant GetParticipant(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}