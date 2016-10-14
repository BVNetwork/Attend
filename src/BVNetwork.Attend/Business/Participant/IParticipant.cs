using System;
using EPiServer.Core;

namespace BVNetwork.Attend.Business.Participant
{
    public interface IParticipant
    {
        DateTime DateSubmitted { get; set; }

        string Code { get; set; }

        string Email { get; set; }

        string XForm { get; set; }

        PageReference EventPage { get; set; }

        string Username { get; set; }

        string SessionsString { get; set; }

        string AttendStatus { get; set; }

        string AttendStatusText { get; }

        string Log { get; set; }

        int Price { get; set; }

        string Comment { get; set; }

        ContentArea Sessions { get; set; }

    }
}
