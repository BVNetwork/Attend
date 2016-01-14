using System;
using BVNetwork.Attend.Models.Blocks;

namespace BVNetwork.Attend.Business.API
{
    public class RegistrationEventArgs : EventArgs
    {
        public bool Cancel;
        public bool SendMail;
        public ParticipantBlock Participant;
    }
}