using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Models.Blocks
{
    [ContentType(DisplayName = "SessionBlock", GUID = "d39128e9-f956-4f7b-85df-812fe2535f40", Description = "", AvailableInEditMode = false, GroupName = Groups.Attend)]
    public class SessionBlock : BlockData
    {
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual int NumberOfSeats { get; set; }
        public virtual PageReference EventPage { get; set; }
        public virtual bool Mandatory { get; set; }
        public virtual string Description { get; set; }
        public virtual XhtmlString IntroContent { get; set; }
        public virtual string TrackID { get; set; }
    }
}