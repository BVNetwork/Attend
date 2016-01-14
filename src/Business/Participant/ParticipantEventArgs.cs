using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Participant
{
    public class ParticipantEventArgs
    {
        public string ParticipantID;
        public IParticipant CurrentParticipant;
        public bool CancelEvent;
        public bool SendMail;

    }
}