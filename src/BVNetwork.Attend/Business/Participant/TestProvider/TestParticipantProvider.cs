using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVNetwork.Attend.Business.Participant.BlockProvider;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Core;

namespace BVNetwork.Attend.Business.Participant.TestProvider
{
    public class TestParticipantProvider : ParticipantProviderBase
    {
        public override IParticipant GenerateParticipant(EPiServer.Core.ContentReference EventPage, string email, bool sendMail, string xform, string logText)
        {
            return new TestParticipant { AttendStatus = AttendStatus.Confirmed.ToString(), Code = "123", EventPage = EventPage as PageReference, Email = email, XForm = xform, DateSubmitted = DateTime.Now };
        }

        public override IEnumerable<IParticipant> GetParticipants(EPiServer.Core.ContentReference EventPageBase)
        {
            List<IParticipant> participants = new List<IParticipant>();
            for(int i = 0; i < 10; i++)
                participants.Add(GenerateParticipant(EventPageBase, "test"+i+"@test.com", false, null, "Created test participant"));
            return participants;
        }

        public override List<EventPageBase> GetEventPages()
        {
            return new BlockParticipantProvider().GetEventPages();
        }

        public override string GetEditUrl(IParticipant participant)
        {
            return "/Modules/BVNetwork.Attend/Views/Pages/EditParticipant.aspx?id="+participant.Code;
        }


        public override void SaveParticipant(IParticipant participant)
        {
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