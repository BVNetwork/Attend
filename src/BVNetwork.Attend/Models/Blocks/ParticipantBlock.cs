using System;
using System.ComponentModel.DataAnnotations;
using BVNetwork.Attend.Business.Participant;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using BVNetwork.Attend.Business.EditorDescriptors;
using BVNetwork.Attend.Business.Text;
using EPiServer.Web;

namespace BVNetwork.Attend.Models.Blocks
{
    [ImageUrl("~/Modules/BVNetwork.Attend/Static/attend_blue.png")]
    [ContentType(DisplayName = "ParticipantBlock", GUID = "98c6383c-c8fd-40d7-bdeb-202493091482", GroupName=Groups.Attend, Description = "", AvailableInEditMode = false)]
    public class ParticipantBlock : BlockData, IParticipant
    {
        public virtual DateTime DateSubmitted { get; set; }

        public virtual string Code { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [UIHint(UIHint.LongString)]
        public virtual string XForm { get; set; }

        public virtual PageReference EventPage { get; set; }

        public virtual string Username { get; set; }

        public virtual string SessionsString { get; set; }

        [UIHint(EventRenderTags.AttendStatus)]
        public virtual string AttendStatus { get; set; }

        [Ignore]
        public virtual string AttendStatusText { get { return BVNetwork.Attend.Business.Text.AttendStatusText.Get(AttendStatus); } }

        public virtual string Log { get; set; }

        public virtual int Price { get; set; }

        [UIHint(UIHint.Textarea)]
        public virtual string Comment { get; set; }

        [AllowedTypes(typeof(SessionBlock))]
        public virtual ContentArea Sessions { get; set; }

    }
}